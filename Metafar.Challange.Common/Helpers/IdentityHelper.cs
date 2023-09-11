using System.Security.Claims;

namespace Metafar.Challange.Common.Helpers
{
    public static class IdentityHelper
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
          => Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type.Equals(IdentityClaims.Id))?.Value);
    }
}
