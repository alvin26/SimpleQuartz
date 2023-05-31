using Microsoft.AspNetCore.Mvc;
using Quartz;
using SimpleQuartz.Models;
using System.Text;

namespace SimpleQuartz.Controllers
{
    public class QuartzController : Controller
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILogger<QuartzController> _logger;
        public QuartzController(ISchedulerFactory schedulerFactory, ILogger<QuartzController> logger)
        {
            _schedulerFactory = schedulerFactory;
            _logger = logger;   
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListJobs()
        {
            _logger.LogDebug("Into ListJobs");
            var rst = "OK";
            StringBuilder sb = new StringBuilder();
            var scheduler = await _schedulerFactory.GetScheduler();
            var keys = scheduler.Context.GetKeys();
            //scheduler.Context.get
            var executingJobs = await scheduler.GetCurrentlyExecutingJobs();
            foreach (var job in executingJobs)
            {
                sb.AppendLine($"{job.JobDetail.Description}");
            }
            rst = $"executing jobs:{sb}.";
            return Ok(rst);
        }
        public async Task<IActionResult> test()
        {
            _logger.LogDebug("Into test");
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
