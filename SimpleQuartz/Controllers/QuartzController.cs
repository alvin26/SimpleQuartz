using Microsoft.AspNetCore.Mvc;
using Quartz;
using SimpleQuartz.Models;

namespace SimpleQuartz.Controllers
{
    public class QuartzController : Controller
    {
        private readonly ISchedulerFactory _schedulerFactory;
        public QuartzController(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> test()
        {
            var rst = "OK";

            var scheduler = await _schedulerFactory.GetScheduler();
            
            //JobBase
            // define the job and tie it to our HelloJob class
            var job = JobBuilder.Create<JobExecutor>()
                .WithIdentity($"myJob{DateTime.Now.ToString("yyyy-MM-ddHHmmss.fff")}", "group1")
                .Build();

            // Trigger the job to run now, and then every 40 seconds
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"myTrigger{DateTime.Now.ToString("yyyy-MM-ddHHmmss.fff")}", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(1)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);

            return Ok(rst);
        }
    }
}
