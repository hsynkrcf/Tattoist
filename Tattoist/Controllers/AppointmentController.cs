using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tattoist.BaseController;
using TattoistDAL.EntityFramework;
using TattoistDAL.EntityFramework.Concrete;
using TattoistDAL.Models;
using TattoistDAL.Models.DTO;
using TattoistDAL.Models.Entities;

namespace Tattoist.Controllers
{
    public class AppointmentController : AppUserBaseController
    {
        UnitOfWork unitOfWork;
        public AppointmentController()
        {
            unitOfWork = new UnitOfWork(SingletonContext<AppDbContext>.Instance);
        }
        public ActionResult List()
        {
            var appointments = unitOfWork.Appointment.GetAll();
            var customers = unitOfWork.Customer.GetAll();
            var brokers = unitOfWork.Broker.GetAll();
            var employees = unitOfWork.Employee.GetAll();

            List<AppointmentList> model = new List<AppointmentList>();
            foreach (var item in appointments)
            {
                try
                {
                    AppointmentList appList = new AppointmentList();
                    if (!string.IsNullOrEmpty(item.Broker))
                    {
                        Guid brokerId = Guid.Parse(item.Broker);
                        appList.BrokerName = brokers.FirstOrDefault(x => x.BrokerId == brokerId) == null ? "" : brokers.FirstOrDefault(x => x.BrokerId == brokerId).RelatedPerson;
                    }
                    if (!string.IsNullOrEmpty(item.TattooArtist))
                    {
                        Guid tatto = Guid.Parse(item.TattooArtist);
                    appList.TattoistName = employees.FirstOrDefault(x => x.EmployeeId == tatto) == null ? "" : employees.FirstOrDefault(x => x.EmployeeId == tatto).EmployeeName;
                    }
                    if (!string.IsNullOrEmpty(item.PiercingArtist))
                    {
                        Guid pier = Guid.Parse(item.PiercingArtist);
                    appList.PiercingistName = employees.FirstOrDefault(x => x.EmployeeId == pier) == null ? "" : employees.FirstOrDefault(x => x.EmployeeId == pier).EmployeeName;
                    }
                    if (!string.IsNullOrEmpty(item.Designer))
                    {
                        Guid design = Guid.Parse(item.Designer);
                    appList.DesignerName = employees.FirstOrDefault(x=>x.EmployeeId == design) == null ? "" : employees.FirstOrDefault(x => x.EmployeeId == design).EmployeeName;
                    }

                    appList.AppointmentId = item.AppointmentId.ToString();
                    appList.AppointmentDate = item.AppointmentDate;
                    appList.CustomerName = customers.FirstOrDefault(x => x.CustomerId == item.CustomerId) == null ? "" : customers.FirstOrDefault(x => x.CustomerId == item.CustomerId).CustomerName;
                    appList.DepositeAmount = item.DepositeAmount;
                    appList.DepositeType = item.DepositeType;
                    appList.Phone = item.Phone;
                    appList.IsClosed = item.IsClosed;
                    appList.TotalPrice = item.TotalPrice;
                    appList.WorkType = item.WorkType;
                    model.Add(appList);
                }
                catch (Exception ex)
                {
                    continue;
                }
            };


            return View(model);
        }

        [Authorize(Roles = "Admin,Designer,Tattoo")]
        public ActionResult System()
        {
            var customers = unitOfWork.Customer.GetAll().ToList();
            var tattoo = unitOfWork.Employee.GetAll(x => x.EmployeeType == 1).ToList();
            var piercing = unitOfWork.Employee.GetAll(x => x.EmployeeType == 2).ToList();
            var designer = unitOfWork.Employee.GetAll(x => x.EmployeeType == 3).ToList();
            var broker = unitOfWork.Broker.GetAll().ToList();

            var model = new AppointmentViewModel()
            {
                Customers = new SelectList(customers, "CustomerId", "CustomerName"),
                Piercingist = new SelectList(piercing, "EmployeeId", "NameWithPercent"),
                Tattoist = new SelectList(tattoo, "EmployeeId", "NameWithPercent"),
                BrokerList = new SelectList(broker, "BrokerId", "NameWithPercent"),
                DesignerList = new SelectList(designer, "EmployeeId", "NameWithPercent")
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SystemDelete(string id)
        {
            var guid = Guid.Parse(id);
            var appointment = unitOfWork.Appointment.Get(x => x.AppointmentId == guid);
            unitOfWork.Appointment.Remove(appointment);
            unitOfWork.Complete();
            ViewBag.success = "Randevu Başarıyla Silindi";
            return RedirectToAction("List");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Broker()
        {
            var model = unitOfWork.Broker.GetAll().ToList();
            return View(model);
        }


        [HttpPost]
        public ActionResult Broker(BrokerViewModel viewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(viewModel.brokerid))
                {
                    Broker broker = new Broker();
                    broker.BrokerId = Guid.NewGuid();
                    broker.BrokerTitle = viewModel.Title;
                    broker.BrokerPercent = int.Parse(viewModel.Percent);
                    broker.RelatedPerson = viewModel.RelatedPerson;
                    broker.BrokerPhone = viewModel.Phone;
                    if (!string.IsNullOrEmpty(viewModel.Iban))
                    {
                        broker.Iban = viewModel.Iban;
                    }
                    else
                    {
                        broker.AccountNumber = viewModel.Account;
                        broker.BranchCode = viewModel.Branch;
                        broker.BankName = viewModel.BankName;
                    }

                    unitOfWork.Broker.Add(broker);
                    unitOfWork.Complete();

                    ViewBag.success = "Aracı Başarıyla Eklendi";
                    var model = unitOfWork.Broker.GetAll().ToList();

                    return View(model);
                }
                else
                {
                    Guid id = Guid.Parse(viewModel.brokerid);
                    var broker = unitOfWork.Broker.Get(x => x.BrokerId == id);
                    broker.BrokerTitle = viewModel.Title;
                    broker.BrokerPercent = int.Parse(viewModel.Percent);
                    broker.RelatedPerson = viewModel.RelatedPerson;
                    broker.BrokerPhone = viewModel.Phone;
                    broker.PaymentType = viewModel.PaymentType;

                    if (!string.IsNullOrEmpty(viewModel.Iban))
                    {
                        broker.Iban = viewModel.Iban;

                        broker.AccountNumber = "";
                        broker.BranchCode = "";
                        broker.BankName = "";
                    }
                    else
                    {
                        broker.AccountNumber = viewModel.Account;
                        broker.BranchCode = viewModel.Branch;
                        broker.BankName = viewModel.BankName;

                        broker.Iban = "";

                    }

                    unitOfWork.Broker.Update(broker);
                    unitOfWork.Complete();

                    ViewBag.success = "Aracı Başarıyla Güncellendi";
                    var model = unitOfWork.Broker.GetAll().ToList();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Aracı Eklenemedi. Hata:" + ex;
                return View();
            }
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult BrokerDelete(string id)
        {
            var guid = Guid.Parse(id);
            var broker = unitOfWork.Broker.Get(x => x.BrokerId == guid);
            unitOfWork.Broker.Remove(broker);
            unitOfWork.Complete();
            ViewBag.success = "Aracı Başarıyla Silindi";
            return RedirectToAction("Broker");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Contact()
        {
            var model = unitOfWork.Customer.GetAll().ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Contact(string fullname, string phone, string birthdate, string customerid)
        {
            try
            {
                if (string.IsNullOrEmpty(customerid))
                {
                    unitOfWork.Customer.Add(new Customer()
                    {
                        CustomerId = Guid.NewGuid(),
                        CustomerName = fullname,
                        CustomerPhone = phone,
                        CustomerBirthday = DateTime.Parse(birthdate)
                    });
                    unitOfWork.Complete();
                    ViewBag.success = "Müşteri Başarıyla Eklendi";
                    var model = unitOfWork.Customer.GetAll().ToList();
                    return View(model);
                }
                else
                {
                    Guid id = Guid.Parse(customerid);
                    var customer = unitOfWork.Customer.Get(x => x.CustomerId == id);
                    customer.CustomerName = fullname;
                    customer.CustomerPhone = phone;
                    customer.CustomerBirthday = DateTime.Parse(birthdate);

                    unitOfWork.Customer.Update(customer);
                    unitOfWork.Complete();

                    ViewBag.success = "Müşteri Başarıyla Güncellendi";
                    var model = unitOfWork.Customer.GetAll().ToList();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Müşteri Eklenemedi. Hata:" + ex;
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ContactDelete(string id)
        {
            var guid = Guid.Parse(id);
            var customer = unitOfWork.Customer.Get(x => x.CustomerId == guid);
            unitOfWork.Customer.Remove(customer);
            unitOfWork.Complete();
            ViewBag.success = "Müşteri Başarıyla Silindi";
            return RedirectToAction("Contact");
        }



        [Authorize(Roles = "Admin")]
        public ActionResult Tattoo()
        {
            var tattoo = unitOfWork.Employee.GetAll(x => x.EmployeeType == 1).ToList();
            return View(tattoo);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Tattoo(string fullname, string percent, string phone, string userid)
        {
            try
            {
                if (string.IsNullOrEmpty(userid))
                {
                    unitOfWork.Employee.Add(new Employee()
                    {
                        EmployeeId = Guid.NewGuid(),
                        EmployeeName = fullname,
                        EmployeePercent = int.Parse(percent),
                        Phone = phone,
                        EmployeeType = 1
                    });
                    unitOfWork.Complete();
                    ViewBag.success = "Sanatçı Başarıyla Eklendi";
                    var tattoo = unitOfWork.Employee.GetAll(x => x.EmployeeType == 1).ToList();
                    return View(tattoo);
                }
                else
                {
                    Guid id = Guid.Parse(userid);
                    var employee = unitOfWork.Employee.Get(x => x.EmployeeId == id);
                    employee.EmployeeName = fullname;
                    employee.EmployeePercent = int.Parse(percent);
                    employee.Phone = phone;

                    unitOfWork.Employee.Update(employee);
                    unitOfWork.Complete();
                    ViewBag.success = "Sanatçı Başarıyla Güncellendi";
                    var tattoo = unitOfWork.Employee.GetAll(x => x.EmployeeType == 1).ToList();
                    return View(tattoo);
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Sanatçı Eklenemedi. Hata:" + ex;
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult TattooDelete(string id)
        {
            var guid = Guid.Parse(id);
            var employee = unitOfWork.Employee.Get(x => x.EmployeeId == guid);
            unitOfWork.Employee.Remove(employee);
            unitOfWork.Complete();
            ViewBag.success = "Sanatçı Başarıyla Silindi";
            return RedirectToAction("Tattoo");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Piercing()
        {
            var tattoo = unitOfWork.Employee.GetAll(x => x.EmployeeType == 2).ToList();
            return View(tattoo);
        }
        [HttpPost]
        public ActionResult Piercing(string fullname, string percent, string phone, string userid)
        {
            try
            {
                if (string.IsNullOrEmpty(userid))
                {
                    unitOfWork.Employee.Add(new Employee()
                    {
                        EmployeeId = Guid.NewGuid(),
                        EmployeeName = fullname,
                        EmployeePercent = int.Parse(percent),
                        Phone = phone,
                        EmployeeType = 2
                    });
                    unitOfWork.Complete();
                    ViewBag.success = "Sanatçı Başarıyla Eklendi";
                    var tattoo = unitOfWork.Employee.GetAll(x => x.EmployeeType == 2).ToList();
                    return View(tattoo);
                }
                else
                {
                    Guid id = Guid.Parse(userid);
                    var employee = unitOfWork.Employee.Get(x => x.EmployeeId == id);
                    employee.EmployeeName = fullname;
                    employee.EmployeePercent = int.Parse(percent);
                    employee.Phone = phone;

                    unitOfWork.Employee.Update(employee);
                    unitOfWork.Complete();
                    ViewBag.success = "Sanatçı Başarıyla Güncellendi";
                    var tattoo = unitOfWork.Employee.GetAll(x => x.EmployeeType == 2).ToList();
                    return View(tattoo);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Sanatçı Eklenemedi. Hata:" + ex;
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult PiercingDelete(string id)
        {
            var guid = Guid.Parse(id);
            var employee = unitOfWork.Employee.Get(x => x.EmployeeId == guid);
            unitOfWork.Employee.Remove(employee);
            unitOfWork.Complete();
            ViewBag.success = "Sanatçı Başarıyla Silindi";
            return RedirectToAction("Piercing");
        }



        [Authorize(Roles = "Admin")]
        public ActionResult Designer()
        {
            var tattoo = unitOfWork.Employee.GetAll(x => x.EmployeeType == 3).ToList();
            return View(tattoo);
        }
        [HttpPost]
        public ActionResult Designer(string fullname, string percent, string phone, string userid)
        {
            try
            {
                if (string.IsNullOrEmpty(userid))
                {
                    unitOfWork.Employee.Add(new Employee()
                    {
                        EmployeeId = Guid.NewGuid(),
                        EmployeeName = fullname,
                        EmployeePercent = int.Parse(percent),
                        Phone = phone,
                        EmployeeType = 3
                    });
                    unitOfWork.Complete();
                    ViewBag.success = "Designer Başarıyla Eklendi";
                    var tattoo = unitOfWork.Employee.GetAll(x => x.EmployeeType == 3).ToList();
                    return View(tattoo);
                }
                else
                {
                    Guid id = Guid.Parse(userid);
                    var employee = unitOfWork.Employee.Get(x => x.EmployeeId == id);
                    employee.EmployeeName = fullname;
                    employee.EmployeePercent = int.Parse(percent);
                    employee.Phone = phone;

                    unitOfWork.Employee.Update(employee);
                    unitOfWork.Complete();
                    ViewBag.success = "Designer Başarıyla Güncellendi";
                    var tattoo = unitOfWork.Employee.GetAll(x => x.EmployeeType == 3).ToList();
                    return View(tattoo);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Designer Eklenemedi. Hata:" + ex;
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DesignerDelete(string id)
        {
            var guid = Guid.Parse(id);
            var employee = unitOfWork.Employee.Get(x => x.EmployeeId == guid);
            unitOfWork.Employee.Remove(employee);
            unitOfWork.Complete();
            ViewBag.success = "Designer Başarıyla Silindi";
            return RedirectToAction("Designer");
        }
    }
}