using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tattoist.BaseController;
using TattoistDAL.EntityFramework;
using TattoistDAL.EntityFramework.Concrete;
using TattoistDAL.Models;
using TattoistDAL.Models.DTO;
using TattoistDAL.Models.Entities;

namespace Tattoist.Controllers
{
    public class ReportController : AppUserBaseController
    {
        private readonly UnitOfWork unitOfWork;
        public ReportController()
        {
            unitOfWork = new UnitOfWork(SingletonContext<AppDbContext>.Instance);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CashReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CashReport(string reportrange)
        {
            try
            {
                var dates = reportrange.Split(' ');
                var startDate = Convert.ToDateTime(dates[0]);
                var endDate = Convert.ToDateTime(dates[2]);

                var payment = unitOfWork.Payment.GetAll(x => x.PaymentDate >= startDate && x.PaymentDate <= endDate).ToList();
                var expenses = unitOfWork.Expense.GetAll(x => x.Date >= startDate && x.Date <= endDate).ToList().Select(x => x.Amount).Sum();

                var listDeposit = GetDeposites();

                var cash = payment.Where(x => x.PaymentType == 1).Select(x => x.PaymentAmount).Sum();
                var credit = payment.Where(x => x.PaymentType == 2).Select(x => x.PaymentAmount).Sum();
                var remit = payment.Where(x => x.PaymentType == 3).Select(x => x.PaymentAmount).Sum();

                foreach (var item in listDeposit)
                {
                    if (item.Date == startDate.Date)
                    {
                        if (item.Type == "Havale")
                        {
                            remit += item.Amount;
                        }
                        else if (item.Type == "Kredi Kartı")
                        {
                            credit += item.Amount;
                        }
                        else
                        {
                            cash += item.Amount;
                        }
                    }
                }

                int incomeSum = cash + credit + remit;
                decimal sum = Convert.ToDecimal(incomeSum) - Convert.ToDecimal(expenses);
                var model = new CashReportVM()
                {
                    ReportRange = reportrange,
                    ExpenseSum = Convert.ToDecimal(expenses),
                    Cash = cash,
                    CreditCard = credit,
                    Remittance = remit,
                    IncomeSum = incomeSum,
                    Sum = sum
                };
                return View(model);

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }



        [Authorize(Roles = "Admin")]
        public ActionResult IncomeExpenseReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IncomeExpenseReport(string reportrange)
        {
            var culture = new CultureInfo("tr-TR");
            var dfi = DateTimeFormatInfo.CurrentInfo;
            List<IncomeExpenseVM> weekList = new List<IncomeExpenseVM>();
            try
            {
                var dates = reportrange.Split(' ');
                var startDate = Convert.ToDateTime(dates[0]);
                var endDate = Convert.ToDateTime(dates[2]);

                for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                {
                    weekList.Add(new IncomeExpenseVM()
                    {
                        ReportRange = reportrange,
                        Date = dt,
                        Day = culture.DateTimeFormat.GetDayName(dt.DayOfWeek),
                    });
                }

                var expenses = unitOfWork.Expense.GetAll(x => x.Date >= startDate && x.Date <= endDate).ToList();
                var payment = unitOfWork.Payment.GetAll(x => x.PaymentDate >= startDate && x.PaymentDate <= endDate).ToList();

                var listDeposit = GetDeposites();


                foreach (var item in payment)
                {
                    foreach (var item2 in weekList)
                    {
                        if (item.PaymentDate == item2.Date)
                        {
                            item2.Income += item.PaymentAmount;
                        }
                    }
                }

                foreach (var item in expenses)
                {
                    foreach (var item2 in weekList)
                    {
                        if (item.Date == item2.Date)
                        {
                            item2.Expense += Convert.ToDecimal(item.Amount);
                        }
                    }
                }

                foreach (var item in listDeposit)
                {
                    foreach (var item2 in weekList)
                    {
                        if (item.Date == item2.Date)
                        {
                            item2.Income += item.Amount;
                        }
                    }
                }

                return View(weekList);

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Contract(string id)
        {
            try
            {
                if (Session["User"] != null)
                {
                    var user = (User)Session["User"];
                    var appId = Guid.Parse(id);
                    var appointment = unitOfWork.Appointment.Get(x => x.AppointmentId == appId);
                    var customer = unitOfWork.Customer.Get(x => x.CustomerId == appointment.CustomerId);

                    var html = System.IO.File.ReadAllText(Server.MapPath("~/assets/Anlasma/Contract.html"));
                    html = html.Replace("ttarih", DateTime.Now.ToString());
                    html = html.Replace("rrezervasyon", id.Split('-')[0]);
                    html = html.Replace("oolusturan", user.FullName);
                    html = html.Replace("add", Helper.NameSplitter(customer.CustomerName, true));
                    html = html.Replace("ssoyad", Helper.NameSplitter(customer.CustomerName, false));
                    html = html.Replace("ffiyat", string.Format("{0:N}", appointment.TotalPrice) + " ₺");
                    html = html.Replace("ddepozito", string.Format("{0:N}", appointment.DepositeAmount) + " ₺");
                    html = html.Replace("kkalan", string.Format("{0:N}", appointment.TotalPrice - appointment.DepositeAmount) + " ₺");
                    html = html.Replace("rreportarih", appointment.AppointmentDate.ToString());
                    //html = html.Replace("ssaat", "");
                    html = html.Replace("ttelefon", appointment.Phone);
                    return Content(html);
                }
                else
                {
                    return Content("Yetkisiz Giriş");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult PersonalReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PersonalReport(string reportrange)
        {
            List<PersonalReportVM> model = new List<PersonalReportVM>();

            var dates = reportrange.Split(' ');
            var startDate = Convert.ToDateTime(dates[0]);
            var endDate = Convert.ToDateTime(dates[2]);

            var employees = unitOfWork.Employee.GetAll().ToList();
            var brokers = unitOfWork.Broker.GetAll();
            var appointment = unitOfWork.Appointment.GetAll(x => x.AppointmentDate >= startDate && startDate <= endDate);
            var settings = Helper.GetSettings();

            foreach (var item in brokers)
            {
                model.Add(new PersonalReportVM()
                {
                    Id = item.BrokerId.ToString().ToLower(),
                    EmployeeName = item.RelatedPerson,
                    Percent = item.BrokerPercent
                });
            }

            foreach (var item in employees)
            {
                model.Add(new PersonalReportVM()
                {
                    Id = item.EmployeeId.ToString().ToLower(),
                    EmployeeName = item.EmployeeName,
                    Percent = item.EmployeePercent
                });
            }

            decimal sum = 0; 
            foreach (var app in appointment)
            {
                int sumOfPercent = 0;
                sum = app.PriceType == "Kredi Kartı" ? app.TotalPrice - (app.TotalPrice / 100) * (settings.KDV + settings.CreditCardTax) : app.TotalPrice -(app.TotalPrice / 100) * settings.KDV;

                sumOfPercent = string.IsNullOrEmpty(app.Broker) != true ? sumOfPercent + model.First(x => x.Id == app.Broker).Percent : sumOfPercent;
                sumOfPercent = string.IsNullOrEmpty(app.TattooArtist) != true ? sumOfPercent + model.First(x => x.Id == app.TattooArtist).Percent : sumOfPercent;
                sumOfPercent = string.IsNullOrEmpty(app.Designer) != true ? sumOfPercent + model.First(x => x.Id == app.Designer).Percent : sumOfPercent;
                sumOfPercent = string.IsNullOrEmpty(app.PiercingArtist) != true ? sumOfPercent + model.First(x => x.Id == app.PiercingArtist).Percent : sumOfPercent;

                foreach (var item in model)
                {
                    string id = item.Id.ToLower();
                    if (app.Broker == id || app.TattooArtist == id || app.PiercingArtist == id || app.Designer == id)
                    {
                        item.ReportRange = reportrange;
                        item.Amount += (sum / 100) * sumOfPercent;
                        item.AmountPercent += (sum / 100) * item.Percent;
                    }                    
                }
            }
            return View(model);
        }

        List<DepositVM> GetDeposites()
        {
            var deposites = unitOfWork.Appointment.GetAll().Select(x => new { x.DepositeAmount, x.DepositeType, x.AppointmentDate }).ToList();
            List<DepositVM> listDeposit = new List<DepositVM>();
            foreach (var item in deposites)
            {
                listDeposit.Add(new DepositVM() { Amount = item.DepositeAmount, Type = item.DepositeType, Date = item.AppointmentDate });
            }
            return listDeposit;
        }
    }
}