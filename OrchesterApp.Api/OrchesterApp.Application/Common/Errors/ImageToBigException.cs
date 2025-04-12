using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TvJahnOrchesterApp.Application.Common.Errors
{
    internal class ImageToBigException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string Title => "Bild zu groß";

        public string ErrorMessage => "Die hochgeladene Bilddatei ist zu groß. Bitte wähle ein Bild mit einer geringeren Auflösung.";
    }
}
