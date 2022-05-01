using BidirectionalMap;
using BiLookup;
using KAT.Layouts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriangleNet;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using KAT.Interaction;
using NaughtyAttributes;
using PubSub;

namespace KAT
{
    [RequireComponent(typeof(KUILocationManager))]
    [RequireComponent(typeof(KUIMappingManager))]
    [RequireComponent(typeof(KUIDockingManager))]
    [RequireComponent(typeof(KUILayoutManager))]
    [RequireComponent(typeof(KUIMappingToLocationBinder))]
    [RequireComponent(typeof(KUIKeyboardState))]

    public class KUIManager : KUIParentChildEventProvider
    {
        private Hub hub = new Hub();
        public Hub KeyEventsHub { get => hub; }

        private Hub nonKeyEvents = new Hub();
        public Hub NonKeyEvents { get => hub; }

        [HideInInspector]
        public KUILocationManager locationManager;
        [HideInInspector]
        public KUIMappingManager mappingManager;
        [HideInInspector]
        public KUIDockingManager dockingManager;
        [HideInInspector]
        public KUILayoutManager layoutManager;
        [HideInInspector]
        public KUIMappingToLocationBinder mappingLocationBinder;

        [BoxGroup("Debug")]
        public bool LogEvents = false;
        [BoxGroup("Debug"), ReadOnly]
        public bool built = false;

        public virtual void Awake()
        {
            mappingManager = GetComponent<KUIMappingManager>();
            locationManager = GetComponent<KUILocationManager>();
            dockingManager = GetComponent<KUIDockingManager>();
            mappingLocationBinder = GetComponent<KUIMappingToLocationBinder>();
            layoutManager = GetComponent<KUILayoutManager>();

            if (LogEvents)
            {
                hub.Subscribe<BaseKUIEvent>(@event =>
                {
                    Debug.LogFormat("<color=red>KUIManager</color> hub [{0}]", @event);
                });
            }
        }

        public void NoteLayoutChanged()
        {
            mappingLocationBinder.AssignLayoutsAndBinders(mappingManager.GetActiveKUIElements(), new List<KUIElement>());
            hub.Publish(new LayoutChangedEvent());
            NonKeyEvents.Publish(new LayoutChangedEvent());
        }

        public void NoteMappingChanged(List<KUIElement> active, List<KUIElement> inactive)
        {
            mappingLocationBinder.AssignLayoutsAndBinders(active, inactive);
            hub.Publish(new MappingChangedEvent());
            NonKeyEvents.Publish(new MappingChangedEvent());

        }

        public void PublishKeyEvent(BaseKUIEvent @event)
        {
            if (@event.selectorTag != null)
            {
                // if a binder has this tag as part of it's location tags, it should receive the message
                List<KUILocation> locationsByTag = locationManager.locations.Where(x => x.LocationTagCollection.GetAllLocationTags().Any(tag => tag.AnyTagsMatch(@event.selectorTag))).ToList();

                if (locationsByTag.Count > 0)
                {
                    locationsByTag.ForEach(location => {
                        if (LogEvents) Debug.LogFormat("SelectivePublish <color=red>KUIManager</color> got event {0} with selector tag, routing event to location {1}", @event, location.gameObject.GetFullName());
                        location.hub.Publish(@event);
                    });
                }
                else
                {
                    if (LogEvents) Debug.LogErrorFormat("SelectivePublish <color=red>KUIManager</color> got event {0} with a valid selectorTag {1} but no location to map this to?", @event, @event.selectorTag);
                }
            }
            else
            {
                if (LogEvents) Debug.LogFormat("SelectivePublish <color=red>KUIManager</color> got event {0}, forwarding to all listeners", @event);
                hub.Publish(@event);
            }
        }

        #region Overlay visibility
        private bool overlayActive = true;


        public bool isOverlayActive()
        {
            return overlayActive;
        }

        public void SetOverlayActive(bool active)
        {
            overlayActive = active;
            hub.Publish(new OverlayActiveEvent(overlayActive));
            NonKeyEvents.Publish(new OverlayActiveEvent(overlayActive));

        }
        #endregion
    }
}
