using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceriesWasmApp.Shared {
    public record Person (Guid PersonId, string PersonName);
    public record Member (Person Person, bool IsAdministrator);
    public record Invitation (string EmailAddress, bool IsAccepted);
    public record Family (Guid FamilyId, string FamilyName, Member[] Members, Invitation[] Invitations);
}
// Can join a family if know the famil name + 