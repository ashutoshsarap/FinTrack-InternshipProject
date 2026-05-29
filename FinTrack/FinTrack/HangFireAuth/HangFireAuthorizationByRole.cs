using FinTrack.Utilities;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace FinTrack.HangFireAuth
{
    public class HangFireAuthorizationByRole : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Check if the user is authenticated and has the "Admin" role
            return httpContext.User.Identity != null &&
                   httpContext.User.Identity.IsAuthenticated &&
                   httpContext.User.IsInRole(Roles.Admin);
        }
    }
}
