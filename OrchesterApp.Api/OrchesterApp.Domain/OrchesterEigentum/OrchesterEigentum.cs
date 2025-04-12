using OrchesterApp.Domain.Common.Models;
using OrchesterApp.Domain.OrchesterEigentum.Entities;
using OrchesterApp.Domain.OrchesterEigentum.Enums;
using OrchesterApp.Domain.OrchesterEigentum.ValueObjects;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace OrchesterApp.Domain.OrchesterEigentum;

public sealed class OrchesterEigentum: AggregateRoot<OrchesterEigentumId, Guid>
{
    private readonly List<VerliehenesOrchesterEigentum> _verliehenesOrchesterEigentum = new();

    public string Name { get; private set; } = null!;
    public string Beschreibung { get; private set; } = null!;
    public EigentumsArt EigentumsArt { get; private set; }
    public int BestandsAnzahl { get; private set; }
    public IReadOnlyList<VerliehenesOrchesterEigentum> VerliehenesEigentum => _verliehenesOrchesterEigentum.AsReadOnly();
    public string? Lagerort { get; private set; } = null;

    private OrchesterEigentum() { }

    private OrchesterEigentum(OrchesterEigentumId id, string name, string beschreibung, EigentumsArt eigentumsArt, int anzahl, string? lagerort): base(id)
    {
        Name = name;
        Beschreibung = beschreibung;
        EigentumsArt = eigentumsArt;
        BestandsAnzahl = anzahl;
        Lagerort = lagerort;
    }

    public static OrchesterEigentum Create(string name, string beschreibung, EigentumsArt eigentumsArt, int anzahl, string? lagerort)
    {
        return new OrchesterEigentum(OrchesterEigentumId.CreateUnique(), name, beschreibung, eigentumsArt, anzahl, lagerort);
    }

    public void VerleihenAn(OrchesterMitgliedsId orchesterMitgliedsId, int zuverleihendeAnzahl, string? bemerkung = null)
    {
        var neueAnzahl = BestandsAnzahl - zuverleihendeAnzahl;
        if(neueAnzahl < 0)
        {
            throw new Exception($"{Name} kann nicht verliehen werden, da bereits alle Exemplare verliehen wurde.");
        }

        var verliehendesEigentum = _verliehenesOrchesterEigentum.Find(e => e.OrchesterMitgliedsId == orchesterMitgliedsId);
        if (verliehendesEigentum is null) 
        {
            var neuesVerleihendesEigentum = VerliehenesOrchesterEigentum.Create(orchesterMitgliedsId, zuverleihendeAnzahl, bemerkung);
        }
        else
        {
            verliehendesEigentum.ErhöheVerleihendeAnzahl(zuverleihendeAnzahl);
            verliehendesEigentum.UpdateBemerkung(bemerkung);
        }
    }

    public void ZurückgebenVon(OrchesterMitgliedsId orchesterMitgliedsId, int zurückgegebeneAnzahl, string? bemerkung = null)
    {
        var verliehendesEigentum = _verliehenesOrchesterEigentum.Find(e => e.OrchesterMitgliedsId == orchesterMitgliedsId);
        if (verliehendesEigentum is null)
        {
            throw new Exception("Dieses Orchestermitglied hat dieses Eigentum nicht ausgeliehen und kann es daher auch nicht zurückgeben.");
        }

        verliehendesEigentum.VerringereVerleihendeAnzahl(zurückgegebeneAnzahl);
        verliehendesEigentum.UpdateBemerkung(bemerkung);

        if(verliehendesEigentum.VerliehendeAnzahl == 0)
        {
            _verliehenesOrchesterEigentum.Remove(verliehendesEigentum);
        }
    }

    public void ErhöheBestand(int anzahl)
    {
        BestandsAnzahl += anzahl;
    }

    public void VerringereBestand(int anzahl)
    {
        if(BestandsAnzahl - anzahl < 0)
        {
            throw new ArgumentException("Bestand kann nicht weiter verringert werden als 0.");
        }
        BestandsAnzahl -= anzahl;
    }
}
