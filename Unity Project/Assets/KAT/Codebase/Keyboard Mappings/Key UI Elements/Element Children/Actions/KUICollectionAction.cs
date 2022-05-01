using KAT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAT
{
    public enum CollectionActionType { SpecificMap, Back, Next, Previous, Toggle }

    public class KUICollectionAction : KUIBinaryElementAction
    {
        public CollectionActionType action = CollectionActionType.SpecificMap;
        public KUIMapping SpecificMapToActivate;
        private KUIManager manager;
        private KUIMappingManager collectionManager;
        private KUIMapping Parent;
        [Header("Automatically deactivate this mapping after the specified duration (seconds).")]
        public float Duration;
        private Coroutine triggerInactiveAfterDuration;
        public bool ClearMappingsOnActivation = false;

        public void Awake()
        {
            manager = FindObjectOfType<KUIManager>();
            collectionManager = FindObjectOfType<KUIMappingManager>();
        }

        public void Start()
        {
            Parent = GetComponentInParent<KUIMapping>();

            if (Parent != null)
            {
                // Check if we can support the requested action
                switch (action)
                {
                    case CollectionActionType.Back:
                        if (Parent == null || Parent.parentCollection == null)
                            this.gameObject.SetActive(false);
                        break;
                    case CollectionActionType.Next:
                        if (Parent == null || Parent.nextCollection == null)
                            this.gameObject.SetActive(false);
                        break;
                    case CollectionActionType.Previous:
                        if (Parent == null || Parent.previousCollection == null)
                            this.gameObject.SetActive(false);
                        break;
                }
            }
        }

        public override void PerformAction()
        {
            base.PerformAction();
            switch (action)
            {
                case CollectionActionType.Toggle:
                    if (collectionManager.IsMappingActiveInHierarchy(SpecificMapToActivate))
                    {
                        Debug.Log("Toggle Mapping Event, going back to Parent mapping " + Parent);
                        collectionManager.ActivateMapping(Parent, ClearMappingsOnActivation);
                    }
                    else
                    {
                        Debug.Log("Toggle Mapping Event, activating specific map " + SpecificMapToActivate);
                        collectionManager.ActivateMapping(SpecificMapToActivate, ClearMappingsOnActivation);
                    }
                    break;

                case CollectionActionType.SpecificMap:
                    collectionManager.ActivateMapping(SpecificMapToActivate, ClearMappingsOnActivation);
                    break;
                case CollectionActionType.Back:
                    if (Parent != null)
                        collectionManager.ActivateMapping(Parent.parentCollection, ClearMappingsOnActivation);
                    break;
                case CollectionActionType.Next:
                    if (Parent != null)
                        collectionManager.ActivateMapping(Parent.nextCollection, ClearMappingsOnActivation);
                    break;
                case CollectionActionType.Previous:
                    if (Parent != null)
                        collectionManager.ActivateMapping(Parent.previousCollection, ClearMappingsOnActivation);
                    break;
            }
        }
    }
}
