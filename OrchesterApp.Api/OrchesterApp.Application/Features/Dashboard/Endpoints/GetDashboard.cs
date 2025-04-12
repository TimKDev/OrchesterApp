using MediatR;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;
using TvJahnOrchesterApp.Application.Features.Dropdown.Services;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Common.Services;

namespace TvJahnOrchesterApp.Application.Features.Dashboard.Endpoints
{
    public static class GetDashboard
    {
        public static void MapGetDashboardEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/dashboard", GetDashboardData)
                .RequireAuthorization();
        }

        private static async Task<DashboardData> GetDashboardData(CancellationToken cancellationToken, ISender sender)
        {
            return await sender.Send(new GetDashboardDataQuery());
        }

        public record TerminOverview(Guid TerminId, string Name, int? TerminArt, DateTime StartZeit, DateTime EndZeit, int Zugesagt);
        public record BirthdayListEntry(string Name, string? Image, DateTime Birthday);
        public record DashboardData(TerminOverview[] NextTermins, DropdownItem[] RückmeldungsDropdownItems, DropdownItem[] TerminArtDropdownItems, BirthdayListEntry[] BirthdayList);

        private record GetDashboardDataQuery() : IRequest<DashboardData>;

        private class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, DashboardData>
        {
            private const int DAYS_TO_INCLUDE_TERMIN = 14;
            private const int DAYS_TO_INCLUDE_BIRTHDAY_FUTURE = 7;
            private const int DAYS_TO_INCLUDE_BIRTHDAY_PAST = 14;
            private readonly IDropdownService dropdownService;
            private readonly ITerminRepository terminRepository;
            private readonly ICurrentUserService currentUserService;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

            public GetDashboardDataQueryHandler(ITerminRepository terminRepository, ICurrentUserService currentUserService, IDropdownService dropdownService, IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                this.terminRepository = terminRepository;
                this.currentUserService = currentUserService;
                this.dropdownService = dropdownService;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<DashboardData> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
            {
                // Response Dropdown Values:
                var responseDropdownValues = await dropdownService.GetAllDropdownValuesAsync(DropdownNames.Rückmeldungsart, cancellationToken);
                var terminArtDropdownValues = await dropdownService.GetAllDropdownValuesAsync(DropdownNames.TerminArten, cancellationToken);
                // Next Termin Values:
                var terminsInNextDays = (await terminRepository.GetAll(cancellationToken)).Where(IsInDayRanch);
                var currentOrchesterMember = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
                var terminsForCurrentUser = terminsInNextDays.Where(t => t.IstZugeordnet(currentOrchesterMember.Id));
                var nextTermins = terminsForCurrentUser.Select(x => new TerminOverview(x.Id.Value, x.Name, x.TerminArt, x.EinsatzPlan.StartZeit, x.EinsatzPlan.EndZeit, x.TerminRückmeldungOrchesterMitglieder.First(r => r.OrchesterMitgliedsId == currentOrchesterMember.Id).Zugesagt)
                );
                // Next Birthdays:
                var orchesterMembersWithBirthdayInNextDays = (await orchesterMitgliedRepository.GetAllAsync(cancellationToken)).Where(IsInDayRanch).Select(TransformToBirthdayListEntry).OrderBy(o => o.Birthday);

                return new DashboardData(nextTermins.OrderBy(termin => termin.StartZeit).ToArray(), responseDropdownValues, terminArtDropdownValues, orchesterMembersWithBirthdayInNextDays.ToArray());
            }

            private bool IsInDayRanch(OrchesterApp.Domain.TerminAggregate.Termin termin)
            {
                if(termin.EinsatzPlan.EndZeit < DateTime.UtcNow)
                {
                    return false;
                }
                var timespan = termin.EinsatzPlan.StartZeit - DateTime.UtcNow;
                return 0 <= timespan.Days && timespan.Days <= DAYS_TO_INCLUDE_TERMIN;
            }

            private bool IsInDayRanch(OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied orchesterMitglied)
            {
                if (orchesterMitglied.Geburtstag is null)
                {
                    return false;
                }
                var projectedBirthdaySameYear = new DateTime(DateTime.UtcNow.Year, orchesterMitglied.Geburtstag.Value.Month, orchesterMitglied.Geburtstag.Value.Day);

                var projectedBirthdayPreviousYear = new DateTime(DateTime.UtcNow.Year - 1, orchesterMitglied.Geburtstag.Value.Month, orchesterMitglied.Geburtstag.Value.Day);

                var projectedBirthdayNextYear = new DateTime(DateTime.UtcNow.Year + 1, orchesterMitglied.Geburtstag.Value.Month, orchesterMitglied.Geburtstag.Value.Day);

                var timespanSameYear = (projectedBirthdaySameYear - DateTime.UtcNow);
                var timespanPreviousYear = (projectedBirthdayPreviousYear - DateTime.UtcNow);
                var timespanNextYear = (projectedBirthdayNextYear - DateTime.UtcNow);

                var checkForSameYear = -DAYS_TO_INCLUDE_BIRTHDAY_PAST <= timespanSameYear.Days && timespanSameYear.Days <= DAYS_TO_INCLUDE_BIRTHDAY_FUTURE;
                var checkForPreviousYear = -DAYS_TO_INCLUDE_BIRTHDAY_PAST <= timespanPreviousYear.Days && timespanPreviousYear.Days <= DAYS_TO_INCLUDE_BIRTHDAY_FUTURE;
                var checkForNextYear = -DAYS_TO_INCLUDE_BIRTHDAY_PAST <= timespanNextYear.Days && timespanNextYear.Days <= DAYS_TO_INCLUDE_BIRTHDAY_FUTURE;

                return checkForSameYear || checkForPreviousYear || checkForNextYear;
            }

            private BirthdayListEntry TransformToBirthdayListEntry(OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied orchesterMitglied)
            {
                var projectedBirthdaySameYear = new DateTime(DateTime.UtcNow.Year, orchesterMitglied.Geburtstag.Value.Month, orchesterMitglied.Geburtstag.Value.Day);

                var projectedBirthdayPreviousYear = new DateTime(DateTime.UtcNow.Year - 1, orchesterMitglied.Geburtstag.Value.Month, orchesterMitglied.Geburtstag.Value.Day);

                var projectedBirthdayNextYear = new DateTime(DateTime.UtcNow.Year + 1, orchesterMitglied.Geburtstag.Value.Month, orchesterMitglied.Geburtstag.Value.Day);

                var timespanSameYear = (projectedBirthdaySameYear - DateTime.UtcNow);
                var timespanPreviousYear = (projectedBirthdayPreviousYear - DateTime.UtcNow);
                var timespanNextYear = (projectedBirthdayNextYear - DateTime.UtcNow);

                var checkForSameYear = -DAYS_TO_INCLUDE_BIRTHDAY_PAST <= timespanSameYear.Days && timespanSameYear.Days <= DAYS_TO_INCLUDE_BIRTHDAY_FUTURE;
                var checkForPreviousYear = -DAYS_TO_INCLUDE_BIRTHDAY_PAST <= timespanPreviousYear.Days && timespanPreviousYear.Days <= DAYS_TO_INCLUDE_BIRTHDAY_FUTURE;
                var checkForNextYear = -DAYS_TO_INCLUDE_BIRTHDAY_PAST <= timespanNextYear.Days && timespanNextYear.Days <= DAYS_TO_INCLUDE_BIRTHDAY_FUTURE;

                var projectedBirthday = orchesterMitglied.Geburtstag!.Value;

                if (checkForSameYear)
                {
                    projectedBirthday = projectedBirthdaySameYear;
                }
                if (checkForPreviousYear)
                {
                    projectedBirthday = projectedBirthdayPreviousYear;
                }
                if (checkForNextYear)
                {
                    projectedBirthday = projectedBirthdayNextYear;
                }

                return new BirthdayListEntry($"{orchesterMitglied.Vorname} {orchesterMitglied.Nachname}", TransformImageService.ConvertByteArrayToBase64(orchesterMitglied.Image), projectedBirthday);
            }
        }
    }
}
