using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.TerminAggregate.ValueObjects;

public sealed class TerminDokument : ValueObject
{
    public string Name { get; private set; }

    public TerminDokument(string name)
    {
        Name = name;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}