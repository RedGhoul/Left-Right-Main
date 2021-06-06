using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Hangfire
{
    public class HangFireJobScheduler
    {
        public static void ScheduleRecurringJobs()
        {
            RecurringJob.RemoveIfExists(nameof(GetHeadLinesJob));
            RecurringJob.AddOrUpdate<GetHeadLinesJob>(nameof(GetHeadLinesJob),
                job => job.Run(JobCancellationToken.Null),
                "0 */2 * * *", TimeZoneInfo.Local);
        }
    }
}
