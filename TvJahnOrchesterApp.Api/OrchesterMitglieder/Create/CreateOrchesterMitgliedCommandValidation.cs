using FluentValidation;
using TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create;

namespace TvJahnOrchesterApp.Application.OrchesterMitglied.Commands.Create
{
    public class CreateOrchesterMitgliedCommandValidation: AbstractValidator<CreateOrchesterMitgliedCommand>
    {
        public CreateOrchesterMitgliedCommandValidation()
        {
            RuleFor(x => x.Vorname).NotEmpty();
            RuleFor(x => x.Nachname).NotEmpty();
        }
    }
}
