using TvJahnOrchesterApp.Domain.AbstimmungsAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;
using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.TerminAggregate
{
    public sealed class Termin: AggregateRoot<TerminId, Guid>
    {
        private readonly List<RückgemeldetePerson> _rückgemeldetePersonen = new();
        private readonly List<OrchesterMitgliedsId> _nichtZurückgemeldeteOrchesterMitglieder = new();
        private readonly List<AnwesenheitsListenEintrag> _anwesenheitsListenEinträge= new();

        public string Name { get; private set; } = null!;
        public TerminGruppe TerminGruppe { get; private set; }
        public TerminArt TerminArt { get; private set; }
        public EinsatzPlan EinsatzPlan { get; private set; } = null!;
        public IReadOnlyList<RückgemeldetePerson> RückgemeldetePersonen => _rückgemeldetePersonen.AsReadOnly();
        public IReadOnlyList<OrchesterMitgliedsId> OrchesterMitgliedsIds => _nichtZurückgemeldeteOrchesterMitglieder.AsReadOnly();
        public IReadOnlyList<AnwesenheitsListenEintrag> AnwesenheitsListenEintrags => _anwesenheitsListenEinträge.AsReadOnly(); 
        public AbstimmungsId? AbstimmungsId { get; private set; }

        private Termin() { }

        private Termin(TerminId id, OrchesterMitgliedsId[] orchesterMitgliedsIds, string name, TerminGruppe terminGruppe, TerminArt terminArt, EinsatzPlan einsatzPlan, AbstimmungsId? abstimmungsId = null): base(id)
        {
            Name = name;
            TerminGruppe = terminGruppe;
            TerminArt = terminArt;
            EinsatzPlan = einsatzPlan;
            AbstimmungsId = abstimmungsId;

            foreach (var orchesterMitgliedsId in orchesterMitgliedsIds)
            {
                _nichtZurückgemeldeteOrchesterMitglieder.Add(orchesterMitgliedsId);
                _anwesenheitsListenEinträge.Add(AnwesenheitsListenEintrag.Create(orchesterMitgliedsId));
            }
        }

        public static Termin Create(OrchesterMitgliedsId[] orchesterMitgliedsIds, string name, TerminGruppe terminGruppe, TerminArt terminArt, DateTime startZeit, DateTime endZeit, Adresse treffPunkt,  AbstimmungsId? abstimmungsId = null, string? zusätzlicheInfo = null)
        {
            var einsatzplan = EinsatzPlan.Create(startZeit, endZeit, treffPunkt, zusätzlicheInfo);

            return new Termin(TerminId.CreateUnique(), orchesterMitgliedsIds, name, terminGruppe, terminArt, einsatzplan, abstimmungsId);
        }

        public void Zusagen(OrchesterMitgliedsId orchesterMitgliedsId, List<Instrument> instruments, List<Notenstimme> notenstimmen)
        {
            if (!_nichtZurückgemeldeteOrchesterMitglieder.Remove(orchesterMitgliedsId))
            {
                throw new ArgumentException("Orchestermitglied kann dem Termin nicht zugeordnet werden.");
            }
            _rückgemeldetePersonen.Add(RückgemeldetePerson.Create(orchesterMitgliedsId, instruments, notenstimmen, true));
        }

        public void Absagen(OrchesterMitgliedsId orchesterMitgliedsId, List<Instrument> instruments, List<Notenstimme> notenstimmen)
        {
            if (!_nichtZurückgemeldeteOrchesterMitglieder.Remove(orchesterMitgliedsId))
            {
                var rückgemeldetePerson = _rückgemeldetePersonen.FirstOrDefault(p => p.OrchesterMitgliedsId == orchesterMitgliedsId);
                if(rückgemeldetePerson is null)
                {
                    throw new ArgumentException("Orchestermitglied kann dem Termin nicht zugeordnet werden.");
                }
                rückgemeldetePerson.ChangeZusage(false);
            }
            else
            {
                _rückgemeldetePersonen.Add(RückgemeldetePerson.Create(orchesterMitgliedsId, instruments, notenstimmen, false));
            }
        }



    }
}
