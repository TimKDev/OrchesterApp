# Feature Implementation: Termin Deadline and Reminder Notification System

## Overview
Implement a comprehensive deadline and reminder notification system for Termine (appointments/events) that automatically notifies users when they haven't responded to event invitations. 

Important: For new endpoint implementation always follow the existing structure of files in which endpoints are defined! And in general try to follow the style of the current implementation when adding new stuff. Do not write comments and try to make the code as readable as possible.

## Context
This is a C# .NET application using:
- Domain-Driven Design (DDD) architecture
- Entity Framework Core for persistence
- MediatR for CQRS pattern
- Minimal API endpoints
- Background services for scheduled tasks

The application manages orchestra events (Termine) and member responses (Rückmeldungen).

---

## Step 1: Extend Termin Domain Model with Deadline Properties

**Location**: `OrchesterApp.Domain/TerminAggregate/Termin.cs`

### Task:
Add two new nullable DateTime properties to the `Termin` aggregate:

1. **Frist** (Deadline): 
   - Type: `TimeSpan?`
   - Purpose: Starttime minus this value Defines when the deadline is due. 
   - If null: No deadline (e.g., casual events like BBQs)
   
2. **ErsteWarnungVorFrist** (First Warning Before Deadline):
   - Type: `TimeSpan?`
   - Purpose: StartTime minus this value defines when the warning notification is send 
   - If null: No warning will be sent

### Implementation Details:
- Add private setters for encapsulation
- Add these properties as read-only public properties
- Update the private constructor to include these parameters
- Modify the `Create` static factory method to accept these parameters with defaults:
  - For rehearsals (Proben): `Frist = 1 day`, `ErsteWarnungVorFrist = null`
  - For performances (Auftritte): `Frist = 2 months`, `ErsteWarnungVorFrist = 1 week before deadline`
- Add `Update` methods: `UpdateFrist(DateTime? frist)` and `UpdateErsteWarnungVorFrist(DateTime? warnung)`

### Default Logic:
Determine the event type based on `TerminArt` property to set appropriate defaults. If `TerminArt` is not sufficient, accept explicit parameters or use reasonable defaults.

---

## Step 2: Update Termin Entity Configuration for Persistence

**Location**: `OrchesterApp.Infrastructure/Persistence/Configurations/TerminConfiguration.cs`

### Task:
Add Entity Framework configuration for the new properties:

```csharp
builder.Property(t => t.Frist)
    .IsRequired(false);

builder.Property(t => t.ErsteWarnungVorFrist)
    .IsRequired(false);
```

---

## Step 3: Create Database Migration

**Location**: `OrchesterApp.Infrastructure/Migrations/`

### Task:
Generate a new Entity Framework migration for the schema changes:

1. Run the migration command to add the new columns
2. Ensure the migration handles existing data appropriately (nulls are acceptable)
3. Name the migration descriptively: `AddTerminDeadlineFields`

---

## Step 4: Create New Notification Types

### 4.1: Add Notification Categories

**Location**: `OrchesterApp.Domain/NotificationAggregate/Enums/NotificationCategory.cs`

Add two new enum values:
```csharp
TerminReminderBeforeDeadline,
TerminMissingResponse
```

### 4.2: Create TerminReminderNotification Class

**Location**: `OrchesterApp.Domain/NotificationAggregate/Notifications/TerminReminderNotification.cs`

**Purpose**: Notification sent as a warning before the deadline expires.

**Properties**:
- `string TerminName` - Name of the event
- `DateTime TerminDate` - Start date/time of the event  
- `DateTime TerminDeadline` - Deadline for response
- `TerminId TerminId` - Link to event details

**Methods**:
1. `static TerminReminderNotification New(...)` - Factory method to create new instance
2. `static TerminReminderNotification Create(Notification notification)` - Factory for deserialization
3. `PortalNotificationContent GetPortalNotificationContent()` - Generate display content

**Implementation Pattern**:
Follow the existing pattern from `ChangeTerminDataNotification`:
- Use a private serializable DTO for the `Data` property
- Serialize metadata to JSON in the `Data` field
- Include proper inheritance from `Notification` base class
- Use `NotificationType.Warning`, `NotificationCategory.TerminReminderBeforeDeadline`, `NotificationUrgency.High`

### 4.3: Create TerminMissingResponseNotification Class

**Location**: `OrchesterApp.Domain/NotificationAggregate/Notifications/TerminMissingResponseNotification.cs`

**Purpose**: Notification sent when the deadline has expired without a response.

**Properties**:
- `string TerminName` - Name of the event
- `DateTime TerminDate` - Start date/time of the event
- `TerminId TerminId` - Link to event details

**Methods**:
1. `static TerminMissingResponseNotification New(...)` - Factory method
2. `static TerminMissingResponseNotification Create(Notification notification)` - Deserialization factory
3. `PortalNotificationContent GetPortalNotificationContent()` - Generate display content

**Implementation Pattern**:
- Follow same pattern as TerminReminderNotification
- Use `NotificationType.Warning`, `NotificationCategory.TerminMissingResponse`, `NotificationUrgency.Medium`

### 4.4: Update NotificationFactory

**Location**: `OrchesterApp.Domain/NotificationAggregate/NotificationFactory.cs`

Add switch cases for the new notification categories:
```csharp
NotificationCategory.TerminReminderBeforeDeadline => TerminReminderNotification.Create(notification),
NotificationCategory.TerminMissingResponse => TerminMissingResponseNotification.Create(notification),
```

---

## Step 5: Update API Contracts for Create/Update Endpoints

### 5.1: Update CreateTerminCommand

**Location**: `OrchesterApp.Application/Features/Termin/Endpoints/CreateTermin.cs`

Add to the `CreateTerminCommand` record:
```csharp
DateTime? Frist,
DateTime? ErsteWarnungVorFrist
```

Update the handler to pass these values to `Termin.Create()`.

### 5.2: Update UpdateTerminCommand

**Location**: `OrchesterApp.Application/Features/Termin/Endpoints/UpdateTermin.cs`

Add to the `UpdateTerminCommand` record:
```csharp
DateTime? Frist,
DateTime? ErsteWarnungVorFrist
```

Update the handler to call:
```csharp
termin.UpdateFrist(request.Frist);
termin.UpdateErsteWarnungVorFrist(request.ErsteWarnungVorFrist);
```

### 5.3: Update Response DTOs

**Location**: `OrchesterApp.Contracts/Termine/`

Update relevant response DTOs to include the new fields so the frontend can display and edit them.

---

## Step 6: Create Background Service for Deadline Checking

**Location**: `OrchesterApp.Application/Common/Services/TerminDeadlineCheckService.cs`

### Purpose:
Periodically check all events to determine if reminders or missing response notifications need to be created.

### Implementation Requirements:

1. **Inherit from**: `BackgroundService`
2. **Inject Dependencies**:
   - `IServiceProvider` (to create scoped services)
   - `ILogger<TerminDeadlineCheckService>`

3. **ExecuteAsync Logic**:
   ```csharp
   protected override async Task ExecuteAsync(CancellationToken stoppingToken)
   {
       while (!stoppingToken.IsCancellationRequested)
       {
           await CheckTerminDeadlinesAsync(stoppingToken);
           await Task.Delay(TimeSpan.FromHours(4), stoppingToken);
       }
   }
   ```

4. **Create CheckTerminDeadlinesAsync Method**:
   - Create a service scope
   - Get required repositories (ITerminRepository, etc.)
   - Query all Termine for the current year
   - For each Termin that is in the future:
     - Check each member's response status
     - For members with `RückmeldungsartEnum.NichtZurückgemeldet`:
       - If `ErsteWarnungVorFrist` exists and current time >= warning time and < deadline: Create reminder notification (only once)
       - If `Frist` exists and current time >= deadline: Create missing response notification (only once)

5. **Notification Creation Logic**:
   - Use `INotifyService.PublishNotificationAsync()` to create notifications
   - Target only members who haven't responded
   - Use both Portal and Email notification sinks

6. **Prevent Duplicate Notifications**:
   - Track which notifications have been sent (store in database or check if notification already exists)
   - Option A: Add a repository method to check if notification of specific category exists for user+termin
   - Option B: Add tracking fields to Termin or create a separate tracking entity

---

## Step 7: Create Manual Trigger Endpoint

**Location**: `OrchesterApp.Application/Features/Termin/Endpoints/TriggerDeadlineCheck.cs`

### Purpose:
Allow administrators to manually trigger the deadline check process.

### Implementation:

Strictly follow the structure of other endpoint definitions. 

1. **Create Minimal API Endpoint**:
   ```csharp
   app.MapPost("api/termin/check-deadlines", TriggerDeadlineCheck)
       .RequireAuthorization(r => r.RequireRole(RoleNames.Admin));
   ```

2. **Create Command and Handler**:
   - Command: `TriggerDeadlineCheckCommand` (can be empty record)
   - Handler: Extract the checking logic into a shared service or directly invoke the same logic used by the background service

3. **Return Response**:
   - Return count of notifications created
   - Include breakdown: reminders sent, missing response notifications sent

### Approach:
Create an interface `ITerminDeadlineCheckService` with method `Task<DeadlineCheckResult> CheckTerminDeadlinesAsync(CancellationToken)`. Implement this in a service class that both the background service and the endpoint handler can use.

---

## Step 8: Register Services

### 8.1: Register Background Service

**Location**: `OrchesterApp.Application/Common/DependencyInjection.cs`

Add to the `AddCommon` method:
```csharp
services.AddSingleton<TerminDeadlineCheckService>();
services.AddHostedService<TerminDeadlineCheckService>(provider =>
    provider.GetRequiredService<TerminDeadlineCheckService>());
```

### 8.2: Register Check Service (if created as separate interface)

If you created `ITerminDeadlineCheckService`:
```csharp
services.AddScoped<ITerminDeadlineCheckService, TerminDeadlineCheckServiceImpl>();
```

### 8.3: Register Endpoint

**Location**: `OrchesterApp.Application/Features/Termin/RegisterEndpoints.cs`

Add:
```csharp
app.MapTriggerDeadlineCheckEndpoint();
```

## Step 10: Frontend Integration Guidance

**Location**: Frontend code (Angular/Ionic)

### Updates Required:

1. **Create/Edit Termin Forms**:
   - Add date-time picker for `Frist` (Deadline)
   - Add date-time picker for `ErsteWarnungVorFrist` (First Warning)
   - Add helper text explaining defaults based on event type
   - Add validation: Warning must be before deadline

2. **Admin Settings Page**:
   - Add button "Check Deadlines Manually"
   - Call the `/api/termin/check-deadlines` endpoint
   - Display success message with notification counts

3. **Termin Detail View**:
   - Display deadline and warning time (if set)
   - Show countdown or indication if deadline is approaching

---

## Step 11: Testing Checklist

### Unit Tests:
- [ ] Termin domain model with new properties
- [ ] Notification creation logic
- [ ] Deadline calculation logic
- [ ] Default value assignment based on event type

### Integration Tests:
- [ ] Background service execution
- [ ] Manual trigger endpoint
- [ ] Notification creation and distribution
- [ ] Duplicate notification prevention

### Manual Testing:
- [ ] Create Termin with deadline - verify defaults
- [ ] Create Termin without deadline (null)
- [ ] Update existing Termin to add/modify deadlines
- [ ] Verify reminder notification at warning time
- [ ] Verify missing response notification after deadline
- [ ] Verify no duplicate notifications
- [ ] Verify manual trigger works for admins only
- [ ] Verify notifications appear in portal
- [ ] Verify emails are sent (if configured)

---

## Step 12: Database Seeding/Migration Considerations

### For Existing Data:
- Existing Termine will have null values for both new fields (acceptable)
- Consider creating a data migration script to set defaults for future events based on their TerminArt
- Document that historical events won't have deadlines unless manually updated

---

## Implementation Order Summary

1. **Domain Layer**: Add properties to Termin aggregate + update methods
2. **Infrastructure Layer**: Update EF configuration + create migration
3. **Domain Layer**: Create notification classes and update factory
4. **Application Layer**: Update Create/Update endpoints
5. **Application Layer**: Create deadline check service (shared logic)
6. **Application Layer**: Create background service
7. **Application Layer**: Create manual trigger endpoint
8. **Application Layer**: Register all services and endpoints
9. **Contracts Layer**: Update DTOs
10. **Testing**: Verify all functionality

---

## Key Design Decisions

1. **Nullable DateTimes**: Allow flexibility for events without deadlines
2. **Background Service Interval**: 4 hours provides reasonable responsiveness without excessive load
3. **Notification Targets**: Only send to members who haven't responded
4. **Duplicate Prevention**: Essential to avoid notification spam
5. **Admin-Only Manual Trigger**: Prevents abuse and resource exhaustion
6. **Year-Based Query**: Limits scope to current year for performance

---

## Success Criteria

- ✅ Termine can be created and updated with deadline fields
- ✅ Reminder notifications are sent at the warning time
- ✅ Missing response notifications are sent after deadline
- ✅ No duplicate notifications are sent
- ✅ Background service runs every 4 hours
- ✅ Admins can manually trigger the check
- ✅ Notifications contain event details and links
- ✅ System handles null deadlines gracefully
- ✅ Defaults are applied based on event type

---

## Notes

- The system should be resilient to clock changes and time zones (use UTC)
- Consider adding logging for notification creation
- Monitor background service performance with large datasets
- Consider adding configuration for check interval (currently hardcoded to 4 hours)
- Ensure the link in notifications routes correctly to the event detail page in the frontend


