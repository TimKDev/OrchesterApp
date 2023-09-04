using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvJahnOrchesterApp.Contracts.Authentication
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Ein Registrierungsschlüssel wird benötigt.")]
        public string RegisterationKey { get; set; } = null!;

        [Required(ErrorMessage = "Eine E-Mail wird benötigt.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Ein Passwort wird benötigt.")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "Die eingegebenen Passwörter stimmen nicht überein.")]
        public string ConfirmPassword { get; set; } = null!;

        public string ClientUri { get; set; } = null!;
    }
}
