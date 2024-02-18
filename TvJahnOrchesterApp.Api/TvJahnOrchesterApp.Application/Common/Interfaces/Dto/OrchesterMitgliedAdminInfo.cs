using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.UserAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Dto
{
    public record OrchesterMitgliedAdminInfo(OrchesterMitgliedsId Id, string Vorname, string Nachname, string? ConnectedUserId, DateTime? UserLastLogin);
}
