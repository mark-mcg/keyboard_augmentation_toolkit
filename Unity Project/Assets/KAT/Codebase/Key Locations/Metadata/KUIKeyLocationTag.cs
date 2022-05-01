using KAT.KeyCodeMappings;
using MiscUtil.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAT
{
    [Serializable]
    public class KUIKeyLocationTag : KUILocationTag
    {
        public List<KKey> keys;

        public override bool HasMultipleTags()
        {
            return keys.Count > 1;
        }

        public KUIKeyLocationTag(params KKey[] key) : base("")
        {
            this.keys = new List<KKey>(key);
        }

        public override bool AllTagsMatch(KUILocationTag tag)
        {
            //Debug.Log("AllTagsMatch for KUIKeyLocationTag");

            if (tag is KUIKeyLocationTag castTarget)
            {
                bool result = this.keys.TrueForAll(x => castTarget.keys.Contains(x));// && this.keys.Count == castTarget.keys.Count;
                //Debug.Log("checking tags " + tag + " against " + this + " result " + result);
                return result;
            }
            return base.AnyTagsMatch(tag);
        }


        public override bool AnyTagsMatch(KUILocationTag tag)
        {
            if (tag is KUIKeyLocationTag castTarget)
            {
                bool result =  this.keys.Any(x => castTarget.keys.Contains(x));// && this.keys.Count == castTarget.keys.Count;
                return result;
            }
            return base.AnyTagsMatch(tag);
        }

        public override string ToString()
        {
            return "KeyTag {" + string.Join(",", keys) + "}";
        }

        public override KUILocationTag Copy()
        {
            return new KUIKeyLocationTag(keys.ToArray());
        }

        public override int GetHashCode()
        {
            return keys.Sum(x => x.pos.GetHashCode());
        }
    }
}

