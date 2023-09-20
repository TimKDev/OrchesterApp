using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.Entities
{
    public sealed class ZeitBlock: Entity<ZeitblockId>
    {
        public DateTime Startzeit { get; private set; }
        public DateTime Endzeit { get; private set; }
        public string Beschreibung { get; private set; } = null!;
        public Adresse? Adresse { get; private set; } 
        
        private ZeitBlock() { }

        private ZeitBlock(ZeitblockId id, DateTime startzeit, DateTime endzeit, string beschreibung, Adresse? adresse = null): base(id)
        {
            Startzeit = startzeit;
            Endzeit = endzeit;
            Beschreibung = beschreibung;
            Adresse = adresse;
        }

        public static ZeitBlock Create(DateTime startzeit, DateTime endzeit, string beschreibung, Adresse? adresse = null)
        {
            //Validiere dass die Startzeit vor der Endzeit ist
            return new ZeitBlock(ZeitblockId.CreateUnique(), startzeit, endzeit, beschreibung, adresse);
        }

        public void Update(DateTime startzeit, DateTime endzeit, string beschreibung, Adresse? adresse = null)
        {
            //Validiere dass die Startzeit vor der Endzeit ist
            Startzeit = startzeit;
            Endzeit = endzeit;
            Beschreibung = beschreibung;
            Adresse = adresse;
        }
    }
}
