using Mapster;
using TvJahnOrchesterApp.Application.Termin.Commands.Create;
using TvJahnOrchesterApp.Application.Termin.Commands.Rückmeldung;
using TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungForOtherUser;
using TvJahnOrchesterApp.Application.Termin.Commands.Update;
using TvJahnOrchesterApp.Application.Termin.Commands.UpdateAnwesenheit;
using TvJahnOrchesterApp.Application.Termin.Common;
using TvJahnOrchesterApp.Contracts.Termine.AnwesenheitsListe;
using TvJahnOrchesterApp.Contracts.Termine.Dto;
using TvJahnOrchesterApp.Contracts.Termine.Main;
using TvJahnOrchesterApp.Contracts.Termine.Rückmeldung;
using TvJahnOrchesterApp.Domain.TerminAggregate;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Api.Common.Mapping
{
    public class TerminMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateTerminRequest, CreateTerminCommand>();

            config.NewConfig<Termin, CreateTerminResponse>()
                .Map(d => d.TerminId, s => s.Id.Value)
                .Map(d => d.OrchestermitgliedIds, s => s.TerminRückmeldungOrchesterMitglieder.Select(e => e.OrchesterMitgliedsId.Value).ToArray());

            config.NewConfig< (string Vorname, string Nachname, string? VornameOther, string? NachnameOther, TerminRückmeldungOrchestermitglied TerminRückmeldungOrchestermitglied), TerminRückmeldungOrchestermitgliedDto>()
                .Map(d => d, s => s.TerminRückmeldungOrchestermitglied)
                .Map(d => d.Vorname, s => s.Vorname)
                .Map(d => d.Nachname, s => s.Nachname)
                .Map(d => d.VornameRückmelder, s => s.VornameOther)
                .Map(d => d.NachnameRückmelder, s => s.NachnameOther);

            config.NewConfig<EinsatzPlan, EinsatzPlanDto>();

            config.NewConfig<TerminResponse, GetTerminResponse>()
                .Map(d => d.TerminId, s => s.Termin.Id.Value)
                .Map(d => d, s => s.Termin)
                .Map(d => d.TerminRückmeldungOrchesterMitglieder, s => s.TerminRückmeldungOrchestermitglied);

            config.NewConfig<Termin, GetAllTerminResponse>()
                .Map(d => d.TerminId, s => s.Id.Value)
                .Map(d => d.StartZeit, s => s.EinsatzPlan.StartZeit)
                .Map(d => d.EndZeit, s => s.EinsatzPlan.EndZeit);

            config.NewConfig<UpdateTerminRequest, UpdateTerminCommand>();

            config.NewConfig<Termin, UpdateTerminResponse>()
                .Map(d => d.TerminId, s => s.Id.Value)
                .Map(d => d.OrchestermitgliedIds, s => s.TerminRückmeldungOrchesterMitglieder.Select(e => e.OrchesterMitgliedsId.Value).ToArray());

            config.NewConfig<RückmeldungTerminRequest, RückmeldungCommand>();

            config.NewConfig<RückmeldungsResponse, RückmeldungTerminResponse>()
                .Map(d => d.Vorname, s => s.Orchestermitglied.Vorname)
                .Map(d => d.Nachname, s => s.Orchestermitglied.Nachname)
                .Map(d => d.OrchesterMitgliedsId, s => s.Orchestermitglied.Id.Value)
                .Map(d => d.TerminId, s => s.TerminId.Value);

            config.NewConfig<RückmeldungTerminForOtherUserRequest, RückmeldungForOtherUserCommand>();

            config.NewConfig<TerminAnwesenheitsListeResponse, GetTerminAnwesenheitsListeResponse>();

            config.NewConfig<TerminAnwesenheitsListenEintrag, TerminAnwesenheitsListenEintragDto>()
                .Map(d => d.OrchesterMitgliedsId, s => s.TerminRückmeldungOrchestermitglied.OrchesterMitgliedsId.Value)
                .Map(d => d.Anwesend, s => s.TerminRückmeldungOrchestermitglied.IstAnwesend)
                .Map(d => d.Kommentar, s => s.TerminRückmeldungOrchestermitglied.KommentarAnwesenheit);

            config.NewConfig<UpdateTerminAnwesenheitsListenRequest, UpdateAnwesenheitCommand>()
                .Map(d => d.UpdateAnwesenheitsListe, s => s.TerminAnwesenheitsListe);

        }
    }
}
