using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TriangleNet;
using System;

namespace KAT.Layouts
{
    /// <summary>
    /// Base class for a serialized virtual-physical 2D/3D layout. Should indicate the KUILocations available through the exposed KUILocationDescriptors
    /// </summary>
    public class KUISerializedLayout : ScriptableObject
    {
        //[HideInInspector]
        protected KUILocationDescriptors LocationsCollection = new KUILocationDescriptors();
        
        // this is for generated composite locations that are temporary
        protected KUILocationDescriptors GeneratedLocationsCollection = new KUILocationDescriptors();

        [BoxGroup("From Prefab")]
        public GameObject LayoutPrefab;

        [ReadOnly]
        public GameObject InstanceRoot;


        // todo: change these to be metadata attachments for flexibility?
        [BoxGroup("Shared Preferences")]
        public Color DefaultTextColor = Color.black;
        [BoxGroup("Shared Preferences")]
        public Color DefaultHighlightColor = Color.grey;
        [BoxGroup("Shared Preferences")]
        public float TouchDepressionAmount = 0.005f;

        public void OnEnable()
        {
            LocationsCollection.Clear();
        }

        public void OnDisable()
        {
            LocationsCollection.Clear();
        }

        public KUILocationDescriptors GetLocationDescriptors()
        {
            if (InstanceRoot == null)
                BuildLayout();
            if (LocationsCollection.Count == 0)
                LocationsCollection.AddRange(InstanceRoot.GetComponentsInChildren<KUILocationDescriptor>());
            return new KUILocationDescriptors(LocationsCollection);
        }

        /// <summary>
        /// Create a composite location - for example, if we have a key that spans multiple keys, this will create a
        /// merged descriptor and a composite mesh made up of all the constituent keys.
        /// </summary>
        /// <param name="fromCollections"></param>
        /// <param name="tagToUse"></param>
        /// <returns></returns>
        public KUILocationDescriptor CreateCompositeLocationDescriptor(KUILocationDescriptors fromCollections, KUILocationDescriptor tagToUse)
        {
            KUILocationDescriptors matchingLayoutLocations = new KUILocationDescriptors();
            foreach (KUILocationDescriptor collection in fromCollections)
            {
                KUILocationDescriptor match = GetLocationDescriptors().FirstOrDefault(x => x.MatchAllTags(collection));
                if (match != null)
                    matchingLayoutLocations.Add(match);
                else
                {
                    Debug.LogError("Couldn't get a match, asking for a location that doesn't exist in layout");
                }                    
            }

            KUILocationDescriptor locationTagMB = null;
            GameObject compositeObject = null;
            if (matchingLayoutLocations.Count > 0)
            {
                compositeObject = new GameObject("Composite Element Location for: " + string.Join(",", fromCollections));
                compositeObject.transform.SetParent(InstanceRoot.transform);
                compositeObject.transform.position = //matchingLayoutLocations.Average(x => x.transform.position);
                     new Vector3(
                        matchingLayoutLocations.Average(x => x.transform.position.x),
                        matchingLayoutLocations.Average(x => x.transform.position.y),
                        matchingLayoutLocations.Average(x => x.transform.position.z));

                compositeObject.transform.localScale = Vector3.one;
                compositeObject.transform.localScale = new Vector3(1 / matchingLayoutLocations[0].transform.lossyScale.x, 1 / matchingLayoutLocations[0].transform.lossyScale.y, 1 / matchingLayoutLocations[0].transform.lossyScale.z);
                compositeObject.transform.eulerAngles = //matchingLayoutLocations[0].transform.eulerAngles;
                    new Vector3(
                        matchingLayoutLocations.Average(x => x.transform.eulerAngles.x),
                        matchingLayoutLocations.Average(x => x.transform.eulerAngles.y),
                        matchingLayoutLocations.Average(x => x.transform.eulerAngles.z));

                // create and add the mesh to be used
                UnityEngine.Mesh mesh = TriangleNet.UnityMeshExtensions.MergeUnityMeshes(
                matchingLayoutLocations.Select(x =>
                {
                    KUILocationBounds localMeshHelper = x.GetComponentInChildren<KUILocationBounds>();
                    Debug.Log("Composite: adding location " + x.gameObject + " mesh " + localMeshHelper.mesh + " with root " + localMeshHelper.GetMeshRoot());
                    Tuple<UnityEngine.Mesh, Transform> tup = new Tuple<UnityEngine.Mesh, Transform>(localMeshHelper.mesh, localMeshHelper.GetMeshRoot().transform);
                    return tup;
                }).ToList(), compositeObject.transform, true, true);
                compositeObject.AddMesh(mesh, false, false, true, false);
                KUILocationBounds meshHelper = compositeObject.AddComponent<KUILocationBounds>();


                // make the mesh semi transparent ( as it will be rendering on top/along side of the existing meshes it was made up of )
                MeshRenderer renderer = compositeObject.GetComponentInChildren<MeshRenderer>();
                renderer.materials = matchingLayoutLocations[0].GetComponent<MeshRenderer>().sharedMaterials;

                Color color = renderer.material.color;
                color.a = 0.6f;
                renderer.material.color = color;
                UnityMeshExtensions.SetupMaterialWithBlendMode(renderer.material, UnityMeshExtensions.BlendMode.Transparent);

                // tag this new object in our layout
                locationTagMB = compositeObject.AddComponent<KUILocationDescriptor>();
                locationTagMB.AddRuntimeTags(tagToUse.GetAllLocationTags(), true);
                LocationsCollection.Add(locationTagMB);
                GeneratedLocationsCollection.Add(locationTagMB);
                compositeObject.transform.localPosition += new Vector3(0, 0, 0.00001f);

            }
            return locationTagMB;
        }

        public virtual GameObject BuildLayout()
        {
            if (LayoutPrefab != null)
            {
                InstanceRoot = GameObject.Instantiate(LayoutPrefab);
            }
            return InstanceRoot;
        }
    }
}
