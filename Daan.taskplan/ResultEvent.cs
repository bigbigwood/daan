using System;
using System.Collections.Generic;
using System.Linq;
using TimeHelper.ScheduledEvents;
using daan.service.proceed;
using TimeHelper.Logging;

namespace daan.taskplan
{
    public class ResultEvent : IEvent
    {
        private static readonly ILog logger = LogFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(object state)
        {
            try
            {
                //获取结果 并且写日志  true 为不自动接收
                logger.Info(Environment.NewLine + new ProDataReceiveService().DownResult(false, null, null, null));
            }
            catch (Exception e)
            {
                logger.Debug("自动获取结果异常:", e);
            }
        }
    }
}
