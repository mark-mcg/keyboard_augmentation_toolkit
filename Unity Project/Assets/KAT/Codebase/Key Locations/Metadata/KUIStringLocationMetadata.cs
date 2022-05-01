using System.Collections.Generic;
using UnityEngine;

namespace KAT
{
    [RequireComponent(typeof(KUILocationDescriptor))]
    public class KUIStringLocationMetadata : KUILocationMetadata
    {
        public List<string> metadata;

        public override List<KUILocationTag> GetLocationTags()
        {
            return new List<KUILocationTag>() { new KUILocationTag(metadata.ToArray()) };
        }
    }
}
