using KAT.Layouts;
using MiscUtil.Collections.Extensions;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using KAT.Interaction;
using PubSub;

namespace KAT
{
    public class KUIElement : KUIParentChildEventProvider
    {
        public Hub hub = new Hub();
        private static bool LogEvents = true;

        /// <summary>
        /// When assigned to a location, can this element co-exist with other active elements?
        /// </summary>
        public enum AssignedLocationOwnership { ExclusiveByContainer, ExclusiveByTrigger, NoRestrictions }
        public AssignedLocationOwnership ownershipMode = AssignedLocationOwnership.NoRestrictions;

        public bool showDebug = true;
        public enum VisibilityState { Invisible, Visible }

        [NaughtyAttributes.ReadOnly, BoxGroup("KUIElement")]
        public VisibilityState visibilityState = VisibilityState.Invisible;

        [HideInInspector]
        public float TimeAdded = -1;

        [NaughtyAttributes.ReadOnly, BoxGroup("KUIElement")]
        public KUIMapping MappingParent;

        [NaughtyAttributes.ReadOnly, BoxGroup("KUIElement")]
        public List<KUILocation> LocationsAddedTo = new List<KUILocation>();

        [NaughtyAttributes.ReadOnly, BoxGroup("KUIElement")]
        public KUILocationDescriptor LocationTagCollection;

        [NaughtyAttributes.ReadOnly,BoxGroup("KUIElement")]
        public List<KUIEvents.KUIElementDepedent> ElementDependents = new List<KUIEvents.KUIElementDepedent>();

        public KUIElementBaseTrigger Trigger;
        public int depth { get { return MappingParent.GetDepth(); } }

        public List<KUIInteractionProvider> LocalInteractionProviders = new List<KUIInteractionProvider>();

        public void Awake()
        {
            MappingParent = GetComponentInParent<KUIMapping>();
            if (MappingParent == null)
                Debug.LogError("Got a KUIelement that doesn't belong to a mapping collection?");

            LocationTagCollection = GetComponent<KUILocationDescriptor>();
            if (LocationTagCollection == null)
                this.gameObject.AddComponent<KUILocationDescriptor>();

            ElementDependents.AddRange(transform.GetComponentsInChildren<KUIEvents.KUIElementDepedent>());
            //foreach (Transform t in transform)
            //{
            //    ElementDependents.AddRange(t.GetComponentsInChildren<KUIEvents.KUIElementDepedent>());
            //}
            ElementDependents.ForEach(x => x.SetKUIParent(this));

            LocalInteractionProviders = GetComponentsInChildren<KUIInteractionProvider>().ToList();


            if (LogEvents)
            {
                hub.Subscribe<BaseKUIEvent>(@event =>
                {
                    Debug.LogFormat("<color=green>KUIElement</color> GO:{0} Class:{1} event:{2} eventtype:{3}", gameObject.name, this.GetType().Name, @event, @event.GetType().Name);
                });
            }
        }


        #region Location

        public bool IsActive()
        {
            if (LocationsAddedTo.Count == 0)
                return false;
            else
                return LocationsAddedTo.All(x => x.IsElementActiveAtThisLocation(this));
        }

        public bool IsVisible()
        {
            return IsActive() && visibilityState == VisibilityState.Visible;
        }

        public virtual void AddParentLocations(params KUILocation[] newParents)
        {
            bool added = false;
            foreach (KUILocation container in newParents)
            {
                if (!LocationsAddedTo.Contains(container))
                {
                    LocationsAddedTo.Add(container);
                    added = true;
                    container.hub.Subscribe<BaseKUIEvent>(@event =>
                    {
                        hub.Publish(@event);
                    });
                }
            }

            RefreshInteractionAreas();
            if (added)
            {
                hub.Publish(new LocationChangedEvent(this, LocationsAddedTo, new List<KUILocation>()));
            }
        }

        public virtual void RemoveParentLocations(params KUILocation[] newParents)
        {
            LocationChangedEvent @event = new LocationChangedEvent(this, LocationsAddedTo.Where(x => !newParents.Contains(x)).ToList(), LocationsAddedTo.Where(x => newParents.Contains(x)).ToList());
            bool removed = false;
            foreach (KUILocation container in newParents)
            {
                if (LocationsAddedTo.Contains(container))
                {
                    LocationsAddedTo.Remove(container);
                    removed = true;
                    container.hub.Unsubscribe<BaseKUIEvent>();
                }
            }

            RefreshInteractionAreas();
            if (removed)
            {
                hub.Publish(@event);
            }
        }

        public List<KUILocationInteractionRegions> assignedInteractionAreas = new List<KUILocationInteractionRegions>();

        public virtual KUILocationInteractionRegions GetPrimaryInteractionRegion()
        {
            if (assignedInteractionAreas.Count > 0)
                return assignedInteractionAreas[0];
            else return null;
        }

        public virtual List<KUILocationInteractionRegions> GetInteractionAreas()
        {
            return assignedInteractionAreas;
        }

        private void RefreshInteractionAreas()
        {
            assignedInteractionAreas.Clear();
            assignedInteractionAreas.AddRange(LocationsAddedTo.Select(x => x.InteractionRegions));
            KUILocationInteractionRegions primaryRegion = assignedInteractionAreas.FirstOrDefault();
            LocalInteractionProviders.ForEach(x => x.SetLocationInteractionRegion(primaryRegion));
        }
        #endregion

        #region Active state handling

        public void SetVisibilityState(VisibilityState visState, bool forceEvent = false)
        {
            if (visibilityState != visState || forceEvent)
            {
                Debug.Log("Setting visibility state to " + visState);
                visibilityState = visState;
                hub.Publish(new VisibilityStateEvent(this));
                //LocationsAddedTo.ForEach(x => x.hub.Publish(new VisibilityStateEvent(this)));
            }
        }

        public VisibilityState GetVisibilityState()
        {
            return visibilityState;
        }
        #endregion
    }

}
