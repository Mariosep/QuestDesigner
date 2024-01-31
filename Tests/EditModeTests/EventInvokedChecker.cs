public class EventInvokedChecker
{
    public bool eventHasBeenInvoked = false;
    
    public void OnEventInvoked(object o)
    {
        eventHasBeenInvoked = true;
    }
}