using NLog;
using Quartz;

namespace SimpleQuartz.Common.Base
{
    public abstract class JobBase : IJob
    {
        public ILogger _logger = LogManager.Setup().LoadConfigurationFromFile().GetCurrentClassLogger();
        public JobBase()
        {

        }


        public virtual async Task Execute(IJobExecutionContext context)
        {

        }
    }
}