using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KAT.Interaction
{
    public class DepressionEventProvider : CollisionBasedKUIInteractionProvider
    {
        KUILocationInteractionRegions interactionArea;
        KUIDockingManager dockingManager;
        KUIKeyLocationMetadata keyMetadata;
        Rigidbody rigidBody;
        [NonSerialized]
        public bool depressed;
        private float depressionAmount = 0.005f;
        KUIManager manager;
        public bool depressionEnabled = true;
        public Color depressionColor = Color.blue * 0.5f;
        public List<DepressionEventProvider> ChildDepressionEventProviders = new List<DepressionEventProvider>();

        public override void Awake()
        {
            this.autoAddCollisionProvider = true;
            base.Awake();

            manager = FindObjectOfType<KUIManager>();
            dockingManager = manager.dockingManager;
            interactionArea = GetComponentInParent<KUILocationInteractionRegions>();
            keyMetadata = GetComponentInParent<KUIKeyLocationMetadata>();
            rigidBody = GetComponent<Rigidbody>();

            if (interactionArea == null)
                Debug.LogError("DepressionEventProvider must be added to the KUIInteractionManager prefab");
        }

        public override void ProcessCollision(CollisionEvent collisionEvent)
        {
            SetDepressionState(collisionEvent.type == CollisionType.Enter || collisionEvent.type == CollisionType.Continue);
        }

        public override void LayoutChanged()
        {
            nonDepressedPosition = GetKeyTransform().transform.localPosition;
        }

        private Transform GetKeyTransform()
        {
            return interactionArea.GetLocation().LayoutLocation.transform;
        }

        private Coroutine changeRoutine;
        public void SetDepressionState(bool newState, float delay = 0.0f)
        {
            if (!depressed)
            {
                // update depression amount based on current layout - todo, have layout changes inform event providers!
                depressionAmount = manager.layoutManager.GetCurrentLayoutElementLocationCollection().TouchDepressionAmount;
            }
            if (changeRoutine != null)
                StopCoroutine(changeRoutine);

            if (this.isActiveAndEnabled)
                changeRoutine = StartCoroutine(SetDepressionStateInternal(newState, delay));
            else
                SetDepressionStateImmediate(newState);
        }


        public IEnumerator SetDepressionStateInternal(bool newState, float delay)
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            SetDepressionStateImmediate(newState);
            ChildDepressionEventProviders.ForEach(x => x.SetDepressionStateImmediate(newState, false));
        }

        private void SetDepressionStateImmediate(bool newState, bool raiseEvents = true)
        {
            bool localDepressionStateChanged = false; // avoid threading issues
            if (newState != depressed)
            {
                localDepressionStateChanged = true;
                depressed = newState;
            }

            if (localDepressionStateChanged)
            {
                if (depressed && raiseEvents)
                {
                    hub.Publish(new DepressionEvent());
                }

                if (depressionEnabled)
                {
                    if (depressed)
                    {
                        GetKeyTransform().transform.Translate(transform.forward * -depressionAmount, Space.World);
                        this.transform.Translate(transform.forward * depressionAmount, Space.World);
                    }
                    else
                    {
                        GetKeyTransform().transform.localPosition = nonDepressedPosition;
                        this.transform.localPosition = Vector3.zero;
                    }
                }

                // hack for now - enable depression color on material by just shifting the colour slightly
                if (depressed)
                {
                    interactionArea.GetLocationBounds().GetComponent<MeshRenderer>().material.color += depressionColor;
                }
                else
                {
                    interactionArea.GetLocationBounds().GetComponent<MeshRenderer>().material.color -= depressionColor;
                }

                if (!dockingManager.IsDocked && keyMetadata != null && raiseEvents)
                    keyMetadata.SimulateKeyPress(false, !depressed);
            }
        }

        private Vector3 nonDepressedPosition;

        private bool detectPhysicsDepression = false;

        public void Update()
        {
            if (detectPhysicsDepression)
            {
                // If our distance is greater than what we specified as a press
                // set it to our max distance and register a press if we haven't already
                float distance = Mathf.Abs(GetKeyTransform().localPosition.z);

                if (!dockingManager.IsDocked)
                {
                    if (distance >= depressionAmount)
                    {
                        SetDepressionState(true);
                    }
                    else
                    {
                        // If we aren't all the way down, reset our press
                        SetDepressionState(false, 0.2f);
                    }

                    // constrain on all axes but z (key depression)
                    GetKeyTransform().localPosition = nonDepressedPosition + new Vector3(0, 0, GetKeyTransform().localPosition.z > 0 ? 0 : GetKeyTransform().localPosition.z < -depressionAmount ? -depressionAmount : GetKeyTransform().localPosition.z); //

                }
                else
                { 
                    SetDepressionState(false);
                }
            }
        }

        public void OnDisable()
        {
            SetDepressionState(false);
        }
    }
}