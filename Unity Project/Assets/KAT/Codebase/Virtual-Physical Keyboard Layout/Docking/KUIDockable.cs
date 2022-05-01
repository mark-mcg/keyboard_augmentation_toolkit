using KAT.Layouts;
using NaughtyAttributes;
using UnityEngine;

namespace KAT
{
    /// <summary>
    /// Indicates a collidable Dock which can specify a keyboard layout to switch to.
    /// </summary>
    public class KUIDockable : KUIEventProvider {
        [ReadOnly]
        public Collider dockedCollider;
        //public KUILayoutProvider LayoutProvider;
        public KUISerializedLayout Layout;
        //public bool HideLayoutWhenNotDocked = true;
        //public bool BuiltLayoutOnAwake = false;
        public GameObject dockingPoint;


        public void Awake()
        {
            //if (Layout != null && BuiltLayoutOnAwake)
            //{
            //    GameObject layoutGO = Layout.BuildLayout();
            //    layoutGO.transform.SetParent(this.transform, false);
            //    layoutGO.SetActive(!HideLayoutWhenNotDocked);
            //}

            if (dockingPoint == null)
                dockingPoint = this.gameObject;
        }

        public KUISerializedLayout GetLayout()
        {
            return Layout;
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckForDock(other, true);
        }

        private void OnCollisionEnter(Collision collision)
        {
            CheckForDock(collision.collider, true);
        }

        private void OnTriggerExit(Collider other)
        {
            CheckForDock(other, false);
        }

        private void OnCollisionExit(Collision collision)
        {
            CheckForDock(collision.collider, false);
        }

        public delegate void OnDockingEventDelegate(DockingEvent @event);
        public event OnDockingEventDelegate OnDockingEvent;

        private KUIDockable CheckForDock(Collider collider, bool enter)
        {
            KUIDockable dockable = collider.gameObject.GetComponent<KUIDockable>();
            if (dockable != null && (enter || (!enter && dockedCollider == collider))) { 
                if (enter)
                {
                    //Debug.LogError(">>>DOCK ENTER for " + this.gameObject + " frame " + Time.frameCount);
                    dockedCollider = collider;
                }
                else if (!enter && dockedCollider == collider)
                {
                    //Debug.LogError(">>>DOCK EXIT for " + this.gameObject + " frame " + Time.frameCount);
                    dockedCollider = null;
                }
                if (OnDockingEvent!=null) OnDockingEvent(new DockingEvent(enter, dockedCollider, dockable));
            }

            return dockable;
        }

    }
}
