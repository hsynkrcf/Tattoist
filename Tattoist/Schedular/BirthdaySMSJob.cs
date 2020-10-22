using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TattoistDAL.Models;
using TattoistDAL.Models.DTO;
using TattoistDAL.Models.Entities;

namespace Tattoist.Schedular
{
    public class BirthdaySMSJob : IJob
    {
        AppDbContext _db;
        public BirthdaySMSJob()
        {
            _db = new AppDbContext();
        }
        async Task IJob.Execute(IJobExecutionContext context)
        {
            try
            {
                string birthdayAddress = string.Empty;
                var settings = TattoistDAL.Models.Helper.GetSettings();
                var customer = _db.Customers.ToList();

                var birthdayCustomers = customer.Where(x => x.CustomerBirthday.Day == DateTime.Now.Day && x.CustomerBirthday.Month == DateTime.Now.Month).ToList();

                if (birthdayCustomers.Count != 0)
                {
                    birthdayAddress = Helper.StringCreator(settings, birthdayCustomers, 1);

                }

                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(birthdayAddress))
                    {
                        string birthdayResponse = await client.GetStringAsync(birthdayAddress);
                        Helper.LogText(birthdayResponse);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}