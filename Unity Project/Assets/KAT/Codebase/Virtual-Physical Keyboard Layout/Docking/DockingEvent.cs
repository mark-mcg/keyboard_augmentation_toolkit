using UnityEngine;

namespace KAT
{
    public class DockingEvent : BaseKUIEvent
    {
        public bool docked;
        public Collider dockedWith;
        public KUIDockable dockedWithDockable;

        public DockingEvent(bool docked, Collider with, KUIDockable dockedWithDockable)
        {
            this.docked = docked;
            this.dockedWith = with;
            this.dockedWithDockable = dockedWithDockable;
        }

        public DockingEvent()
        {
            this.docked = false;
            this.dockedWith = null;
            this.dockedWithDockable = null;
        }
    }
}
