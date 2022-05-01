using KAT.Layouts;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriangleNet;
using UnityEngine;
using UnityEngine.UIElements;

namespace KAT.Interaction
{
    public class KUILocationInteractionRegions : KUIParentChildEventProvider
    {
        public KUILocationBounds LocationBounds;

        [ReadOnly,BoxGroup("Internal Debug")]
        protected KUILocation parentLocation;

        public void Awake()
        {
            SetupInteractionRegions();
        }

        public void Start()
        {
            GetComponentsInChildren<IKUIInteractionProvider>().ToList().ForEach(x => {
                x.SetLocationInteractionRegion(this);
            });
        }

        #region Construction
        public static KUILocationInteractionRegions ConstructInteractionRegion(KUILocation root, GameObject KUIInteractionManagerPrefab = null)
        {
            if (KUIInteractionManagerPrefab == null)
            {
                KUIInteractionManagerPrefab = Resources.Load("Prefabs/Default KUIInteractionManager") as GameObject;
            }
            GameObject intManagerInstance = Instantiate(KUIInteractionManagerPrefab);
            intManagerInstance.transform.SetParent(root.transform);
            KUILocationInteractionRegions intArea = intManagerInstance.GetComponent<KUILocationInteractionRegions>();
            intArea.SetParent(root);
            return intArea;
        }

        public void SetParent(KUILocation root)
        {
            if (parentLocation != null && parentLocation != root)
            {
                parentLocation.hub.Unsubscribe<DockingEvent>();
                parentLocation.hub.Unsubscribe<LayoutChangedEvent>();
            }

            parentLocation = root;
            parentLocation.hub.Subscribe<DockingEvent>(@event =>
            {
                SetDocked(@event.docked);
            });

            parentLocation.hub.Subscribe<LayoutChangedEvent>(@event =>
            {
                OnKeyboardLayoutChanged();
            });

            SetDocked(GetComponentInParent<KUIDockingManager>().IsDocked);
        }
        #endregion

        public void UpdateBounds(KUILocationBounds bounds)
        {
            this.LocationBounds = bounds;
            OnKeyboardLayoutChanged();
        }

        public KUILocationBounds GetLocationBounds()
        {
            return LocationBounds;
        }

        public KUILocation GetLocation()
        {
            return parentLocation;
        }

        #region Interaction Regions (colliders, gameobjects)
        private GameObject gestureSurface;
        [ReadOnly, BoxGroup("Internal Debug")]
        public BoxCollider surfaceCollider, midairCollider, depressionCollider, xrInteractionCollider;
        private Rigidbody keyMeshRB, depressionRB, gestureRB, xrRB;
        //private SpringJoint keySpring;
        private bool followModelAnchor = false;

        public void SetInteractionAreaEnabled(bool enabled)
        {
            this.GetComponentsInChildren<Collider>().ToList().ForEach(x => x.enabled = enabled);
            if (gestureSurface != null)
                gestureSurface.SetActive(enabled);
        }

        [Button("Setup Prefab Interaction Regions")]
        public void SetupInteractionRegions()
        {
            if (transform.Find("Depression Surface"))
            {
                depressionCollider = transform.Find("Depression Surface").GetComponent<BoxCollider>();
                depressionRB = transform.Find("Depression Surface").GetComponent<Rigidbody>();

                gestureSurface = transform.Find("Gesture Surface").gameObject;
                gestureRB = gestureSurface.GetComponent<Rigidbody>();
                surfaceCollider = gestureSurface.GetComponent<BoxCollider>();

                xrInteractionCollider = transform.Find("XR Interaction").GetComponent<BoxCollider>();
                xrRB = transform.Find("XR Interaction").GetComponent<Rigidbody>();

                keyMeshRB = gameObject.GetComponent<Rigidbody>();
                midairCollider = transform.Find("Above Surface").GetComponent<BoxCollider>();
            }
            else 
            {
                // Position the container transform to default
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                transform.localScale = Vector3.one;
                keyMeshRB = gameObject.GetComponent<Rigidbody>();
                if (keyMeshRB == null)
                    keyMeshRB = gameObject.AddComponent<Rigidbody>();
                keyMeshRB.useGravity = false;
                keyMeshRB.isKinematic = true;

                // each key element is composed of a number of colliders
                // these denote e.g. the gesture area on the surface of the key or above the key, and the "physical" surface of the key

                if (createDebugCollider)
                {
                    // Debug collider for testing sizes of mesh
                    GameObject debugColliderGO = new GameObject("Debug Collider");
                    debugColliderGO.transform.SetParent(transform);
                    debugColliderGO.transform.localPosition = Vector3.zero;
                    debugColliderGO.transform.localEulerAngles = Vector3.zero;
                    debugColliderGO.transform.localScale = Vector3.one;
                    debugCollider = debugColliderGO.AddComponent<BoxCollider>();
                    debugCollider.isTrigger = false;
                }

                // Define the depression surface - for collisions with e.g. fingers
                GameObject depressionSurface = new GameObject("Depression Surface");
                depressionSurface.transform.SetParent(transform);
                depressionSurface.transform.localPosition = Vector3.zero;
                depressionSurface.transform.localEulerAngles = Vector3.zero;
                depressionSurface.transform.localScale = Vector3.one;
                depressionCollider = depressionSurface.AddComponent<BoxCollider>();
                depressionCollider.isTrigger = true;
                depressionRB = depressionSurface.AddComponent<Rigidbody>();
                depressionRB.isKinematic = true;
                depressionRB.useGravity = false;

                // collider that lies on the surface of the key and extends upwards slightly (for surface gestures like swipes)
                gestureSurface = new GameObject("Gesture Surface");
                gestureSurface.transform.SetParent(gameObject.transform);
                gestureSurface.transform.localEulerAngles = Vector3.zero;
                gestureSurface.transform.localPosition = Vector3.zero;
                gestureSurface.transform.localScale = Vector3.one;
                surfaceCollider = gestureSurface.AddComponent<BoxCollider>();
                surfaceCollider.isTrigger = true;
                gestureRB = gestureSurface.AddComponent<Rigidbody>();
                gestureRB.isKinematic = true;
                gestureRB.useGravity = false;


                // XR Interaction collider - this is for unity XR event detection, and needs to be on its own layer to avoid colliding with other objects
                GameObject xrInteractionGO = new GameObject("XR Interaction");
                xrInteractionGO.transform.SetParent(transform);
                xrInteractionGO.transform.localPosition = Vector3.zero;
                xrInteractionGO.transform.localEulerAngles = Vector3.zero;
                xrInteractionGO.transform.localScale = Vector3.one;

                xrInteractionCollider = xrInteractionGO.AddComponent<BoxCollider>();
                xrInteractionCollider.isTrigger = false;
                xrRB = xrInteractionGO.AddComponent<Rigidbody>();
                xrRB.useGravity = false;
                xrRB.isKinematic = true;

                // this is the layer that XR controllers should be interacting on, but other collisions shouldn't operate here
                xrInteractionGO.layer = LayerMask.NameToLayer("XRInteractionLayer");
                if (xrInteractionGO.layer == -1)
                    Debug.LogError("Layer XRInteractionLayer not found, XR interactions won't work properly! Add this layer in the Editor, then under Edit -> Project Settings -> Physics disable interactions with other layers, and tag any XR Interactable controllers to only work on this layer to avoid physics collisions.");

                // add the interactable last, as otherwise the events don't get subscribed to correctly
                XRInteractableEventProvider xrInteractionEventProvider = xrInteractionGO.AddComponent<XRInteractableEventProvider>();


                // now add a mesh collider denoting the space above the key (e.g. for hover interactions)
                // we'll make this proportional to the existing dimensions of the surface
                GameObject aboveSurface = new GameObject("Above Surface");
                aboveSurface.transform.SetParent(gameObject.transform);
                aboveSurface.transform.localEulerAngles = Vector3.zero;
                aboveSurface.transform.localPosition = Vector3.zero;
                aboveSurface.transform.localScale = Vector3.one;
                midairCollider = aboveSurface.AddComponent<BoxCollider>();
                midairCollider.isTrigger = true;

                Rigidbody aboveSurfaceRB = aboveSurface.AddComponent<Rigidbody>();
                aboveSurfaceRB.isKinematic = true;
                aboveSurfaceRB.useGravity = false;
            }

            // now add event providers 
            //aboveSurface.AddComponent<CollisionEventProvider>();
            //root.AddEventProvider(depressionSurface.AddComponent<DepressionEventProvider>());
            //root.AddEventProvider(gestureSurface.AddComponent<CollisionEventProvider>());
            //root.AddEventProvider(gestureSurface.AddComponent<SwipeEventProvider>());
            //root.AddEventProvider(gestureSurface.AddComponent<TapEventProvider>());
            //root.AddEventProvider(aboveSurface.AddComponent<DwellEventProvider>());
        }
        #endregion

        #region Layout management
        private bool createDebugCollider = false;
        BoxCollider debugCollider;

        public void SetCollisionsActive(bool active)
        {
            GetComponentsInChildren<Collider>().ToList().ForEach(x => x.enabled = active);
        }

        public void OnKeyboardLayoutChanged()
        {
            if (LocationBounds == null)
            {
                Debug.LogError("No LocationBounds present, LocationBounds should be assigned to the mesh");
                return;
            }

            //Debug.Log("InteractionArea UpdatingLayout for " + this.gameObject.GetFullName() +  "  using meshhelper " + meshHelper.gameObject.GetFullName());

            // set our scale to effectively be 1/1/1 globally
            Transform inverseScale = LocationBounds.GetMeshRoot().transform;
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(1 / inverseScale.lossyScale.x, 1 / inverseScale.lossyScale.y, 1 / inverseScale.lossyScale.z);
            Vector3 localMeshSize = LocationBounds.GetLocalMeshBoundsSize(this.transform);
            Vector3 localMeshCenter = LocationBounds.GetLocalMeshBoundsCenter(this.transform);

            //Debug.Log("Mesh Sizes are " + localMeshSize.ToString("F4") + "  center " + localMeshCenter.ToString("F4"));
            if (createDebugCollider && debugCollider != null)
            {
                debugCollider.size = localMeshSize;
                debugCollider.center = localMeshCenter;
            }

            if (depressionCollider != null)
            {
                Vector3 depressionBounds = localMeshSize;
                if (depressionBounds.z < 0.01)
                    depressionBounds = new Vector3(depressionBounds.x, depressionBounds.y, 0.02f);

                depressionCollider.size = new Vector3(depressionBounds.x * 0.8f, depressionBounds.y * 0.8f, depressionBounds.z);
                depressionCollider.center = localMeshCenter + new Vector3(0, 0, localMeshSize.z / 2 - (depressionBounds.z / 2) - 0.0025f);
            }
            else
            {
                Debug.LogError("No depression collider?");
            }

            if (gestureSurface != null)
            {
                gestureSurface.AddOrUpdateMesh(LocationBounds.mesh, false, false, false, false);

                if (surfaceCollider != null)
                {
                    // gesture surface is at the front edge of the mesh on z
                    float offset = 0.01f;
                    surfaceCollider.size = new Vector3(localMeshSize.x, localMeshSize.y, offset);
                    surfaceCollider.center = localMeshCenter + new Vector3(0, 0, localMeshSize.z / 2 + offset / 2);
                    xrInteractionCollider.size = surfaceCollider.size;
                    xrInteractionCollider.center = surfaceCollider.center;
                }

                if (midairCollider != null)
                {
                    // mid air collider is intended to be hoverable above the key i.e. a few centimeters beyond the gesture surface
                    float offset = 0.015f;
                    float depth = 0.04f;
                    midairCollider.size = new Vector3(localMeshSize.x, localMeshSize.y, depth);
                    midairCollider.center = localMeshCenter + new Vector3(0, 0, localMeshSize.z / 2 + offset + depth / 2);
                }
            }
            else
            {
                Debug.LogError("No gesture surface?");
            }

            parentLocation.hub.Publish(new LocationInteractionRegionChangedEvent());
        }
        #endregion

        #region Docking and layout change event support

        public void SetDocked(bool isDocked)
        {
            if (isDocked)
            {
                if (keyMeshRB)
                {
                    keyMeshRB.detectCollisions = false;
                    //keySpring.enableCollision = false;
                    keyMeshRB.transform.localPosition = Vector3.zero;
                }
                followModelAnchor = true;
            }
            else
            {
                if (keyMeshRB) keyMeshRB.detectCollisions = true;
                //keySpring.enableCollision = true;
                followModelAnchor = false;
            }
        }
        #endregion
    }
}
