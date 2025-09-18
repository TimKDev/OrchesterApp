using OrchesterApp.Domain.AbstimmungsAggregate.ValueObjects;
using OrchesterApp.Domain.Common.Enums;
using OrchesterApp.Domain.Common.Models;
using OrchesterApp.Domain.Common.ValueObjects;
using OrchesterApp.Domain.OrchesterMitgliedAggregate;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using OrchesterApp.Domain.TerminAggregate.Entities;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace OrchesterApp.Domain.TerminAggregate
{
    public sealed class Termin : AggregateRoot<TerminId, Guid>
    {
        private readonly List<TerminRückmeldungOrchestermitglied> _terminRückmeldungOrchesterMitglieder = new();
        private List<TerminDokument> _dokumente = new();
        public string Name { get; private set; } = null!;
        public byte[]? Image { get; private set; } = null!;
        public int? TerminArt { get; private set; }
        public int? TerminStatus { get; private set; }
        public EinsatzPlan EinsatzPlan { get; private set; } = null!;

        public IReadOnlyList<TerminRückmeldungOrchestermitglied> TerminRückmeldungOrchesterMitglieder =>
            _terminRückmeldungOrchesterMitglieder.AsReadOnly();

        public AbstimmungsId? AbstimmungsId { get; private set; }
        public IReadOnlyList<TerminDokument> Dokumente => _dokumente.AsReadOnly();

        private Termin()
        {
        }

        private Termin(TerminId id, TerminRückmeldungOrchestermitglied[] terminRückmeldungOrchesterMitglieder,
            string name, int? terminArt, EinsatzPlan einsatzPlan, int terminStatus, byte[]? image = null,
            AbstimmungsId? abstimmungsId = null) : base(id)
        {
            Name = name;
            TerminArt = terminArt;
            EinsatzPlan = einsatzPlan;
            AbstimmungsId = abstimmungsId;
            _terminRückmeldungOrchesterMitglieder = terminRückmeldungOrchesterMitglieder.ToList();
            TerminStatus = terminStatus;
            Image = image;
        }

        public static Termin Create(TerminRückmeldungOrchestermitglied[] terminRückmeldungOrchesterMitglieder,
            string name, int? terminArt, DateTime startZeit, DateTime endZeit, Adresse treffPunkt, List<int>? noten,
            List<int>? uniform, AbstimmungsId? abstimmungsId = null,
            TerminStatusEnum terminStatus = TerminStatusEnum.Zugesagt, string? zusätzlicheInfo = null,
            byte[]? image = null)
        {
            var notenMappings = noten?.Select(EinsatzplanNotenMapping.Create);
            var uniformMappings = uniform?.Select(EinsatzplanUniformMapping.Create);
            var einsatzplan = EinsatzPlan.Create(startZeit, endZeit, treffPunkt, notenMappings, uniformMappings,
                zusätzlicheInfo);

            return new Termin(TerminId.CreateUnique(), terminRückmeldungOrchesterMitglieder, name, terminArt,
                einsatzplan, (int)terminStatus, image, abstimmungsId);
        }

        public void AddMitgliedToTermin(OrchesterMitglied mitglied)
        {
            var terminRueckmeldung = TerminRückmeldungOrchestermitglied.Create(mitglied.Id,
                [mitglied.DefaultInstrument],
                [mitglied.DefaultNotenStimme]);

            _terminRückmeldungOrchesterMitglieder.Add(terminRueckmeldung);
        }

        public void RückmeldenZuTermin(OrchesterMitgliedsId orchesterMitgliedsId, int zugesagt,
            string? kommentar = null, OrchesterMitgliedsId otherOrchesterId = null)
        {
            var terminRückmeldungOrchesterMitglied =
                _terminRückmeldungOrchesterMitglieder.Find(t => t.OrchesterMitgliedsId == orchesterMitgliedsId) ??
                throw new ArgumentException("Orchestermitglied wurde nicht gefunden.");
            terminRückmeldungOrchesterMitglied.ChangeZusage(zugesagt, kommentar, otherOrchesterId);
        }

        public void AnwesenheitZuTermin(OrchesterMitgliedsId orchesterMitgliedsId, bool anwesend,
            string? kommentar = null)
        {
            var terminRückmeldungOrchesterMitglied =
                _terminRückmeldungOrchesterMitglieder.Find(t => t.OrchesterMitgliedsId == orchesterMitgliedsId) ??
                throw new ArgumentException("Orchestermitglied wurde nicht gefunden.");
            terminRückmeldungOrchesterMitglied.ChangeAnwesenheit(anwesend, kommentar);
        }

        public bool IstZugeordnet(OrchesterMitgliedsId orchesterMitgliedsId)
        {
            return _terminRückmeldungOrchesterMitglieder.Select(x => x.OrchesterMitgliedsId)
                .Contains(orchesterMitgliedsId);
        }

        public List<TerminDokument> UpdateDokumenteAndReturnDokumenteForCleanup(List<TerminDokument> dokumente)
        {
            var cleanupDokumente = _dokumente.Where(x => !dokumente.Contains(x)).ToList();
            _dokumente = dokumente;

            return cleanupDokumente;
        }

        public bool NichtZurückgemeldet(OrchesterMitgliedsId orchesterMitgliedsId)
        {
            return _terminRückmeldungOrchesterMitglieder.First(i => i.OrchesterMitgliedsId == orchesterMitgliedsId)
                .Zugesagt == (int)RückmeldungsartEnum.NichtZurückgemeldet;
        }

        public void UpdateName(string name)
        {
            Name = name is not null ? name : Name;
        }

        public void UpdateImage(byte[]? image)
        {
            Image = image;
        }

        public void UpdateTerminArt(int? terminArt)
        {
            TerminArt = terminArt;
        }

        public void UpdateTerminStatus(int? terminStatus)
        {
            TerminStatus = terminStatus;
        }

        public void UpdateTerminRückmeldungOrchestermitglied(
            TerminRückmeldungOrchestermitglied[] newTerminRückmeldungOrchestermitglieds)
        {
            var elementsToRemove = new List<TerminRückmeldungOrchestermitglied>();

            foreach (var terminRückmeldungOrchesterMitglied in _terminRückmeldungOrchesterMitglieder)
            {
                if (!newTerminRückmeldungOrchestermitglieds.Select(n => n.OrchesterMitgliedsId)
                        .Contains(terminRückmeldungOrchesterMitglied.OrchesterMitgliedsId))
                {
                    elementsToRemove.Add(terminRückmeldungOrchesterMitglied);
                }
            }

            elementsToRemove.ForEach(e => _terminRückmeldungOrchesterMitglieder.Remove(e));

            foreach (var newTerminRückmeldungOrchestermitglied in newTerminRückmeldungOrchestermitglieds)
            {
                if (!_terminRückmeldungOrchesterMitglieder.Select(o => o.OrchesterMitgliedsId)
                        .Contains(newTerminRückmeldungOrchestermitglied.OrchesterMitgliedsId))
                {
                    _terminRückmeldungOrchesterMitglieder.Add(newTerminRückmeldungOrchestermitglied);
                }
            }
        }
    }
}