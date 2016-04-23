using System.Collections.Generic;
using TimeHelper.Config;

namespace TimeHelper.ScheduledEvents
{
    /// <summary>
    /// EventManager is called from the EventHttpModule (or another means of scheduling a Timer). Its sole purpose
    /// is to iterate over an array of Events and deterimine of the Event's IEvent should be processed. All events are
    /// added to the managed threadpool. 
    /// </summary>
    public class EventManager
    {
        private EventManager()
        {
        }

        ///<summary>
        /// 计划任务间隔初始分钟
        ///</summary>
        public static readonly int TimerMinutesInterval = 5;

        static EventManager()
        {
            if (WshelperConfigs.GetConfig().TimerMinutesInterval > 0)
            {
                TimerMinutesInterval = WshelperConfigs.GetConfig().TimerMinutesInterval;
            }
        }

        ///<summary>
        /// 执行计划任务
        ///</summary>
        public static void Execute()
        {
            Config.Event[] simpleItems = WshelperConfigs.GetConfig().ScheduledEvents;

            List<Event> list = new List<Event>();
            foreach (Config.Event newEvent in simpleItems)
            {
                if (!newEvent.Enabled)
                {
                    continue;
                }
                Event e = new Event();
                e.Key = newEvent.Key;
                e.ScheduleType = newEvent.ScheduleType;
                list.Add(e);
            }

            Event[] items = list.ToArray();
            Event item = null;
            for (int i = 0; i < items.Length; i++)
            {
                item = items[i];
                //if (item.ShouldExecute)
                //{
                //    item.UpdateTime();
                IEvent e = item.IEventInstance;
                ManagedThreadPool.QueueUserWorkItem(e.Execute);
                //}
            }
        }
    }
}
