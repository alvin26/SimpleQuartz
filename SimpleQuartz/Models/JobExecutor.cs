using Quartz;
using System.Diagnostics;

namespace SimpleQuartz.Models
{

    public class JobExecutor : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Debug.Print($"{DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
