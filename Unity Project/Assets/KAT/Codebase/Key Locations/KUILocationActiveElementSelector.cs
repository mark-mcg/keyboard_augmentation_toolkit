using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAT
{
    public class KUILocationActiveElementSelector
    {
        public enum ElementActiveMode { AllActive, SingleHighestPriorityActive }
        public ElementActiveMode elementActiveMode = ElementActiveMode.SingleHighestPriorityActive;

        public List<KUIElement> SelectActiveElements(KUILocation location, List<KUIElement> added, List<KUIElement> active)
        {
            if (elementActiveMode == ElementActiveMode.SingleHighestPriorityActive)
            {
                return SelectActiveKUIElements(location, added, active);
            } else
            {
                return added;
            }
        }

        public List<KUIElement> lastPriorityOrder = new List<KUIElement>();

        private KUIElement GetHighestPriorityVisibleShortcut(List<KUIElement> elements)
        {
            if (elements.Count > 0)
            {
                var sorted = elements.OrderBy(x => x.depth).ThenBy(x => x.GetVisibilityState());
                lastPriorityOrder.Clear();
                lastPriorityOrder.AddRange(sorted);
                if (sorted.Count() > 0 && sorted.Last().GetVisibilityState() == KUIElement.VisibilityState.Visible)
                    return sorted.Last();
            }
            return null;
        }

        public bool HasExclusiveByContainer(List<KUIElement> list)
        {
            if (list == null || list.Count == 0)
                return false;
            return list.Any(x => x.ownershipMode == KUIElement.AssignedLocationOwnership.ExclusiveByContainer);
        }

        public bool HasExclusiveByTrigger(List<KUIElement> list, KUIElement toCompare)
        {
            if (list == null || list.Count == 0)
                return false;
            return list.Any(x => x.ownershipMode == KUIElement.AssignedLocationOwnership.ExclusiveByTrigger && x.Trigger.Equals(toCompare.Trigger));
        }
        private List<KUIElement> SelectActiveKUIElements(KUILocation location, List<KUIElement> added, List<KUIElement> previousActive)
        {
            List<KUIElement> nowActive = new List<KUIElement>();

            if (added.Count > 0)
            {
                var sorted = added.OrderBy(x => x.depth).ThenBy(x => x.transform.GetSiblingIndex());

                foreach (KUIElement element in sorted)
                {
                    // we can add this if we don't already have an exclusive by container element added,
                    // and we don't have an exclusive by trigger added that shares a trigger type
                    if (!HasExclusiveByContainer(nowActive) && !HasExclusiveByTrigger(nowActive, element))
                        nowActive.Add(element);
                }

                List<KUIElement> newlyActive = nowActive.Where(x => previousActive.Contains(x)).ToList();
                List<KUIElement> newlyInactive = previousActive.Where(x => !nowActive.Contains(x)).ToList();
            }
            return nowActive;
        }

    }
}