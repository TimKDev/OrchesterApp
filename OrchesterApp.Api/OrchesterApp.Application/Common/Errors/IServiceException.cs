﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TvJahnOrchesterApp.Application.Common.Errors
{
    public interface IServiceException
    {
        public HttpStatusCode StatusCode { get; }
        public string Title { get; }
        public string ErrorMessage { get; }
    }
}
