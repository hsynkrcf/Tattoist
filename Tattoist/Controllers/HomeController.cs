using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tattoist.BaseController;
using TattoistDAL.EntityFramework;
using TattoistDAL.EntityFramework.Concrete;
using TattoistDAL.Models;
using TattoistDAL.Models.DTO;
using TattoistDAL.Models.Entities;

namespace RGNMarketPlace.Controllers
{
    public class HomeController : AppUserBaseController
    {
        private readonly UnitOfWork unitOfWork;
        public HomeController()
        {
            unitOfWork = new UnitOfWork(SingletonContext<AppDbContext>.Instance);
        }
        public ActionResult HomePage()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UploadCSV(HttpPostedFileBase file)
        {
            try
            {
                List<Customer> customers = new List<Customer>();
                if (file != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path);
                    }
                    Directory.CreateDirectory(path);

                    string filePath = path + Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    file.SaveAs(filePath);

                    string csvData = System.IO.File.ReadAllText(filePath);
                    int count = 0;
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (count == 0)
                        {
                            count++;
                            continue;
                        }
                        if (!string.IsNullOrEmpty(row))
                        {
                            string s = row.Split(',')[2].Replace("\"", string.Empty);
                            customers.Add(new Customer
                            {
                                CustomerId = Guid.NewGuid(),
                                CustomerName = row.Split(',')[0].Replace("\"", string.Empty),
                                CustomerPhone = row.Split(',')[1].Replace("\"", string.Empty),
                                CustomerBirthday = Convert.ToDateTime(row.Split(',')[2].Replace("\"", string.Empty))
                            });
                        }
                    }
                    unitOfWork.Customer.AddRange(customers);
                    unitOfWork.Complete();
                    TempData["Success"] = "Csv Başarıyla Yüklendi";
                    return RedirectToAction("Contact", "Appointment");
                }
                else
                {
                    TempData["Error"] = "Lütfen Dosya Seçiniz";
                    return RedirectToAction("Contact", "Appointment");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "CSV Eklenemedi. Hata:" + ex;
                return RedirectToAction("Contact", "Appointment");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Expense()
        {
            var model = unitOfWork.Expense.GetAll().ToList();
            return View(model);
        }
        [HttpPost]
        public JsonResult Expense(Expense expense)
        {
            try
            {
                if (expense.Id == Guid.Empty)
                {
                    unitOfWork.Expense.Add(new Expense()
                    {
                        Id = Guid.NewGuid(),
                        Date = expense.Date,
                        Title = expense.Title,
                        Amount = expense.Amount,
                        Description = expense.Description
                    });
                    unitOfWork.Complete();

                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var temp = unitOfWork.Expense.Get(x => x.Id == expense.Id);
                    temp.Date = expense.Date;
                    temp.Title = expense.Title;
                    temp.Amount = expense.Amount;
                    temp.Description = expense.Description;

                    unitOfWork.Expense.Update(temp);
                    unitOfWork.Complete();

                    return Json("updated", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ExpenseDelete(string id)
        {
            var guid = Guid.Parse(id);
            var expense = unitOfWork.Expense.Get(x => x.Id == guid);
            unitOfWork.Expense.Remove(expense);
            unitOfWork.Complete();
            ViewBag.success = "Gider Başarıyla Silindi";
            return RedirectToAction("Expense");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Settings()
        {
            SettingsVM settings = null;
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + "settings.json";

            if (!System.IO.File.Exists(path))
            {
                Helper.SetSettings(2, 18, "12:00","09:00", "İyiki Doğdun -Escalade INK.", "Yarın Randevunuz Vardır. Gelmeyi Unutmayınız -Escalade INK.", "5323614919", "orkiasli7681", "EscaladeInk");
            }
            settings = Helper.GetSettings();

            return View(settings);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Settings(SettingsVM settings)
        {
            try
            {
                Helper.SetSettings(settings.CreditCardTax, settings.KDV, settings.BirthdayScheduling,settings.AppointmentScheduling, settings.BirthdayText, settings.AppointmentText,settings.Username,settings.Password,settings.Sender);

                TempData["Success"] = "Ayarlar Başarıyla Kaydedildi";
                return View(Helper.GetSettings());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ayarlar Kaydedilemedi HATA: " + ex.Message;
                return View(Helper.GetSettings());
            }
        }
    }
}