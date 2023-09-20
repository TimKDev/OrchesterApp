using TvJahnOrchesterApp.Domain.AbstimmungsAggregate.Entities;
using TvJahnOrchesterApp.Domain.AbstimmungsAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.AbstimmungsAggregate
{
    public sealed class Abstimmung: AggregateRoot<AbstimmungsId, Guid>
    {
        private readonly List<UserAbstimmung> _userAbstimmungen = new();

        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;    
        public string[] AuswahlMöglichkeiten { get; private set; } = null!;
        public IReadOnlyList<UserAbstimmung> UserAbstimmungen => _userAbstimmungen.AsReadOnly();
        //UserGruppe (die zur Abstimmmung berechtigt ist)?

        private Abstimmung() { }

        private Abstimmung(AbstimmungsId id, string name, string description, string[] auswahlMöglichkeiten): base(id)
        {
            Name = name;
            Description = description;
            AuswahlMöglichkeiten = auswahlMöglichkeiten;
        }

        public static Abstimmung Create(string name, string description, string[] auswahlMöglichkeiten)
        {
            return new Abstimmung(AbstimmungsId.CreateUnique(), name, description, auswahlMöglichkeiten);
        }

        //TTODO: Erstelle Methode um UserAbstimmung hinzu zu fügen oder zu ändern, wobei validiert werden sollte, dass jeder abstimmungsberechtigte User nur einmal abstimmt
    }
}
