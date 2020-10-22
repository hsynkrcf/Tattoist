using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Tattoist.Schedular;
using TattoistDAL.Models;

namespace QuartzNetTest.Scheduling
{
    public class SchedularWrapper
    {
        public void Start()
        {
            var settings = Helper.GetSettings();
            var birthdaySplitted = settings.BirthdayScheduling.Split(':');
            var appointmentSplitted = settings.AppointmentScheduling.Split(':');

            try
            {
                var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Result.Start();

                IJobDetail birthdayJob = JobBuilder.Create<BirthdaySMSJob>().Build();

                ITrigger birthdayTrigger = TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule
                      (s =>
                         s
                        .OnEveryDay() 
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(int.Parse(birthdaySplitted[0]), int.Parse(birthdaySplitted[1])))
                      )
                    .Build();

                IJobDetail appointmentJob = JobBuilder.Create<AppointmentSMSJob>().Build();

                ITrigger appointmentTrigger = TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule
                      (s =>
                         s
                        .OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(int.Parse(appointmentSplitted[0]), int.Parse(appointmentSplitted[1])))
                      )
                    .Build();

                scheduler.Result.ScheduleJob(birthdayJob, birthdayTrigger);
                scheduler.Result.ScheduleJob(appointmentJob, appointmentTrigger);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}