namespace TimeHelper.ScheduledEvents
{
    /// <summary>
    /// Interface for defining an event.
    /// </summary>
    public interface IEvent
    {
        ///<summary>
        ///</summary>
        ///<param name="state"></param>
        void Execute(object state);
    }
}
