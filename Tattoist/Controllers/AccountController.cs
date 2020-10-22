using System.Web.Mvc;
using System.Web.Security;

namespace RGNMarketPlace.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {            
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult SignOut()
        {
            Session.Clear();

            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}