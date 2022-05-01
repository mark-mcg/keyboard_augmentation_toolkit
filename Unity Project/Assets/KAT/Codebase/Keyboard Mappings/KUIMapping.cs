using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAT
{
    public class KUIMapping : MonoBehaviour
    {
        public KUIMapping parentCollection, previousCollection, nextCollection;
        public List<KUIMapping> childCollections;

        public List<KUIElement> kuiElements = new List<KUIElement>();

        public void Setup()
        {
            RefreshCollection();
        }

        public KUIMapping GetRoot()
        {
            if (parentCollection != null)
                return parentCollection.GetRoot();
            else
                return this;
        }

        public int GetDepth()
        {
            if (parentCollection != null)
                return parentCollection.GetDepth() + 1;
            else
                return 1;
        }

        public void RefreshCollection()
        {
            kuiElements.Clear();
            kuiElements.AddRange(this.GetComponentsInImmediateChildren<KUIElement>().ToList());
            SetAncestorsAndAntecedants();
        }

        public void SetAncestorsAndAntecedants()
        {
            Debug.Log("SetAncestorsAndAntecedants for " + this.gameObject);
            bool first = true;

            foreach (KUIMapping ancestor in this.transform.GetComponentsInParent<KUIMapping>())
            {
                if (ancestor != this)
                {
                    if (first)
                    {
                        parentCollection = ancestor;
                        first = false;
                    }
                }
            }

            if (this.parentCollection != null)
            {
                List<KUIMapping> siblings = this.parentCollection.transform.GetComponentsInChildren<KUIMapping>().ToList();
                siblings = siblings.Where(x => x.transform.parent == this.transform.parent).ToList();

                LinkedList<KUIMapping> collection = new LinkedList<KUIMapping>();
                siblings.ForEach(x => collection.AddLast(x));

                for (LinkedListNode<KUIMapping> node = collection.First; node != null; node = node.Next)
                {
                    Debug.Log("Setting siblings for " + node.Value);
                    node.Value.nextCollection = node.Next != null ? node.Next.Value : node.List.First.Value;
                    node.Value.previousCollection = node.Previous != null ? node.Previous.Value : node.List.Last.Value;
                }               
            }

            this.childCollections = GetComponentsInChildren<KUIMapping>().ToList();
            this.childCollections.Remove(this);
        }

        public List<KUIElement> GetElements(bool refresh = false, bool recursiveParents = false)
        {
            if (refresh)
                RefreshCollection();

            if (recursiveParents && parentCollection != null)
            {
                List<KUIElement> elements = new List<KUIElement>();
                elements.AddRange(kuiElements);
                elements.AddRange(parentCollection.GetElements(refresh, recursiveParents));
                return elements;
            }
            return kuiElements;
        }
    }
}