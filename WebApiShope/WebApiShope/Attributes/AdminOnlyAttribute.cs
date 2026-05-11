using Microsoft.AspNetCore.Authorization;

namespace WebApiShope.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        public AdminOnlyAttribute() => Roles = "admin";
    }
}
