using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Errors;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Models.Errors
{
    internal class IdentityRegistrationException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public string Title => "Fehler bei Registrierung";
        public string ErrorMessage { get; }

        public IdentityRegistrationException(string errorMesssage)
        {
            ErrorMessage = errorMesssage;
        }
    }
}
