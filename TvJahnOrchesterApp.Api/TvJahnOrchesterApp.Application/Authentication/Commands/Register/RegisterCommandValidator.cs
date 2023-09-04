using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create;

namespace TvJahnOrchesterApp.Application.Authentication.Commands.Register
{
    internal class RegisterCommandValidator: AbstractValidator<RegisterCommand>
    {
        RegisterCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
