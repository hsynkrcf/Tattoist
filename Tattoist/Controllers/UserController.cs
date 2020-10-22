using System;
using System.Web.Mvc;
using Tattoist.BaseController;
using TattoistDAL.EntityFramework;
using TattoistDAL.EntityFramework.Concrete;
using TattoistDAL.Models;
using TattoistDAL.Models.Entities;

namespace RGNMarketPlace.Controllers
{
    public class UserController : AppUserBaseController
    {
        UnitOfWork unitOfWork;
        public UserController()
        {
            unitOfWork = new UnitOfWork(SingletonContext<AppDbContext>.Instance);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            var users = unitOfWork.User.GetAll();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Guid userId)
        {
            var model = unitOfWork.User.Get(x => x.Id == userId);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid userId)
        {
            var user = unitOfWork.User.Get(x => x.Id == userId);
            if (user != null)
            {
                unitOfWork.User.Remove(user);
                unitOfWork.Complete();
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Designer,Tattoo,Report")]
        public ActionResult MyProfile()
        {
            var model = (User)Session["User"];
            var user = unitOfWork.User.Get(x => x.Id == model.Id);
            return View(user);
        }
    }
}