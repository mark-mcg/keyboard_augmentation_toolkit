using KAT.KeyCodeMappings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WindowsInput.Native;

namespace KAT {

    /// <summary>
    /// For Logitech Bridge SDK G810 model - keys are defined using Unity KeyCodes...
    /// </summary>
    [CreateAssetMenu(fileName = "G810KeyboardDefinition", menuName = "KAT/Layouts/Models/G810KeyboardDefinition_v2)", order = 1)]
    public class G810KeyboardDefinition : BaseKeyboardDefinition
    {
        protected override GameObject FindKeyModel(KKey key)
        {
            GameObject KeyModel = null;
            try
            {
                KeyModel = KeyboardModelRoot.transform.FindDeepChild("group1_" + KeyPositionToUnityKeyCode.Forward(key.pos).ToString()).gameObject;

                if (KeyModel == null)
                {
                    string name = "group1_";

                    switch (key.pos)
                    {
                        case KeyPosition.Shift_L:
                            name += "LeftShift";
                            break;
                        case KeyPosition.Shift_R:
                            name += "RightShift";
                            break;
                    }

                    KeyModel = KeyboardModelRoot.transform.FindDeepChild(name).gameObject;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Couldn't find key model for keyboard " + KeyboardModelRoot + " key " + key);
            }

            return KeyModel;
        }

        public override List<KKey> GetSupportedKeys()
        {
            List<KKey> codes = new List<KKey>();

            // search through the key gameobjects parsing the VK codes out from the names
            foreach (Transform t in KeyboardModelRoot.transform)
            {
                if (t.gameObject.name.Contains("group1_"))
                {
                    KeyCode key;
                    if ( Enum.TryParse<KeyCode>(t.gameObject.name.Substring(7), out key))
                    {
                        KeyPosition keyPos = KeyPositionToUnityKeyCode.Reverse(key);
                        codes.Add(new KKey(keyPos));
                    }
                }
            }

            return codes;
        }
    }
}