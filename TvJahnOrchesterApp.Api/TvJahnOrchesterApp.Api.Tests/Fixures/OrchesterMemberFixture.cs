
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Api.Controllers;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;

namespace TvJahnOrchesterApp.Api.Tests.Fixures
{
    internal static class OrchesterMemberFixture
    {
        public static OrchestraMemberContract GetOrchestraMemberContract()
        {
            var address = new Address("Teststraße", "234", "73638", "Berlin");
            return new OrchestraMemberContract("Tim", "Kempkens",new[] { 1, 2 }, address, "01223434556");
        }

        public static OrchestraMemberContract GetInvalidOrchestraMemberContractWithoutFirstName()
        {
            return new OrchestraMemberContract("", "Kempkens", null, null, null);
        }

        internal static OrchestraMemberContract GetInvalidOrchestraMemberContractWithoutLastName()
        {
            return new OrchestraMemberContract("Tim", "", null, null, null);

        }
    }
}
