using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.ValueObjects
{
    public sealed class Notenstimme : ValueObject
    {
        public NotenstimmeEnum Stimme { get; private set; }

        private Notenstimme() { }

        private Notenstimme(NotenstimmeEnum stimme)
        {
            Stimme = stimme;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Stimme;
        }

        public static Notenstimme Create(NotenstimmeEnum notenstimme)
        {
            return new Notenstimme(notenstimme);
        }
    }

    public enum NotenstimmeEnum
    {
        AltSaxophon1,
        AltSaxophon2,
    }
}
