using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.OrchestraMembers.Common;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;

namespace TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create
{
    public class CreateOrchestraMemberCommand: IRequest<CreateOrchestraMemberResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int[] InstrumentIds { get; set; }
        public string Street { get; set;  }
        public string HouseNumber { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
    }
}
