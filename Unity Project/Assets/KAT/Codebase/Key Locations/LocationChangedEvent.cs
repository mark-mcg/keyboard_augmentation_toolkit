using System.Collections.Generic;

namespace KAT
{
    public class LocationChangedEvent : KUIElementEvent
    {
        public List<KUILocation> locationsActive, locationsInactive;
        public LocationChangedEvent(KUIElement element, List<KUILocation> locationsActive, List<KUILocation> locationsInactive) : base(element)
        {
            this.locationsActive = locationsActive;
            this.locationsInactive = locationsInactive;

        }
    }
}
