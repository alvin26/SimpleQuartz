using Quartz;
using SimpleQuartz.Common.Base;

namespace JobForExample
{
    public class Job : JobBase
    {
        public override async Task Execute(IJobExecutionContext context)
        {
            this._logger.Debug($"Example Job Executed!");
            
        }
    }
}