namespace KAT
{
    public class TriggerStateEvent : KUIElementEvent
    {
        public TriggerState triggerState;

        public TriggerStateEvent(KUIElement element, TriggerState state) : base(element)
        {
        }
    }
}
