namespace Aspire.WebApp;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

public class ClaimsTransformer : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claimsIdentity = (ClaimsIdentity)principal.Identity;


        return Task.FromResult(principal);
    }
}
