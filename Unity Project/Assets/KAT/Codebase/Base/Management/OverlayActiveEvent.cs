namespace KAT
{
    public class OverlayActiveEvent : BaseKUIEvent
    {
        public bool isActive;
        public OverlayActiveEvent(bool active)
        {
            this.isActive = active;
        }
    }
}
