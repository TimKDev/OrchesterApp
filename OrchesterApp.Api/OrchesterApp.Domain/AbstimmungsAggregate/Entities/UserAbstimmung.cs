using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchesterApp.Domain.AbstimmungsAggregate.ValueObjects;
using OrchesterApp.Domain.Common.Models;
using OrchesterApp.Domain.UserAggregate.ValueObjects;

namespace OrchesterApp.Domain.AbstimmungsAggregate.Entities
{
    public sealed class UserAbstimmung : Entity<UserAbstimmungsId>
    {
        public UserId UserId { get; private set; } = null!;
        public string AuswahlErgebnis { get; private set; } = null!;

        private UserAbstimmung() { }

        private UserAbstimmung(UserAbstimmungsId id, UserId userId, string auswahlErgebnis): base(id)
        {
            UserId = userId;
            AuswahlErgebnis = auswahlErgebnis;
        }

        public static UserAbstimmung CreateUnique(UserId userId, string auswahlErgebnis)
        {
            //TTODO: Sollte hier validiert werden, dass das Auswahlergebnis auch zu den möglichen Auswahlerbenissen der Abstimmung passt?
            return new UserAbstimmung(UserAbstimmungsId.CreateUnique(), userId, auswahlErgebnis);
        }
    }
}
