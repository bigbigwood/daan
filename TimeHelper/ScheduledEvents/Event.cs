using System;
using System.Xml.Serialization;
using TimeHelper.Logging;

namespace TimeHelper.ScheduledEvents
{
    /// <summary>
    /// Event is the configuration of an IEvent. 
    /// </summary>
    public class Event
    {
        private static readonly ILog logger = LogFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ///<summary>
        ///</summary>
        public Event()
        {}

        private IEvent _ievent = null;

        /// <summary>
        /// The current implementation of IEvent
        /// </summary>
        public IEvent IEventInstance
        {
            get
            {
                LoadIEvent();
                return _ievent;
            }
        }

        /// <summary>
        /// Private method for loading an instance of IEvent
        /// </summary>
        private void LoadIEvent()
        {
            if (_ievent == null)
            {
                if (this.ScheduleType == null)
                {
                    logger.Error("计划任务没有定义其 type 属性");
                }
                else
                {
                    Type type = Type.GetType(this.ScheduleType,true,false);
                    if (type == null)
                    {
                        logger.Error(string.Format("计划任务 {0} 无法被正确识别", this.ScheduleType));
                    }
                    else
                    {
                        _ievent = (IEvent)Activator.CreateInstance(type);
                        if (_ievent == null)
                        {
                            logger.Error(string.Format("计划任务 {0} 未能正确加载", this.ScheduleType));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// A unique key used to query the database. The name of the Server will also be used to ensure the "Key" is 
        /// unique in a cluster
        /// </summary>
        public string Key { get; set; }

        //private int _timeOfDay = -1;

        ///// <summary>
        ///// Absolute time in mintues from midnight. Can be used to assure event is only 
        ///// executed once per-day and as close to the specified
        ///// time as possible. Example times: 0 = midnight, 27 = 12:27 am, 720 = Noon
        ///// </summary>
        //public int TimeOfDay
        //{
        //    get { return this._timeOfDay; }
        //    set { this._timeOfDay = value; }
        //}

        //private int _minutes = 60;

        ///// <summary>
        ///// The scheduled event interval time in minutes. If TimeOfDay has a value >= 0, Minutes will be ignored. 
        ///// This values should not be less than the Timer interval.
        ///// </summary>
        //public int Minutes
        //{
        //    get
        //    {
        //        return this._minutes < EventManager.TimerMinutesInterval ? EventManager.TimerMinutesInterval : this._minutes;
        //    }
        //    set { this._minutes = value; }
        //}

        /// <summary>
        /// The Type of class which implements IEvent
        /// </summary>
        [XmlAttribute("type")]
        public string ScheduleType { get; set; }

        private DateTime _lastCompleted;

        /// <summary>
        /// Last Date and Time this event was processed/completed.
        /// </summary>
        [XmlIgnoreAttribute]
        public DateTime LastCompleted
        {
            get { return this._lastCompleted; }
            set
            {
                dateWasSet = true;
                this._lastCompleted = value;
            }
        }

        //internal testing variable
        bool dateWasSet = false;

        /////<summary>
        /////</summary>
        //[XmlIgnore]
        //public bool ShouldExecute
        //{
        //    get
        //    {
        //        //if (!dateWasSet) //if the date was not set (and it can not be configured), check the data store
        //        //{
        //        //    IScheduledEventsDao dao = ScheduledEventsDaoFatory.Create();
        //        //    LastCompleted = dao.GetLastExecuteScheduledEventDateTime(this.Key, Environment.MachineName);
        //        //}

        //        //If we have a TimeOfDay value, use it and ignore the Minutes interval
        //        if (this.TimeOfDay > -1)
        //        {
        //            //Now
        //            DateTime dtNow = DateTime.Now;  //now
        //            //We are looking for the current day @ 12:00 am
        //            DateTime dtCompare = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);

        //            //Check to see if the LastCompleted date is less than the 12:00 am + TimeOfDay minutes
        //            return LastCompleted < dtCompare.AddMinutes(this.TimeOfDay) && dtCompare.AddMinutes(this.TimeOfDay) <= DateTime.Now;
        //        }

        //        //Is the LastCompleted date + the Minutes interval less than now?
        //        return LastCompleted.AddMinutes(this.Minutes) < DateTime.Now;
        //    }
        //}

        /// <summary>
        /// Call this method BEFORE processing the ScheduledEvent. This will help protect against long running events 
        /// being fired multiple times. Note, it is not absolute protection. App restarts will cause events to look like
        /// they were completed, even if they were not. Again, ScheduledEvents are helpful...but not 100% reliable
        /// </summary>
        public void UpdateTime()
        {
            this.LastCompleted = DateTime.Now;
            //IScheduledEventsDao dao = ScheduledEventsDaoFatory.Create();
            //dao.SetLastExecuteScheduledEventDateTime(this.Key, Environment.MachineName, this.LastCompleted);
        }
    }
}
