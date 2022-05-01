using KAT.KeyCodeMappings;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriangleNet;
using UnityEngine;

namespace KAT.Layouts 
{
    /// <summary>
    /// Specific class for handling a variety of serialized keyboard layouts, either based on
    /// - 2/3d keyboard model definition (where key locations need to be identified)
    /// - 2d KLE layout (text asset)
    /// - 2/3d existing prefab (where key locations are assumed to already be assigned)
    /// </summary>
    [CreateAssetMenu(fileName = "KUI Keyboard Layout", menuName = "KAT/Layouts/KUI Keyboard Layout", order = 1), Serializable]
    public class KUISerializedKeyboardLayout : KUISerializedLayout
    {
        #region From 3D keyboard model with a known naming scheme for keys
        [BoxGroup("For Physical Keyboard Definition")]
        public BaseKeyboardDefinition keyboardModelDefinition;
        [BoxGroup("For Physical Keyboard Definition")]
        public Material[] MaterialsToApply;

        [BoxGroup("For Keyboard Layout Editor")]
        public TextAsset KLELayout;

        [BoxGroup("Shared Keyboard Preferences")]
        public float UniformScale = 1.0f;

        public override GameObject BuildLayout()
        {
            if (InstanceRoot == null)
            {
                if (LayoutPrefab != null)
                {
                    GenerateLayoutFromPrefab();
                }
                else if (keyboardModelDefinition != null)
                {
                    GenerateLayoutFromDefinition();
                }
                else if (KLELayout != null)
                {
                    GeneratedLayoutFromKLELayout();
                }
            }

            InstanceRoot.transform.localScale = new Vector3(UniformScale, UniformScale, UniformScale);

            return InstanceRoot;
        }

        protected virtual void GenerateLayoutFromPrefab()
        {
            InstanceRoot = GameObject.Instantiate(LayoutPrefab);
            InstanceRoot.transform.localPosition = Vector3.zero;
            InstanceRoot.transform.localEulerAngles = Vector3.zero;
            InstanceRoot.transform.localScale = Vector3.one;
            InstanceRoot.name += " Layout Instance";
        }

        public void AddGameObjectToRoot(GameObject go) {
            go.transform.SetParent(InstanceRoot.transform);
        }

        protected virtual void GenerateLayoutFromDefinition()
        {
            InstanceRoot = new GameObject(keyboardModelDefinition.name + " Layout Instance");
            name = keyboardModelDefinition.name;
            Dictionary<KKey, GameObject> VKMap = keyboardModelDefinition.GetKeyPositionMap();

            foreach (KKey key in VKMap.Keys)
            {
                GameObject element = CreateKeyElement(VKMap[key]);
                KUIKeyLocationMetadata metadata = element.AddComponent<KUIKeyLocationMetadata>();
                metadata.metadata.Add(key);
                element.name = "Element Location for " + key;
            }

            // get any non-key locations and add those too
            List<KUILocationDescriptor> preDefinedLocations = keyboardModelDefinition.KeyboardModelRoot.GetComponentsInChildren<KUILocationDescriptor>().ToList();
            Debug.Log("Got " + preDefinedLocations.Count + " preDefinedLocations");
            foreach (KUILocationDescriptor elementLocation in preDefinedLocations)
            {
                GameObject element = CreateKeyElement(elementLocation.gameObject);
                KUILocationDescriptor localLocationDescriptor = element.AddComponent<KUILocationDescriptor>();
                localLocationDescriptor.AddRuntimeTags(elementLocation.GetAllLocationTags(true), true);
                element.name = "Element Location for " + string.Join(",", elementLocation.GetAllLocationTags());
            }
        }

        protected GameObject CreateKeyElement(GameObject model)
        {
            GameObject kbOrigin = keyboardModelDefinition.KeyboardModelRoot.transform.Find("Origin").gameObject;
            GameObject element = new GameObject("Element Location from " + model.name);
            UnityEngine.Mesh mesh = UnityMeshExtensions.ExtractSurfaceMesh(model.GetComponent<MeshFilter>(), keyboardModelDefinition.KeycapForward, 20, keyboardModelDefinition.FlipTriangleOrder);

            if (mesh != null)
            {
                element.transform.localPosition = kbOrigin.transform.InverseTransformPoint(model.transform.position) + new Vector3(0f,0f,0.0001f);
                element.transform.localEulerAngles= kbOrigin.transform.localEulerAngles - model.transform.localEulerAngles;
                element.transform.localScale = Vector3.one;
                element.AddMesh(mesh, false, false, true, false);
                element.AddComponent<KUILocationBounds>();

                if (MaterialsToApply != null && MaterialsToApply.Length > 0)
                {
                    MeshRenderer elementMR = element.GetComponent<MeshRenderer>();
                    elementMR.materials = MaterialsToApply;
                }

                element.transform.SetParent(InstanceRoot.transform);
            }

            return element;
        }
        #endregion

        #region From KLE
        protected virtual void GeneratedLayoutFromKLELayout()
        {
            KLELayoutParser parser = new KLELayoutParser();
            KLEKeyboard keyboard = parser.ParseLayoutFromAsset(KLELayout);
            InstanceRoot = parser.GenerateKeyboardLayoutFromKLEKeyboard(KLELayout.name, keyboard);
        }
        #endregion

    }
}
