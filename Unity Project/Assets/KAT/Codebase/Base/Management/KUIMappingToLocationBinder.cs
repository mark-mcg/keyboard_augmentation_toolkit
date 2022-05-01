using KAT.Interaction;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAT
{
    public class KUIMappingToLocationBinder : MonoBehaviour
    {
        [BoxGroup("Debug")]
        public bool LogEvents = false;

        [BoxGroup("Debug")]
        public List<KUIElement> AssignedElements, UnassignedElements;

        private KUILocationManager locationManager;
        private KUILayoutManager layoutManager;

        public virtual void Awake()
        {
            locationManager = GetComponent<KUILocationManager>();
            layoutManager = GetComponent<KUILayoutManager>();
        }

        /// <summary>
        /// Binds Elements, to KUIElementBinders, to KUILocationTagCollection locations in our keyboard layout
        /// </summary>
        /// <param name="toAssign"></param>
        /// <param name="toRemove"></param>
        public void AssignLayoutsAndBinders(List<KUIElement> toAssign, List<KUIElement> toRemove)
        {
            // Get the locations associated with the current layout - these are what we will attempt to bind to
            if (layoutManager.HasActiveLayout())
            {
                KUILocationDescriptors currentLayoutTags = layoutManager.GetCurrentLayoutElementLocationCollection().GetLocationDescriptors();
                List<KUIElement> unassignedElements = AssignElements(toAssign, currentLayoutTags);
            }
            RemoveElementsFromLocations(toRemove);
        }

        private void RemoveElementsFromLocations(List<KUIElement> toRemove)
        {
            if (LogEvents) Debug.Log("KUIMananger <b> AssignLayoutsAndBinders </b> ");
            foreach (KUIElement element in toRemove)
            {
                List<KUILocationDescriptor> locationsBinderWasAddedTo = new List<KUILocationDescriptor>();
                List<KUILocation> locationsAddedTo = new List<KUILocation>();
                locationsAddedTo.AddRange(element.LocationsAddedTo);
                locationsAddedTo.ForEach(x =>
                {
                    x.RemoveElement(element);
                    if (!locationsBinderWasAddedTo.Contains(x.LayoutLocation))
                        locationsBinderWasAddedTo.Add(x.LayoutLocation);
                });

                // issue here - if the binder isn't in use anymore, and the location we made in the layout was a composite, then it shouldn't be visible any longer
                // for composite locations, disable them if there are no layouts assigned
                locationsBinderWasAddedTo.ForEach(x =>
                {
                    if (x.GetAllLocationTags().Count > 1 && x.GetComponentsInChildren<KUILocation>().Sum(y => y.AddedElements.Count()) == 0)
                        x.gameObject.SetActive(false);
                });
            }
        }

        private List<KUIElement> AssignElements(List<KUIElement> toAssign, KUILocationDescriptors currentLayoutTags)
        {
            UnassignedElements.Clear();
            AssignedElements.Clear();
            if (LogEvents) Debug.Log("KUIMappingToLocationBinder ------------ AssignElements --------------");

            foreach (KUIElement element in toAssign)
            {
                KUILocationDescriptor matchedLocation = currentLayoutTags.GetMatchingDescriptor(layoutManager, element.LocationTagCollection);
                if (matchedLocation != null)
                {
                    AssignElementUsingLocationTags(element, matchedLocation, IsCompositeElement(element, currentLayoutTags));
                }
                else
                {
                    UnassignedElements.Add(element);
                    if (LogEvents) Debug.LogError("Could not find location matching tags " + string.Join(",", element.LocationTagCollection) + " for KUIElement " + element);
                }
            }
            if (LogEvents) Debug.Log("KUIMappingToLocationBinder ------------ finished AssignElements --------------");

            return UnassignedElements;
        }

        private bool IsCompositeElement(KUIElement element, KUILocationDescriptors currentLayoutTags)
        {
            //Debug.Log("Tag count is " + currentLayoutTags.GetAnyMatchingDescriptors(element.LocationTagCollection).Count);
            return currentLayoutTags.GetAnyMatchingDescriptors(element.LocationTagCollection).Count > 1;
        }

        private void AssignElementUsingLocationTags(KUIElement element, KUILocationDescriptor matchedLocation, bool isCompositeElement)
        {
            if (LogEvents) Debug.LogFormat("AssignElementUsingLocationTags attempting to assign {0} to matchedLocation {1} isCompositeElement {2}",
                element, matchedLocation, isCompositeElement);

            KUILocation binderForLocation = locationManager.GetLocation(element.LocationTagCollection);

            if (binderForLocation != null)
            {
                // we now have our collection of a location on the keyboard layout, a binder, and a kuielement
                if (LogEvents) Debug.Log("KUIManager: Assigning, <b> location </b> " + matchedLocation.gameObject + " <b> binder </b> " + binderForLocation + " <b> element </b> " + element);

                // first, unassign the element from any existing binding
                List<KUILocation> locationsPresentAt = new List<KUILocation>();
                locationsPresentAt.AddRange(element.LocationsAddedTo);
                locationsPresentAt.ForEach(binder =>
                {
                    if (binder != binderForLocation)
                        binder.RemoveElement(element);
                });

                // then assign to our new binding+location
                matchedLocation.gameObject.SetActive(true);
                binderForLocation.SetLayoutLocation(matchedLocation);
                binderForLocation.AddElement(element);
                AssignedElements.Add(element);

                if (isCompositeElement)
                {
                    // disable depression on composites
                    DepressionEventProvider depressionEventProvider = binderForLocation.GetComponentInChildren<DepressionEventProvider>();
                    if (depressionEventProvider != null)
                    {
                        depressionEventProvider.depressionEnabled = false;
                    }
                }

            }
            else
            {
                if (LogEvents) Debug.LogError("Couldn't retrieve/create a binder?");
            }
        }
    }
}
