using Mapster;
using TvJahnOrchesterApp.Application.Termin.Commands.Create;
using TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanUpdate;
using TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanZeitblockUpdate;
using TvJahnOrchesterApp.Application.Termin.Commands.Rückmeldung;
using TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungChangeInstrumentsAndNotes;
using TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungForOtherUser;
using TvJahnOrchesterApp.Application.Termin.Commands.Update;
using TvJahnOrchesterApp.Application.Termin.Commands.UpdateAnwesenheit;
using TvJahnOrchesterApp.Application.Termin.Common;
using TvJahnOrchesterApp.Contracts.Termine.AnwesenheitsListe;
using TvJahnOrchesterApp.Contracts.Termine.Dto;
using TvJahnOrchesterApp.Contracts.Termine.Einsatzplan;
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

            config.NewConfig<EinsatzPlan, EinsatzPlanDto>();

            config.NewConfig<ZeitBlock, ZeitBlockDto>()
                .Map(d => d.ZeitBlockId, s => s.Id.Value);

            config.NewConfig<(Termin termin, TerminRückmeldungOrchestermitglied rückmeldung), GetTerminResponse>()
                .Map(d => d.TerminId, s => s.termin.Id.Value)
                .Map(d => d.UserRückmeldung, s => s.rückmeldung)
                .Map(d => d.UserRückmeldung.RückmeldungsId, s => s.rückmeldung.Id.Value)
                .Map(d => d, s => s.termin);

            config.NewConfig<(Termin termin, TerminRückmeldungOrchestermitglied rückmeldung), GetAllTerminResponse>()
                .Map(d => d.TerminId, s => s.termin.Id.Value)
                .Map(d => d.StartZeit, s => s.termin.EinsatzPlan.StartZeit)
                .Map(d => d.EndZeit, s => s.termin.EinsatzPlan.EndZeit)
                .Map(d => d.IstAnwesend, s => s.rückmeldung.IstAnwesend)
                .Map(d => d.Zugesagt, s => s.rückmeldung.Zugesagt);

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

            config.NewConfig<UpdateAnwesenheitsListeResponse, UpdateTerminAnwesenheitsListenResponse>();

            config.NewConfig<GlobalAnwesenheitsListe, GetAnwesenheitsListeResponse>()
                .Map(d => d.GlobalAnwesenheitsListe, s => s.GlobalAnwesenheitsListenEinträge);

            config.NewConfig<(UpdateEinsatzplanRequest request, Guid terminId), EinsatzplanUpdateCommand>()
                .Map(d => d.TerminId, s => s.terminId)
                .Map(d => d, s => s.request);

            config.NewConfig<UpdateEinsatzplanRequest, UpdateEinsatzplanResponse>();

            config.NewConfig<(UpdateCreateZeitblockRequest request, Guid terminId), EinsatzplanZeitblockUpdateCommand>()
                .Map(d => d.TerminId, s => s.terminId)
                .Map(d => d, s => s.request);

            config.NewConfig<UpdateCreateZeitblockRequest, UpdateCreateZeitblockResponse>();

            config.NewConfig<TerminRückmeldungsResponse, GetRückmeldungenTerminResponse>()
                .Map(d => d.TerminRückmeldungOrchesterMitglieder, s => s.TerminRückmeldungOrchestermitglied);

            config.NewConfig<(string Vorname, string Nachname, string? VornameOther, string? NachnameOther, TerminRückmeldungOrchestermitglied TerminRückmeldungOrchestermitglied), TerminRückmeldungOrchestermitgliedDto>()
                .Map(d => d.RückmeldungsId, s => s.TerminRückmeldungOrchestermitglied.Id.Value)
                .Map(d => d, s => s.TerminRückmeldungOrchestermitglied)
                .Map(d => d.Vorname, s => s.Vorname)
                .Map(d => d.Nachname, s => s.Nachname)
                .Map(d => d.VornameRückmelder, s => s.VornameOther)
                .Map(d => d.NachnameRückmelder, s => s.NachnameOther);

            config.NewConfig<(Guid terminId, RückmeldungenChangeRequest request), RückmeldungChangeInstrumentsAndNotesCommand>()
                .Map(d => d.TerminId, s => s.terminId)
                .Map(d => d, s => s.request);

        }
    }
}
