using Quartz;

namespace SimpleQuartz.Models
{
    [DisallowConcurrentExecution]
    public class DisallowConcurrentJobExecutor : IJob

    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
