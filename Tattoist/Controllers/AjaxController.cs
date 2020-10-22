using RGNMarketPlace.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Tattoist.BaseController;
using TattoistDAL.EntityFramework;
using TattoistDAL.EntityFramework.Concrete;
using TattoistDAL.Models;
using TattoistDAL.Models.DTO;
using TattoistDAL.Models.Entities;

namespace RGNMarketPlace.Controllers
{
    public class AjaxController : AppUserBaseController
    {
        UnitOfWork unitOfWork;
        public AjaxController()
        {
            unitOfWork = new UnitOfWork(SingletonContext<AppDbContext>.Instance);
        }

        public JsonResult AjaxCreate(User user)
        {
            try
            {
                user.Id = Guid.NewGuid();
                unitOfWork.User.Add(user);
                unitOfWork.Complete();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AjaxEdit(User user)
        {
            try
            {
                unitOfWork.User.DetachedUpdate(user);
                unitOfWork.Complete();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult MyProfile(User user)
        {
            try
            {
                var updatedUser = unitOfWork.User.Get(x => x.Id == user.Id);
                updatedUser.FullName = user.FullName;
                updatedUser.UserName = user.UserName;
                updatedUser.Email = user.Email;
                updatedUser.Password = user.Password;


                unitOfWork.User.DetachedUpdate(updatedUser);
                unitOfWork.Complete();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CreateCustomer(Customer customer)
        {
            try
            {
                customer.CustomerId = Guid.NewGuid();
                unitOfWork.Customer.Add(customer);
                unitOfWork.Complete();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Login(LoginViewModel login)
        {
            try
            {
                var user = unitOfWork.User.Get(x => x.UserName == login.Username && x.Password == login.Password);
                if (user != null)
                {
                    Session["User"] = user;
                    FormsAuthentication.SetAuthCookie(login.Username, true);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Register(User user)
        {
            try
            {
                user.Id = Guid.NewGuid();
                user.UserType = 4;
                unitOfWork.User.Add(user);
                unitOfWork.Complete();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public string TestFile()
        {
            try
            {
                System.IO.File.Move(Server.MapPath($"~/TempImages/temp.png"), Server.MapPath($"~/Images/temp.png"));
                return "success";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
        public JsonResult SaveAppointment(AppointmentJsonVM appointment)
        {
            try
            {
                Guid appointmentId = Guid.NewGuid();
                if (!string.IsNullOrEmpty(appointment.PaymentList[0]))
                {
                    for (int i = 0; i < appointment.PaymentList.Count; i++)
                    {
                        unitOfWork.Payment.Add(new Payment()
                        {
                            AppointmentId = appointmentId,
                            LineId = Guid.NewGuid(),
                            CustomerId = Guid.Parse(appointment.Customer),
                            PaymentAmount = int.Parse(appointment.PaymentList[i]),
                            PaymentType = int.Parse(appointment.PaymentTypeList[i]),
                            PaymentDate = DateTime.Parse(appointment.PaymentDateList[i])
                        });
                    }
                }

                unitOfWork.Appointment.Add(new Appointment()
                {
                    AppointmentId = appointmentId,
                    CustomerId = Guid.Parse(appointment.Customer),
                    AppointmentDate = DateTime.Parse(appointment.AppointmentDate),
                    Phone = appointment.Phone,
                    WorkType = appointment.WorkType,
                    DepositeAmount = appointment.DepositeAmount,
                    DepositeType = appointment.DepositeType,
                    TattooArtist = appointment.TattooArtist == null ? "" : appointment.TattooArtist,
                    PiercingArtist = appointment.PiercingArtist == null ? "" : appointment.PiercingArtist,
                    Broker = appointment.Broker == null ? "" : appointment.Broker,
                    Designer = appointment.Designer == null ? "" : appointment.Designer,
                    IsClosed = appointment.IsClosed,
                    TotalPrice = appointment.TotalPrice,
                    PriceType = appointment.PriceType
                });

                unitOfWork.Complete();
                if (!string.IsNullOrEmpty(appointment.HeaderId))
                {
                    Guid id = Guid.Parse(appointment.HeaderId);
                    if (System.IO.File.Exists(Server.MapPath($"/Images/") + appointment.HeaderId + ".jpg")) System.IO.File.Exists(Server.MapPath($"/Images/") + appointment.HeaderId + ".jpg");
                    var model = unitOfWork.Appointment.Get(x => x.AppointmentId == id);
                    var payment = unitOfWork.Payment.GetAll(x => x.AppointmentId == id);
                    unitOfWork.Appointment.Remove(model);
                    unitOfWork.Payment.RemoveRange(payment);
                    unitOfWork.Complete();
                }
                string s = Server.MapPath($"/TempImages/temp.jpg");
                if (System.IO.File.Exists(s))
                {
                    FileOperation(appointmentId.ToString());
                }
                else
                {
                    string sourceFile = Server.MapPath($"/Images/") + appointment.HeaderId + ".jpg";
                    System.IO.FileInfo fi = new System.IO.FileInfo(sourceFile);
                    if (fi.Exists)
                    {
                        fi.MoveTo(Server.MapPath($"/Images/") + appointmentId + ".jpg");
                    }
                }
                return Json(appointmentId, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }


        public void FileOperation(string name)
        {
            if (!Directory.Exists(Server.MapPath($"/Images/")))
            {
                Directory.CreateDirectory(Server.MapPath($"/Images/"));
            }

            string[] dizindekiDosyalar = Directory.GetFiles(Server.MapPath($"/TempImages/"));
            //.Replace("temp", productCode)
            foreach (string dosya in dizindekiDosyalar)
            {
                string newFile = Server.MapPath($"/Images/") + name + ".jpg";
                FileInfo fileInfo = new FileInfo(dosya);
                if (System.IO.File.Exists(newFile))
                {
                    System.IO.File.Delete(newFile);
                }
                System.IO.File.Move(fileInfo.FullName, newFile);
            }
        }

        [HttpGet]
        public JsonResult GetAppointment(string jsonData)
        {
            List<string> paymentDateList = new List<string>();
            List<string> paymentTypeList = new List<string>();
            List<string> paymentAmountList = new List<string>();

            try
            {
                Guid appointmentID = Guid.Parse(jsonData);
                var appointment = unitOfWork.Appointment.Get(x => x.AppointmentId == appointmentID);
                var payments = unitOfWork.Payment.GetAll(x => x.AppointmentId == appointmentID);


                foreach (var item in payments)
                {
                    paymentDateList.Add(item.PaymentDate.ToString("yyyy-MM-dd"));
                    paymentTypeList.Add(item.PaymentType.ToString());
                    paymentAmountList.Add(item.PaymentAmount.ToString());
                }
                var model = new AppointmentJsonVM()
                {
                    AppointmentDate = appointment.AppointmentDate.ToString("yyyy-MM-dd"),
                    Broker = appointment.Broker,
                    Customer = appointment.CustomerId.ToString(),
                    DepositeAmount = appointment.DepositeAmount,
                    DepositeType = appointment.DepositeType,
                    PriceType = appointment.PriceType,
                    Designer = appointment.Designer,
                    WorkType = appointment.WorkType,
                    IsClosed = appointment.IsClosed,
                    Phone = appointment.Phone,
                    TotalPrice = appointment.TotalPrice,
                    TattooArtist = appointment.TattooArtist,
                    PiercingArtist = appointment.PiercingArtist,
                    PaymentDateList = paymentDateList,
                    PaymentTypeList = paymentTypeList,
                    PaymentList = paymentAmountList,
                    HeaderId = appointment.AppointmentId.ToString()
                };

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetEmployee(string jsonData)
        {
            try
            {
                Guid employeeID = Guid.Parse(jsonData);
                var employee = unitOfWork.Employee.Get(x => x.EmployeeId == employeeID);

                return Json(employee, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetCustomers(string jsonData)
        {
            try
            {
                Guid customerID = Guid.Parse(jsonData);
                var customer = unitOfWork.Customer.Get(x => x.CustomerId == customerID);
                customer.Date = customer.CustomerBirthday.ToString("yyyy-MM-dd");

                return Json(customer, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetExpense(string jsonData)
        {
            try
            {
                Guid expenseId = Guid.Parse(jsonData);
                var expense = unitOfWork.Expense.Get(x => x.Id == expenseId);
                expense.Time = expense.Date.ToString("yyyy-MM-dd");
                return Json(expense, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetBroker(string jsonData)
        {
            try
            {
                Guid brokerId = Guid.Parse(jsonData);
                var broker = unitOfWork.Broker.Get(x => x.BrokerId == brokerId);

                return Json(broker, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                string path = Server.MapPath($"/TempImages/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Server.MapPath($"/TempImages/temp.jpg"));
                    }
                }
                else if (System.IO.File.Exists(Server.MapPath($"/TempImages/temp.jpg")))
                {
                    System.IO.File.Delete(Server.MapPath($"/TempImages/temp.jpg"));
                }
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}