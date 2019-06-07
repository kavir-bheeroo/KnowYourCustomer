using System;
using System.Security.Claims;

namespace KnowYourCustomer.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            return user.GetClaimValue(ClaimTypes.SubjectId);
        }

        private static Guid? GetClaimValue(this ClaimsPrincipal user, string claimType)
        {
            var value = user?.FindFirst(claimType)?.Value;
            var isGuid = Guid.TryParse(value, out var result);

            return string.IsNullOrEmpty(value) || !isGuid ? (Guid?)null : result;
        }
    }

    public static class ClaimTypes
    {
        public const string SubjectId = "sub";
    }
}