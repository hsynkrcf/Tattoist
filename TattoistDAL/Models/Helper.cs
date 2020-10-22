using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TattoistDAL.Models.DTO;
using TattoistDAL.Models.Entities;

namespace TattoistDAL.Models
{
    public static class Helper
    {
        public static SettingsVM GetSettings()
        {
            string jsonData = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "settings.json");
            return JsonConvert.DeserializeObject<SettingsVM>(jsonData);
        }

        public static void SetSettings(int tax, int kdv, string birthday,string appointment, string birthdayText, string appointmentText, string username, string password, string sender)
        {
            SettingsVM settings = new SettingsVM()
            {
                CreditCardTax = tax,
                KDV = kdv,
                BirthdayScheduling = birthday,
                AppointmentScheduling = appointment,
                BirthdayText = birthdayText,
                AppointmentText = appointmentText,
                Username = username,
                Password = password,
                Sender = sender
            };

            string json = JsonConvert.SerializeObject(settings);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "settings.json", json);
        }

        public static void LogText(string log)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + "log.txt";

            File.AppendAllText(path, DateTime.Now+"  ---->  "+log + "\n");
        }
        public static string NameSplitter(string fullname, bool nameOrSurname)
        {
            string name = ""; string surname = "";
            var splitted = fullname.Split(' ');
            for (int i = 0; i < splitted.Length; i++)
            {
                if (i == splitted.Length - 1)
                {
                    surname = splitted[i];
                }
                else
                {
                    name += splitted[i] + " ";
                }
            }

            return nameOrSurname == true ? name : surname;
        }

        public static string StringCreator(SettingsVM settings, List<Customer> customers, int type)
        {
            string receipents = string.Empty;
            string baseAddress = string.Empty;
            for (int i = 0; i < customers.Count; i++)
            {
                if (customers.Count == 1)
                {
                    receipents += customers[i].CustomerPhone;
                }
                else if (i == customers.Count - 1)
                {
                    receipents += customers[i].CustomerPhone;
                }
                else
                {
                    receipents += customers[i].CustomerPhone + ",";
                }
            }

            if (type == 1)
            {
                baseAddress = "http://api.iletimerkezi.com/v1/send-sms/get/?username=" + settings.Username + "&password=" + settings.Password + "&text=" + settings.BirthdayText + "&receipents=" + receipents + "&sender=" + settings.Sender;
            }
            else if (type == 2)
            {
                baseAddress = "http://api.iletimerkezi.com/v1/send-sms/get/?username=" + settings.Username + "&password=" + settings.Password + "&text=" + settings.AppointmentText + "&receipents=" + receipents + "&sender=" + settings.Sender;
            }

            return baseAddress;
        }
    }
}
