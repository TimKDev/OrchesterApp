using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects
{
    public sealed class Noten : ValueObject
    {
        public NotenEnum NotenEnum { get; private set; }

        private Noten() { }

        private Noten(NotenEnum notenEnum)
        {
            NotenEnum = notenEnum;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return NotenEnum;
        }

        public static Noten Create(NotenEnum notenEnum)
        {
            return new Noten(notenEnum);
        }
    }

    public enum NotenEnum
    {
        SchwarzesMarschbuch,
        BlauesMarschbuch,
        RotesMarschbuch,
        Konzertmappe,
        Weihnachtsnoten,
        StMartinNoten,
        KarnevalsNoten
    }
}
