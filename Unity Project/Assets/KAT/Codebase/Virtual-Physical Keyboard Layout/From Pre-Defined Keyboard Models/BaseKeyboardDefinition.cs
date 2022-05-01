using KAT.KeyCodeMappings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WindowsInput.Native;

namespace KAT
{

    /// <summary>
    /// Use for defining and parsing a layout from an existing 3D VR keyboard
    /// 
    /// Encapsulates details regarding the specific VR keyboard e.g. what keypress features we should enable,
    /// mapping from Keys to GameObjects for each key etc.
    /// </summary>
    public abstract class BaseKeyboardDefinition : ScriptableObject
    {
        public const string PREFAB_BACKGROUND = "Background Quad";

        public GameObject KeyboardModelRoot;
        public GameObject KeycapPrefab;
        public Vector3 KeycapForward = Vector3.forward;
        public bool FlipTriangleOrder = false;
        public bool DepressKeysWhenDocked = false; // disable this if we are following the keyboard key model which itself handles depression
        public bool SwapYZAxesForPosition = false;

        //public bool AddKeyDepression = false;
        //public bool AddKeyHighlighting = false;
        //public bool AddKeyHighlightOnPress = false;
        //public bool HideKeycapBackground = false;

        public List<KeyPosition> SupportedKeys = new List<KeyPosition>();

        public virtual void SetupDefinition()
        {
        }


        public virtual void Awake()
        {

            //if (HideKeycapBackground)
            //{
            //    // n.b. we need to keep the background quad for the (re)sizing to the keycap, but hide the background itself
            //    Transform background = KeycapPrefab.transform.Find(PREFAB_BACKGROUND);
            //    MeshRenderer BackgroundToFade = background.GetComponent<MeshRenderer>();

            //    Color color = BackgroundToFade.material.color;
            //    color.a = 0.0f;
            //    BackgroundToFade.material.color = color;

            //    KeycapPrefab.GetComponent<TextMeshProFader>().BackgroundToFade = null;
            //}

        }

        public virtual Dictionary<KKey, GameObject> GetKeyPositionMap()
        {
            Dictionary<KKey, GameObject> VKMap = new Dictionary<KKey, GameObject>();
            List<KKey> Keys = GetSupportedKeys();
            SupportedKeys.Clear();
            SupportedKeys.AddRange(Keys.Select(x => x.pos));

            foreach (KKey code in Keys)
            {
                GameObject keyGO = FindKeyModel(code);

                if (!VKMap.ContainsKey(code) && keyGO != null)
                {
                    VKMap.Add(code, keyGO);
                }
            }

            return VKMap;
        }

        /// <summary>
        /// Denotes what physical keys this keyboard has. See  https://docs.microsoft.com/en-us/windows/desktop/inputdev/virtual-key-codes
        /// </summary>
        /// <returns></returns>
        public abstract List<KKey> GetSupportedKeys();

        /// <summary>
        /// Handles mapping from Keys to GameObject models on the keyboard for physical keycaps.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract GameObject FindKeyModel(KKey key);
    }
}