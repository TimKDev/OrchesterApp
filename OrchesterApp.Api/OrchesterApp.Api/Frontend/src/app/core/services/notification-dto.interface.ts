import { NotificationType } from "./notification-type.enum";
import { NotificationUrgency } from "./notification-urgency.enum";

export interface NotificationDto {
  id: string;
  title: string;
  message: string;
  isRead: boolean;
  type: NotificationType;
  urgency: NotificationUrgency;
  terminId?: string;
  createdAt: Date;
}