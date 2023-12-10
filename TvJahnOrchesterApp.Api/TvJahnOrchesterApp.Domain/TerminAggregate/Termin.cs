using TvJahnOrchesterApp.Domain.AbstimmungsAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.TerminAggregate
{
    public sealed class Termin: AggregateRoot<TerminId, Guid>
    {
        private readonly List<TerminRückmeldungOrchestermitglied> _terminRückmeldungOrchesterMitglieder = new();

        public string Name { get; private set; } = null!;
        public int? TerminArt { get; private set; }
        public int? TerminStatus { get; private set; }
        public EinsatzPlan EinsatzPlan { get; private set; } = null!;
        public IReadOnlyList<TerminRückmeldungOrchestermitglied> TerminRückmeldungOrchesterMitglieder => _terminRückmeldungOrchesterMitglieder.AsReadOnly();
        public AbstimmungsId? AbstimmungsId { get; private set; }

        private Termin() { }

        private Termin(TerminId id, TerminRückmeldungOrchestermitglied[] terminRückmeldungOrchesterMitglieder, string name, int terminArt, EinsatzPlan einsatzPlan, int terminStatus, AbstimmungsId? abstimmungsId = null): base(id)
        {
            Name = name;
            TerminArt = terminArt;
            EinsatzPlan = einsatzPlan;
            AbstimmungsId = abstimmungsId;
            _terminRückmeldungOrchesterMitglieder = terminRückmeldungOrchesterMitglieder.ToList();
            TerminStatus = terminStatus;
        }

        public static Termin Create(TerminRückmeldungOrchestermitglied[] terminRückmeldungOrchesterMitglieder, string name, int terminArt, DateTime startZeit, DateTime endZeit, Adresse treffPunkt, List<int> noten, List<int> uniform, AbstimmungsId? abstimmungsId = null,TerminStatusEnum terminStatus = TerminStatusEnum.Zugesagt, string? zusätzlicheInfo = null)
        {
            //TTODO: Noten und Uniform über Methoden hinzufügen
            var einsatzplan = EinsatzPlan.Create(startZeit, endZeit, treffPunkt, zusätzlicheInfo);

            return new Termin(TerminId.CreateUnique(), terminRückmeldungOrchesterMitglieder, name, terminArt, einsatzplan,(int)terminStatus, abstimmungsId);
        }

        public void RückmeldenZuTermin(OrchesterMitgliedsId orchesterMitgliedsId, bool istAnwesend, string? kommentar = null, OrchesterMitgliedsId otherOrchesterId = null)
        {
            var terminRückmeldungOrchesterMitglied = _terminRückmeldungOrchesterMitglieder.Find(t => t.OrchesterMitgliedsId == orchesterMitgliedsId) ?? throw new ArgumentException("Orchestermitglied wurde nicht gefunden.");
            terminRückmeldungOrchesterMitglied.ChangeZusage(istAnwesend, kommentar, otherOrchesterId);
        }

        public bool IstZugeordnet(OrchesterMitgliedsId orchesterMitgliedsId)
        {
            return _terminRückmeldungOrchesterMitglieder.Select(x => x.OrchesterMitgliedsId).Contains(orchesterMitgliedsId);
        }

        public bool NichtZurückgemeldet(OrchesterMitgliedsId orchesterMitgliedsId)
        {
            return _terminRückmeldungOrchesterMitglieder.First(i => i.OrchesterMitgliedsId == orchesterMitgliedsId).Zugesagt == (int)RückmeldungsartEnum.NichtZurückgemeldet;
        }

        public void UpdateName(string name)
        {
            Name = name is not null ? name : Name;
        }

        public void UpdateTerminArt(int? terminArt)
        {
            TerminArt = terminArt;
        }

        public void UpdateTerminRückmeldungOrchestermitglied(TerminRückmeldungOrchestermitglied[] newTerminRückmeldungOrchestermitglieds)
        {
            var elementsToRemove = new List<TerminRückmeldungOrchestermitglied>();

            foreach(var terminRückmeldungOrchesterMitglied in _terminRückmeldungOrchesterMitglieder)
            {
                if (!newTerminRückmeldungOrchestermitglieds.Select(n => n.OrchesterMitgliedsId).Contains(terminRückmeldungOrchesterMitglied.OrchesterMitgliedsId)){
                    elementsToRemove.Add(terminRückmeldungOrchesterMitglied);
                }
            }

            elementsToRemove.ForEach(e => _terminRückmeldungOrchesterMitglieder.Remove(e));

            foreach(var newTerminRückmeldungOrchestermitglied in newTerminRückmeldungOrchestermitglieds)
            {
                if(!_terminRückmeldungOrchesterMitglieder.Select(o => o.OrchesterMitgliedsId).Contains(newTerminRückmeldungOrchestermitglied.OrchesterMitgliedsId))
                {
                    _terminRückmeldungOrchesterMitglieder.Add(newTerminRückmeldungOrchestermitglied);
                }
            }
        }

    }
}
