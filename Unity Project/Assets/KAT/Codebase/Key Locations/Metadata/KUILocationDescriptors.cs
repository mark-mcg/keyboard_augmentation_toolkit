using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAT
{
    public class KUILocationDescriptors : List<KUILocationDescriptor>
    {
        public KUILocationDescriptors(List<KUILocationDescriptor> descriptors)
        {
            if (descriptors != null && descriptors.Count > 0)
                this.AddRange(descriptors);
        }

        public KUILocationDescriptors()
        {
        }

        public KUILocationDescriptor GetMatchingDescriptor(KUILayoutManager layoutManager, KUILocationDescriptor descriptor)
        {
            Debug.Log("Checking descriptor " + descriptor);
            KUILocationDescriptor matchedLocation = null;

            if (descriptor == null)
            {
                Debug.LogError("Can't match descriptor " + descriptor);
            }
            else
            {
                // These are the locations that directly match all our requested tags
                matchedLocation = this.FirstOrDefault(x => descriptor.MatchAllTags(x));

                // if there are no locations that directly match, check for a composite element that spans multiple locations
                if (matchedLocation == null)
                {
                    Debug.LogError("Didn't find matching location for layout, potential multi-element? " + this.Count());
                    KUILocationDescriptors anyMatchingTags = GetAnyMatchingDescriptors(descriptor);
                    if (anyMatchingTags.Count > 0)
                    {
                        // make a new composite location
                        matchedLocation = layoutManager.GetCurrentLayoutElementLocationCollection().CreateCompositeLocationDescriptor(anyMatchingTags, descriptor);
                    }
                }
            }
            return matchedLocation;
        }

        public KUILocationDescriptors GetAnyMatchingDescriptors(KUILocationDescriptor descriptor)
        {
            return new KUILocationDescriptors(this.Where(x => descriptor.MatchAnyTags(x)).ToList());
        }
    }
}
