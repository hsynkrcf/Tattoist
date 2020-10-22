using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using TattoistDAL.Models;
using TattoistDAL.Models.Entities;

namespace Tattoist.Schedular
{
    public class AppointmentSMSJob : IJob
    {
        AppDbContext _db;
        public AppointmentSMSJob()
        {
            _db = new AppDbContext();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                string appointmentAddress = string.Empty;
                DateTime tomorrowland = DateTime.Now.AddDays(1).Date;
                var settings = TattoistDAL.Models.Helper.GetSettings();
                var customer = _db.Customers.ToList();
                var appointment = _db.Appointments.Where(x => x.AppointmentDate == tomorrowland).ToList();


                var appointmentCustomers = new List<Customer>();
                if (appointment.Count != 0)
                {
                    foreach (var item in appointment)
                    {
                        appointmentCustomers.Add(customer.SingleOrDefault(x => x.CustomerId == item.CustomerId));
                    }
                }

                if (appointmentCustomers.Count != 0)
                {
                    appointmentAddress = Helper.StringCreator(settings, appointmentCustomers, 2);
                }

                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(appointmentAddress))
                    {
                        string appointmentResponse = await client.GetStringAsync(appointmentAddress);
                        Helper.LogText(appointmentResponse);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}