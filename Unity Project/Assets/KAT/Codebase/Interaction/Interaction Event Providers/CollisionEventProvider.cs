using PubSub;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KAT.Interaction
{
    public interface ITouchProvider { }
    public class CollisionDescriptor
    {
        public const float MAX_BUFFER_SECONDS = 1.0f;

        private Transform relativeTo;
        public CollisionEventProvider provider;

        public CollisionDescriptor(Vector3 localPoint, Vector3 worldPoint, Collider collider, Transform relativeTo, CollisionEventProvider provider)
        {
            this.relativeTo = relativeTo;
            this.provider = provider;
            AddPosition(localPoint, worldPoint, collider);
        }

        public void AddPosition(Vector3 localPoint, Vector3 worldPoint, Collider collider)
        {
            CollisionPositions.Add(new Tuple<Vector3, Vector3, float>(localPoint, worldPoint, Time.time));// (relativeTo.InverseTransformDirection(collider.transform.position), Time.time));
            CollisionPositions.RemoveAll(x => Time.time - x.Item3 > MAX_BUFFER_SECONDS);
        }

        public List<Tuple<Vector3, Vector3, float>> CollisionPositions = new List<Tuple<Vector3, Vector3, float>>();
    }

    public enum CollisionType { Enter, Continue, Exit }

    /// <summary>
    /// Keys are oriented so Z is forward, so we get touches in x/y relative to this
    /// </summary>
    public class CollisionEventProvider : KUIInteractionProvider
    {
        public string collisionTag = "Default";


        public Dictionary<Collider, CollisionDescriptor> CurrentTouches = new Dictionary<Collider, CollisionDescriptor>();
        //public bool UseCollisions = true;
        //public bool UseTriggers = false;

        private Collider ourCollider;
        public void Awake()
        {
            ourCollider = GetComponent<Collider>();
        }

        public Collider GetCollider()
        {
            return ourCollider;
        }

        public virtual bool ValidateCollider(Collider collider)
        {
            return (collider.GetComponents<XRDirectInteractor>().Count() > 0 || collider.GetComponentsInChildren<ITouchProvider>().Count() > 0);
        }

        protected void NewCollision(Collider collider, CollisionType type)
        {
            if (ValidateCollider(collider))
            {
                Vector3 pointInLocalSpace = transform.InverseTransformPoint(collider.transform.position);

                if (!CurrentTouches.ContainsKey(collider))
                {
                    CurrentTouches.Add(collider, new CollisionDescriptor(pointInLocalSpace, collider.transform.position, collider, transform, this));
                } else
                {
                    CurrentTouches[collider].AddPosition(pointInLocalSpace, collider.transform.position, collider);
                }

                CollisionEvent @event = new CollisionEvent(this, collider, ourCollider, CurrentTouches[collider], type, collisionTag);
                hub.Publish(@event);

                if (type == CollisionType.Exit)
                {
                    CurrentTouches[collider] = null;
                    CurrentTouches.Remove(collider);
                }
            } else
            {
                // Debug.Log("Collision being ignored as validation failed for collider " + collider.gameObject);
            }
        }

        //public virtual void OnCollisionEnter(Collision collision)
        //{
        //    NewCollision(collision.collider, CollisionType.Enter);
        //}

        //public virtual void OnCollisionExit(Collision collision)
        //{
        //    NewCollision(collision.collider, CollisionType.Exit);

        //}

        //public virtual void OnCollisionStay(Collision collision)
        //{
        //    NewCollision(collision.collider, CollisionType.Continue);

        //}

        public virtual void OnTriggerEnter(Collider collider)
        {
            NewCollision(collider, CollisionType.Enter);
        }


        public virtual void OnTriggerExit(Collider collider)
        {
            NewCollision(collider, CollisionType.Exit);
        }


        /// <summary>
        /// For things like UI element updates based on continuous movements...
        /// </summary>
        /// <param name="enabled"></param>
        public void SetOnStayUpdates(bool enabled)
        {
            OnStayUpdatesEnabled = enabled;
        }
        public bool OnStayUpdatesEnabled = false;

        public virtual void OnTriggerStay(Collider collider)
        {
            if (OnStayUpdatesEnabled)
            {
                NewCollision(collider, CollisionType.Continue);
            }
        }
    }
}