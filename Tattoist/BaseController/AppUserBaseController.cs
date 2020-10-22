using System.Web.Mvc;

namespace Tattoist.BaseController
{
    public class AppUserBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (!Request.CurrentExecutionFilePath.Contains("/Ajax/Login") && Session["User"] == null)
            {
                filterContext.Result = new RedirectResult(Url.Action("Login", "Account"));
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}