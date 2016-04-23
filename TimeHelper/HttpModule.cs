using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Xml;
using TimeHelper.Config;
using TimeHelper.ScheduledEvents;

namespace TimeHelper
{
    ///<summary>
    ///</summary>
    [ParseChildren(true), PersistChildren(false)]
    public class HttpModule : IHttpModule
    {
        static Timer eventTimer;

        /// <summary>
        /// ʵ�ֽӿڵ�Init����
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            
            if (eventTimer == null && WshelperConfigs.GetConfig().ScheduledEnabled)
            {
                eventTimer = new Timer(ScheduledEventWorkCallback, context.Context, 0, EventManager.TimerMinutesInterval * 1000 * 60);
            }
        }

        private void ScheduledEventWorkCallback(object sender)
        {
            try
            {
                if (WshelperConfigs.GetConfig().ScheduledEnabled)
                {
                    EventManager.Execute();
                }
            }
            catch (Exception ex)
            {
                //    logger.Error(String.Format("ִ�мƻ��������:\r\n{0}", ex));
            }
        }

        /// <summary>
        /// ʵ�ֽӿڵ�Dispose����
        /// </summary>
        public void Dispose()
        {
        }

       
    }

}
