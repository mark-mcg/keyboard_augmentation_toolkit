using MiscUtil.Collections.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAT
{
    /// <summary>
    /// A collection of tags that describes a virtual key location. Note this can be
    /// multiple keys due to the creation of composite locations (e.g. treating "asd"
    /// as one key)
    /// </summary>
    public class KUILocationDescriptor : MonoBehaviour
    {
        [HideInInspector]
        protected List<KUILocationTag> RuntimeLocationTags = new List<KUILocationTag>();

        public bool ShouldSpanAllLocations = false;

        public void AddRuntimeTags(List<KUILocationTag> tags, bool clear = false)
        {
            RuntimeLocationTags.AddRange(tags);

            //if (clear)
            //    RuntimeLocationTags.Clear();

            //if (tags.Count > 0)
            //{
            //    tags.ForEach(x => RuntimeLocationTags.Remove(x));
            //}

        }

        private List<KUILocationTag> metadataTags;
        public List<KUILocationTag> GetAllLocationTags(bool forceMetadatRefresh = false)
        {
            if (metadataTags == null || forceMetadatRefresh)
            {
                metadataTags = new List<KUILocationTag>(GetLocationMetadata(forceMetadatRefresh).SelectMany(x => x.GetLocationTags()));
            }

            List<KUILocationTag> allTags = new List<KUILocationTag>();
            allTags.AddRange(metadataTags);
            allTags.AddRange(RuntimeLocationTags);

            return allTags;
        }

        private List<KUILocationMetadata> locationMetadata;
        public List<KUILocationMetadata> GetLocationMetadata(bool forceMetadatRefresh = false)
        {
            if (locationMetadata == null || forceMetadatRefresh)
                locationMetadata = GetComponents<KUILocationMetadata>().ToList();
            return locationMetadata;
        }

        public override bool Equals(object other)
        {
            if (other is KUILocationDescriptor castOther)
            {
                return MatchAllTags(castOther);
            }

            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool MatchAllTags(KUILocationDescriptor other)
        {
            //Debug.Log("MatchAllTags on KUILocationTagCollection, other tags count " + otherTags.Count);
            return this.GetAllLocationTags().TrueForAll(ourTag => other.GetAllLocationTags().Any(y => ourTag.AllTagsMatch(y)));
        }

        public bool MatchAllTags(params KUILocationTag[] theirTags)
        {
            //Debug.Log("MatchAllTags[] on KUILocationTagCollection, our tags count " + this.GetAllLocationTags().Count + " on " + this.gameObject);
            return this.GetAllLocationTags().TrueForAll(ourTag => theirTags.Any(y => ourTag.AllTagsMatch(y)));
        }

        public bool MatchAnyTags(KUILocationDescriptor other)
        {
            //Debug.Log("MatchAllTags on KUILocationTagCollection, other tags count " + otherTags.Count);
            return this.GetAllLocationTags().TrueForAll(ourTag => other.GetAllLocationTags().Any(y => ourTag.AnyTagsMatch(y)));
        }

        public bool MatchAnyTags(params KUILocationTag[] theirTags)
        {
            //Debug.Log("MatchAllTags[] on KUILocationTagCollection, our tags count " + this.GetAllLocationTags().Count + " on " + this.gameObject);
            return this.GetAllLocationTags().TrueForAll(ourTag => theirTags.Any(y => ourTag.AnyTagsMatch(y)));
        }

        public override string ToString()
        {
            return "KUILocationTagCollection for " + gameObject.name + " tag count " + GetAllLocationTags().Count() + (GetAllLocationTags().Count()> 0 ? " first tag " + GetAllLocationTags()[0] : "");
        }
    }
}
