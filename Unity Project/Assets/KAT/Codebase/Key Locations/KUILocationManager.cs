using KAT;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAT
{
    /// <summary>
    /// Locations are basically containers for KUIElements that are identified by their attached tags.
    /// 
    /// This allows for the virtual location of the "a" key for example to be entirely seperate from
    /// whether the a key exists in the current layout.
    /// </summary>
    public class KUILocationManager : MonoBehaviour
    {
        [BoxGroup("Debug")]
        public bool LogEvents = false;

        [BoxGroup("Debug")]
        [ReadOnly]
        public List<KUILocation> locations = new List<KUILocation>();
        [ReadOnly]
        [BoxGroup("Debug")]
        public GameObject LocationStore;
        private KUIManager kuiManager;
        public void Awake()
        {
            kuiManager = FindObjectOfType<KUIManager>();
            LocationStore = new GameObject("Location Store");
            LocationStore.transform.SetParent(this.transform);
            locations.Clear();
        }

        public KUILocation GetLocation(KUILocationDescriptor location)
        {
            List<KUILocation> potentialBinders = GetPossibleLocationsForTags(location);
            if (LogEvents) Debug.Log("Got " + potentialBinders.Count + " matching binders " + (potentialBinders.Count == 0 ? " <b> creating new binder </b>" : " using existing binder " + potentialBinders[0]));
            if (potentialBinders.Count == 0)
            {
                KUILocation newBinder = CreateBinder(location);//element.LocationTagCollection);
                newBinder.SetKUIManager(kuiManager);
                return newBinder;
            }
            else
            {
                return potentialBinders[0];
            }
        }

        public List<KUILocation> GetPossibleLocationsForTags(KUILocationDescriptor expectedTags)
        {
            //Debug.Log("<b>Searching binders</b>, total binders " + binders.Count());
            return locations.Where(x => x.LocationTagCollection.MatchAllTags(expectedTags)).ToList();
        }

        public KUILocation CreateBinder(params KUILocationDescriptor[] forLocation)
        {
            GameObject containerRoot = new GameObject("Location for target(s): " + forLocation);
            containerRoot.transform.SetParent(LocationStore.transform);
            KUILocationDescriptor locationTagCollection = containerRoot.AddComponent<KUILocationDescriptor>();

            forLocation.ToList().ForEach(x =>
            {
                List<KUILocationTag> tags = x.GetAllLocationTags();
                //Debug.Log("Adding " + tags.Count() + " tags from " + x.gameObject);
                locationTagCollection.AddRuntimeTags(tags, true);
            });
            KUILocation container = containerRoot.gameObject.AddComponent<KUILocation>();
            locations.Add(container);
            return container;
        }
    }
}
