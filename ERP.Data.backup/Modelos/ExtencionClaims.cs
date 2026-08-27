using System.Security.Claims;

namespace ERP.Data.Modelos
{
    public static class ExtencionClaims
    {
        public static bool TieneRol(this ClaimsPrincipal user, params string[] roles)
        {
            var roleClaim = user.Claims.FirstOrDefault(c => c.Type == "idRol")?.Value;
            return roleClaim != null && roles.Contains(roleClaim);
        }
    }
}
