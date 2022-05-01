using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAT
{
    /// <summary>
    /// This manages the binding of UIElement (e.g. augmentations) collections to the available KUIElementContainers (e.g. keys)
    /// </summary>
    //[RequireComponent(typeof(KUIElementBinderAssigner))]
    public class KUIMappingManager : MonoBehaviour
    {
        [BoxGroup("Debug")]
        public List<KUIMapping> ActiveMappings = new List<KUIMapping>();
        protected List<KUIElement> CurrentKUIElements = new List<KUIElement>();
        private KUIManager kuiManager;

        public virtual void Awake()
        {
            kuiManager = GetComponent<KUIManager>();
        }

        /// <summary>
        /// Activates the new mapping after a slight delay. This is to allow the KeyboardHook to finish processing
        /// before we start swapping KUIElements out (otherwise we can end up with the KeyboardHook thinking a 
        /// keypress wasn't consumed/suppressed).
        /// </summary>
        /// <param name="mappingGroup"></param>
        public void ActivateMapping(KUIMapping mappingGroup, bool clearMappingsOnAdd = false)
        {
            Debug.Log("ActivateMappingNextFrame called for KUIElementCollection " + mappingGroup);
            StartCoroutine(ActivateMappingInternal(mappingGroup, clearMappingsOnAdd));
        }

        public void ClearMappings()
        {
            Debug.Log("Clearing all mappings ");
            ActiveMappings.Clear();
            RefreshActiveKUIElements();
        }

        public void DeactivateMapping(KUIMapping mappingGroup)
        {
            Debug.Log("Removing mapping " + mappingGroup);
            ActiveMappings.Remove(mappingGroup);
            RefreshActiveKUIElements();
        }

        public List<KUIElement> GetActiveKUIElements()
        {
            List<KUIElement> AllKUIElementsToBeAssigned = new List<KUIElement>();

            foreach (KUIMapping mappingGroup in GetActiveMappings())
            {
                FetchKUIElements(mappingGroup, AllKUIElementsToBeAssigned);
            }

            return AllKUIElementsToBeAssigned;
        }


        #region Internal
        /// <summary>
        /// Controls the logic of activating mappings and their associated KUIElements.
        /// </summary>
        /// <param name="mappingGroupToActivate"></param>
        /// <returns></returns>
        private IEnumerator ActivateMappingInternal(KUIMapping mappingGroupToActivate, bool clearMappingsOnAdd = false)
        {
            yield return new WaitForSeconds(0.01f);

            Debug.Log("ActivateMapping called with " + mappingGroupToActivate);

            if (clearMappingsOnAdd)
                ActiveMappings.Clear();    

            if (!ActiveMappings.Contains(mappingGroupToActivate))
                AddMapping(mappingGroupToActivate);

            RefreshActiveKUIElements();
        }

        private void AddMapping(KUIMapping mapping)
        {
            if (mapping != null && !ActiveMappings.Contains(mapping))
            {
                Debug.Log("Adding mapping " + mapping);
                ActiveMappings.Add(mapping);
            }
        }


        /// <summary>
        /// This will look at all the active mappings and determine which KUIElements to activate based on
        /// where the KUIElements come in the hierarchy, and what KUIElementCollectionCategory the mapping is.

        /// </summary>
        protected void RefreshActiveKUIElements()
        {
            // Figure out which KUIElements should be removed entirely
            Debug.Log("Removing KUIElements that should no longer be active");
            List<KUIElement> ToRemove = new List<KUIElement>();
            List<KUIElement> AllKUIElementsToBeAssigned = new List<KUIElement>();

            foreach (KUIMapping mappingGroup in GetActiveMappings())
            {
                FetchKUIElements(mappingGroup, AllKUIElementsToBeAssigned);
            }

            ToRemove.AddRange(CurrentKUIElements.Where(x => !AllKUIElementsToBeAssigned.Contains(x)));
            CurrentKUIElements.Clear();
            CurrentKUIElements.AddRange(AllKUIElementsToBeAssigned);
            kuiManager.NoteMappingChanged(CurrentKUIElements, ToRemove);
            //kuiManager.AssignLayoutsAndBinders(CurrentKUIElements, ToRemove);

            ////OutputKUIElementList(AllKUIElementsToBeAssigned, "ToBeAssigned");
            ////OutputKUIElementList(ToRemove, "ToBeRemoved");
            //KUIElementAssigner.RemoveElements(ToRemove);

            //// Add the new KUIElements (some may be re-added here but we don't really care)
            //// iterate in order of category types assigning based on AssignmentAlgorithm
            //Debug.Log("Assigning active KUIElements");

            //List<KUIElement> KUIElementsToBeAssigned = new List<KUIElement>();
            //foreach (KUIElementCollection mappingGroup in GetActiveMappings())
            //{
            //    FetchKUIElements(mappingGroup, KUIElementsToBeAssigned);
            //}

            //Debug.Log("Got " + KUIElementsToBeAssigned.Count + " to be assigned");
            //KUIElementAssigner.AssignElements(KUIElementsToBeAssigned, true, true);

            ////Debug.Log("MappingManager: " + ToRemove.Count + " KUIElements removed");
            ////Debug.Log("MappingManager: " + AllKUIElementsToBeAssigned.Count + " KUIElements that we attempted to activate");
            ////Debug.Log("MappingManager: " + OrphanedKUIElements.Count + " orphaned");
            ////Debug.Log("MappingManager: " + (AllKUIElementsToBeAssigned.Count - OrphanedKUIElements.Count) + " KUIElements activated");


        }

        #region Helpers
        private void OutputKUIElementList(List<KUIElement> KUIElements, string prefix)
        {
            foreach (KUIElement KUIElement in KUIElements)
            {
                Debug.Log(prefix + ": " + KUIElement);
            }
        }

        public List<KUIMapping> GetActiveMappings()
        {
            return ActiveMappings;
        }

        public bool IsMappingActiveInHierarchy(KUIMapping map)
        {
            foreach (KUIMapping activemap in ActiveMappings)
            {
                if (IsMappingActiveInHierarchy(activemap, map, 5))
                    return true;
            }
            return false;
        }

        public bool IsMappingActiveInHierarchy(KUIMapping toSearch, KUIMapping shouldBeActive, int depth)
        {
            if (depth == 0)
                return false;

            depth--; // prevent any pesky infinite recursions by just bailing out early

            if (toSearch == null)
                return false;

            if (toSearch == shouldBeActive)
                return true;

            else
                return IsMappingActiveInHierarchy(toSearch.parentCollection, shouldBeActive, depth);
        }

        private void FetchKUIElements(KUIMapping group, List<KUIElement> list)
        {
            Debug.Log("FetchKUIElements for " + group);
            if (group == null)
                return;

            foreach (KUIElement s in group.GetElements(true, true))
            {
                if (!list.Contains(s))
                    list.Add(s);
            }
        }

        /// <summary>
        /// Get the mapping hierarchy for a given mapping, from leaf to parents.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private List<KUIMapping> GetAllMappings(KUIMapping group)
        {
            List<KUIMapping> mappings = new List<KUIMapping>();

            if (group != null)
            {
                mappings.Add(group);
                if (group.parentCollection != null)
                    mappings.AddRange(GetAllMappings(group.parentCollection));
            }

            return mappings;
        }
        #endregion
        #endregion
    }
}
