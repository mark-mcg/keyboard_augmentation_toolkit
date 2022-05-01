using KAT.Layouts;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KAT
{
    /// <summary>
    /// Detects and handles docking collisions with KUIDockables.
    /// </summary>
    public class KUIDockingManager : KUIEventProvider
    {
        [BoxGroup("Debug")]
        public bool IsDocked = false;
        [BoxGroup("Config")]
        public GameObject scaleRoot;
        [BoxGroup("Config")]
        public GameObject handle;
        [BoxGroup("Config")]
        public KUIDockable dock;
        [BoxGroup("Config")]
        public KUISerializedLayout UndockedLayout;
        private KUIManager kuiManager;
        private KUILayoutManager layoutManager;

        public void OnEnable()
        {
            kuiManager = FindObjectOfType<KUIManager>();
            layoutManager = FindObjectOfType<KUILayoutManager>();

            if (scaleRoot != null)
            {
                XRGrabInteractable unityXRGrab = handle.GetComponent<XRGrabInteractable>();
                if (unityXRGrab != null)
                    unityXRGrab.onActivate.AddListener(OnGrab);

                OVRGrabbableWithEvents ovrGrab = handle.GetComponent<OVRGrabbableWithEvents>();
                if (ovrGrab != null)
                    ovrGrab.OnGrab += OvrGrab_OnGrab;
            }

            dock.OnDockingEvent += Dock_OnKUIEvent;

            DelayedDockEvent(new DockingEvent(), 0.2f, true);
        }


        private void OvrGrab_OnGrab()
        {
            UnDock();
        }

        private void OnGrab(XRBaseInteractor arg0)
        {
            UnDock();
        }

        private Coroutine NextDelayedDockEvent;

        public void DelayedDockEvent(DockingEvent @event, float delayTime, bool forceUpdate)
        {
            if (NextDelayedDockEvent != null)
                StopCoroutine(NextDelayedDockEvent);

            NextDelayedDockEvent = StartCoroutine(DelayedDockEventInternal(@event, delayTime, forceUpdate));
        }

        IEnumerator DelayedDockEventInternal(DockingEvent @event, float delayTime, bool forceUpdate)
        {
            if (delayTime > 0)
                yield return new WaitForSeconds(delayTime);
            UpdateDockingState(@event, forceUpdate);
        }


        /// <summary>
        /// For if you want to undock based on an external event e.g. an object grab
        /// 
        /// TODO - switch to delayed undocking to prevent jitter?
        /// </summary>
        /// <param name="resetParent"></param>
        public void UnDock(bool forceUpdate = false)
        {
            DockingEvent dockEvent = new DockingEvent();
            UpdateDockingState(dockEvent, forceUpdate);
        }

        private void Dock_OnKUIEvent(DockingEvent dockEvent)
        {
            if (dockEvent.docked != IsDocked)
                UpdateDockingState(dockEvent);
        }

        int lastEventFrame;
        bool disableDocking = false;
        private void UpdateDockingState(DockingEvent dockEvent, bool forceUpdate = false)
        {
            if ( !disableDocking && ( this.IsDocked != dockEvent.docked) || forceUpdate)
            { // lastEventFrame < Time.frameCount-1 &&
                this.IsDocked = dockEvent.docked;
                lastEventFrame = Time.frameCount;

                Debug.Log("UpdateDockingState DockingEvent " + dockEvent + " docking " + dockEvent.docked + " with " + dockEvent.dockedWithDockable + " layout " + dockEvent.dockedWithDockable?.GetLayout());

                if (IsDocked && dockEvent.dockedWithDockable != null)
                {
                    KUISerializedLayout dockedLayout = dockEvent.dockedWithDockable.GetLayout();

                    Debug.LogError("Enacting DOCK ENTER procedure, setting dock to " + dockEvent.dockedWithDockable.transform);
                    scaleRoot.transform.SetParent(dockEvent.dockedWithDockable.dockingPoint.transform);
                    //scaleRoot.transform.localScale = dockedLayout.LocalScale;
                    scaleRoot.transform.localPosition = Vector3.zero;// - GetDockedLayout().originOffset;
                    scaleRoot.transform.localEulerAngles = Vector3.zero;

                    //scaleRoot.transform.rotation = Quaternion.RotateTowards(dock.transform.rotation, dockEvent.dockedWithDockable.transform.rotation, float.PositiveInfinity);
                    //scaleRoot.transform.localPosition -= dockEvent.dockedWithDockable.transform.position - dock.transform.position;
                    //Debug.Log("ScaleRoot position should be offset to be " + (dockEvent.dockedWithDockable.transform.position - dock.transform.position));
                    kuiManager.KeyEventsHub.Publish(dockEvent);
                }
                else
                {
                    Vector3 originalPosition = scaleRoot.transform.position;
                    //scaleRoot.transform.localScale = UndockedLayout.LocalScale;
                    scaleRoot.transform.SetParent(null, true);
                    Debug.LogError("Enacting DOCK EXIT procedure, original position prior to exit was " + originalPosition);

                    //scaleRoot.transform.position = originalPosition;
                    //Debug.Log("scaleRoot position before scaling " + originalPosition + " after scaling " + scaleRoot.transform.position);
                    //scaleRoot.transform.position += scaleRoot.transform.position - originalPosition;
                }
                layoutManager.SetLayout(IsDocked && dockEvent.dockedWithDockable && dockEvent.dockedWithDockable.GetLayout() ? dockEvent.dockedWithDockable.GetLayout() : UndockedLayout);
                    
            }
            else
            {
                Debug.LogError("Ignoring docking event in same frame " + dockEvent + " docking " + dockEvent.docked);
            }
        }
    }
}
