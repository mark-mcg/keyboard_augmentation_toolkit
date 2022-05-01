using MiscUtil.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput.Native;
using System.Linq;

namespace KAT
{
    [Serializable]
    public class KUILocationTag
    {
        public List<string> TargetTags = new List<string>();

        public virtual bool HasMultipleTags()
        {
            return TargetTags.Count > 1;
        }

        public KUILocationTag(params string[] tag)
        {
            this.TargetTags.AddRange(tag);
        }

        public virtual KUILocationTag Copy()
        {
            return new KUILocationTag(this.TargetTags.ToArray());
        }

        public virtual bool AllTagsMatch(KUILocationTag tags)
        {
            //Debug.Log("AllTagsMatch comparing " + this + " with " + tags);
            return TargetTags.All(x => tags.TargetTags.Contains(x));// && TargetTags.Count == tags.TargetTags.Count;

        }

        public virtual bool AnyTagsMatch(KUILocationTag tags)
        {
            //Debug.Log("AnyTagsMatch comparing " + this + " with " + tags);
            return TargetTags.Any(x => tags.TargetTags.Contains(x));// && TargetTags.Count == tags.TargetTags.Count;
        }

        public override bool Equals(object other)
        {
            if (other is KUILocationTag castTag)
                return AllTagsMatch(castTag);
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return TargetTags.Sum(x => x.GetHashCode());
        }

        public override string ToString()
        {
            return "Tags: " + string.Join(",", TargetTags);
        }
    }
}

