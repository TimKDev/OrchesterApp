using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.Entities
{
    public sealed class AnwesenheitsListenEintrag: Entity<AnwesenheitsListenEintragId>
    {
        public OrchesterMitgliedsId OrchesterMitgliedsId { get; private set; } = null!;
        public bool IstAnwesend { get; private set; }
        public string? Kommentar { get; private set; }

        private AnwesenheitsListenEintrag() { }

        private AnwesenheitsListenEintrag(AnwesenheitsListenEintragId id, OrchesterMitgliedsId orchesterMitgliedsId, bool istAnwesend): base(id)
        { 
            OrchesterMitgliedsId = orchesterMitgliedsId;
            IstAnwesend = istAnwesend;
        }

        public static AnwesenheitsListenEintrag Create(OrchesterMitgliedsId orchesterMitgliedsId)
        {
            return new AnwesenheitsListenEintrag(AnwesenheitsListenEintragId.CreateUnique(), orchesterMitgliedsId, false);
        }

        public void Update(bool istAnwesend, string? kommentar = null)
        {
            IstAnwesend = istAnwesend;
            Kommentar = kommentar;
        }
    }
}
