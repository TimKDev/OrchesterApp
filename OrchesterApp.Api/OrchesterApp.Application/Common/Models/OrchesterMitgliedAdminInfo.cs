using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using OrchesterApp.Domain.UserAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Dto
{
    public record OrchesterMitgliedAdminInfo(OrchesterMitgliedsId Id, string Vorname, string Nachname, string? ConnectedUserId, DateTime? UserLastLogin);
}
