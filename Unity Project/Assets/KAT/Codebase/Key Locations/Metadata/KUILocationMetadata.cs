using KAT;
using MiscUtil.Collections.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAT
{

    [RequireComponent(typeof(KUILocationDescriptor))]
    public class KUILocationMetadata : MonoBehaviour
    {
        public virtual List<KUILocationTag> GetLocationTags()
        {
            return new List<KUILocationTag>();
        }
    }
}
