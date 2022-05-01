namespace KAT
{
    public class KUIElementEvent : BaseKUIEvent
    {
        public KUIElement element;
        public KUIElementEvent(KUIElement element)
        {
            this.element = element;
        }
    }
}
