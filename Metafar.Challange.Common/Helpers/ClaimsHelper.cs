using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Metafar.Challange.Common.Helpers
{
    public static class ClaimHelper
    {
        public static IEnumerable<Claim> BuildDefaultClaims(string id, string name, string accountNumber)
        {
            var claims = new[] {
                    new Claim(IdentityClaims.Id, id),
                    new Claim(IdentityClaims.Name, name),
                    new Claim(IdentityClaims.AccountNumber, accountNumber)
            }.ToList();

            return claims;
        }
    }
}
