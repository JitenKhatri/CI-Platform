namespace CI_Platform.Controllers
{
    using CI_Platform.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using System.Security.Claims;

    public class ProfileCompletionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                var userId = long.Parse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);

                using (var dbContext = new CiPlatformContext())
                {
                    var User = dbContext.Users.FirstOrDefault(u => u.UserId == userId);

                    if (User != null && (string.IsNullOrEmpty(User.FirstName) || string.IsNullOrEmpty(User.LastName) || string.IsNullOrEmpty(User.CityId.ToString()) || string.IsNullOrEmpty(User.CountryId.ToString())))
                    {
                        context.Result = new RedirectToActionResult("EditProfile", "UserAuthentication", null);
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
