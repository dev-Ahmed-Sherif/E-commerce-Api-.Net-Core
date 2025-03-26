using System.Security.Claims;

namespace WebApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal) =>
        principal.Claims
            .SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?
            .Value!;
}