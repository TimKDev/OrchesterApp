using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.Entities
{
    public sealed class EinsatzPlan : Entity<EinsatzplanId>
    {
        private readonly List<ZeitBlock> _zeitBlocks = new();
        private readonly List<Noten> _noten = new();
        private readonly List<Uniform> _uniform = new();

        public DateTime StartZeit { get; private set; }
        public DateTime EndZeit { get; private set; }
        public Adresse Treffpunkt { get; private set; } = null!;
        public IReadOnlyList<ZeitBlock> ZeitBlocks => _zeitBlocks.AsReadOnly();
        public IReadOnlyList<Noten> Noten => _noten.AsReadOnly();
        public IReadOnlyList<Uniform> Uniform => _uniform.AsReadOnly();
        public string? WeitereInformationen { get; private set; }

        private EinsatzPlan() { }
        private EinsatzPlan(EinsatzplanId id, DateTime startZeit, DateTime endZeit, Adresse treffpunkt, string? weitereInformationen = null): base(id)
        {
            StartZeit = startZeit;
            EndZeit = endZeit;
            Treffpunkt = treffpunkt;
            WeitereInformationen = weitereInformationen;
        }

        public static EinsatzPlan Create(DateTime startZeit, DateTime endZeit, Adresse treffpunkt, string? weitereInformationen = null)
        {
            //Validiere dass startZeit vor Endzeit ist
            return new EinsatzPlan(EinsatzplanId.CreateUnique(), startZeit, endZeit, treffpunkt);
        }

        public void UpdateEinsatzPlan(DateTime startZeit, DateTime endZeit, Adresse treffpunkt, string? weitereInformationen = null)
        {
            //Validiere dass startZeit vor Endzeit ist
            StartZeit = startZeit;
            EndZeit = endZeit;
            Treffpunkt = treffpunkt;
            WeitereInformationen = weitereInformationen;
        }

        public void AddZeitBlock(DateTime startzeit, DateTime endzeit, string beschreibung, Adresse? adresse = null)
        {
            //Validieren, dass sich die Zeitblöcke nicht überschneiden => Dies ist keine Validierung die innerhalb der Zeitblöcke gemacht werden kann.
            _zeitBlocks.Add(ZeitBlock.Create(startzeit, endzeit, beschreibung, adresse));
        }

        public void UpdateZeitBlock(ZeitblockId id, DateTime startZeit, DateTime endZeit, string beschreibung, Adresse? adresse = null)
        {
            var zeitBlock = _zeitBlocks.FirstOrDefault(x => x.Id == id);
            if(zeitBlock is null)
            {
                throw new Exception("ZeitBlock könnte nicht gefunden werden");
            }
            //Validieren, dass sich die Zeitblöcke nach dem Update nicht überschneiden.
            zeitBlock.Update(startZeit, endZeit, beschreibung, adresse);
        }

        public void DeleteZeitBlock(ZeitblockId id)
        {
            var zeitBlock = _zeitBlocks.FirstOrDefault(x => x.Id == id);
            if (zeitBlock is null)
            {
                throw new Exception("ZeitBlock könnte nicht gefunden werden");
            }
            _zeitBlocks.Remove(zeitBlock);
        }

        public void AddNoten(List<Noten> notesToAdd)
        {
            foreach (var note in notesToAdd)
            {
                if (!_noten.Contains(note))
                {
                    _noten.Add(note);
                }
            }
        }

        public void removeNoten(List<Noten> notesToDelete)
        {
            foreach(var note in notesToDelete)
            {
                _noten.Remove(note);
            }
        }

        public void AddUniform(List<Uniform> uniformToAdd)
        {
            foreach (var uniform in uniformToAdd)
            {
                if (!_uniform.Contains(uniform))
                {
                    _uniform.Add(uniform);
                }
            }
        }

        public void removeUniform(List<Uniform> uniformToDelete)
        {
            foreach (var uniform in uniformToDelete)
            {
                _uniform.Remove(uniform);
            }
        }
    }
}
