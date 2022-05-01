using KAT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using KAT.Layouts;
using TriangleNet;
using KAT.Interaction;

namespace KAT
{
    public class KUIPrefabDisplayServer : MonoBehaviour
    {
        public static KUIPrefabDisplayServer GetDisplayServer(KUILocationInteractionRegions intArea)
        {
            if (intArea != null)
            {
                KUIPrefabDisplayServer server = intArea.gameObject.GetComponentInChildren<KUIPrefabDisplayServer>();
                if (server == null)
                {
                    Debug.Log("Adding TextMeshPro component to " + intArea);
                    GameObject tmpLocation = new GameObject("Mesh Display");
                    tmpLocation.transform.SetParent(intArea.transform);
                    tmpLocation.transform.localPosition = Vector3.zero;
                    tmpLocation.transform.localScale = Vector3.one;
                    tmpLocation.transform.localEulerAngles = new Vector3(0, 0, 0);
                    server = tmpLocation.AddComponent<KUIPrefabDisplayServer>();
                    server.interactionArea = intArea;
                }
                return server;
            }
            return null;
        }

        [Serializable]
        public class MeshDescriptor
        {
            public GameObject instance;
            public float timeEntered;
            [HideInInspector]
            public KUIPrefabDisplay client;

            public MeshDescriptor(GameObject instance, KUIPrefabDisplay client)
            {
                this.instance = instance;
                this.timeEntered = Time.time;
                this.client = client;
            }
        }

        public Dictionary<object, MeshDescriptor> AvailableMeshes = new Dictionary<object, MeshDescriptor>();
        public MeshDescriptor CurrentMesh;
        public KUILocationInteractionRegions interactionArea;

        // Start is called before the first frame update
        void Awake()
        {
        }

        public void ShowMesh(MeshDescriptor mesh)
        {
            AvailableMeshes[mesh.client] = mesh;
            mesh.timeEntered = Time.time;
            CurrentMesh = AvailableMeshes.Values.Last();
            UpdateDisplay();
        }

        public void StopShowingMesh(object client)
        {
            if (AvailableMeshes.ContainsKey(client))
            {
                AvailableMeshes[client].instance.SetActive(false);
                AvailableMeshes.Remove(client);
            }

            CurrentMesh = AvailableMeshes.Values.Count > 0 ? AvailableMeshes.Values.Last() : null;
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            if (CurrentMesh != null) {
                GameObject Instance = CurrentMesh.instance;

                // we need to size the instance so that it fits the area bounds
                Instance.transform.SetParent(this.transform);
                Instance.transform.localPosition = Vector3.zero;
                Instance.transform.localEulerAngles = new Vector3(90, 0, 0);
                Instance.transform.localScale = Vector3.one;

                KUILocationInteractionRegions intArea = GetComponentInParent<KUILocationInteractionRegions>();
                Vector3 localMeshSize = intArea.GetLocationBounds().GetLocalMeshBoundsSize(intArea.transform);
                Vector3 localMeshCenter = intArea.GetLocationBounds().GetLocalMeshBoundsCenter(intArea.transform);

                // scale the instance so it fits the bounds of the interaction areas
                Bounds bounds = UnityMeshExtensions.GetEncapsulatedBounds(Instance); //this.gameObject);

                // uniformly scale
                float scaleFactor = Mathf.Min(localMeshSize.x / bounds.size.x, localMeshSize.y / bounds.size.y);
                Instance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                Instance.transform.localPosition += localMeshCenter + new Vector3(0, 0, localMeshSize.z/2 + (bounds.size.z * scaleFactor)/2  + 0.015f); // offset so it sits above the key entirely

                Rigidbody rb = Instance.GetComponentInChildren<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }                
                Instance.SetActive(true);
            }
        }
    }
}
