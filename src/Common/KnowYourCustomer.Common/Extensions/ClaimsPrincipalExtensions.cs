using System.Security.Claims;

namespace KnowYourCustomer.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.GetClaimValue(ClaimTypes.SubjectId);
        }

        private static string GetClaimValue(this ClaimsPrincipal user, string claimType)
        {
            var value = user?.FindFirst(claimType)?.Value;

            return string.IsNullOrEmpty(value) ? null : value;
        }
    }

    public static class ClaimTypes
    {
        public const string SubjectId = "sub";
    }
}