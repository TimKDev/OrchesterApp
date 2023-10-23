using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Errors;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Models.Errors
{
    internal class AccountLockedException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public string Title => "Account geperrt";
        public string ErrorMessage => "Account wurde wegen zu vieler falscher Anmeldeversuche gesperrt. Bitte ändern Sie ihr Passwort um ihren Account zu entsperren oder sprechen Sie mit ihrem Administrator.";
    }
}
