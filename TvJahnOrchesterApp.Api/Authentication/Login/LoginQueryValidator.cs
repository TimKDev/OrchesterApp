using FluentValidation;

namespace TvJahnOrchesterApp.Application.Authentication.Queries.Login
{
    internal class LoginQueryValidator: AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator() 
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
