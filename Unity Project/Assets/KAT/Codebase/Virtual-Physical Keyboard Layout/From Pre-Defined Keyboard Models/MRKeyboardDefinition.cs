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
    /// For Logitech MR Keyboard Proof of Concept. MR Keys are defined using VK codes...
    /// </summary>
    [CreateAssetMenu(fileName = "MRKeyboardDefinition", menuName = "KAT/Layouts/Models/MRKeyboardDefinition_v2)", order = 1)]
    public class MRKeyboardDefinition : BaseKeyboardDefinition
    {
        public bool AddKeyMaterialSwap = true;

        //public override void Awake()
        //{
        //    base.Awake();

        //    //if (KeyboardModelRoot == null)
        //    //    KeyboardModelRoot = this.transform.Find("K780_Proto").gameObject;

        //    if (KeyboardModelRoot == null)
        //        Debug.LogError("KeyboardModelRoot not set, should be pointing to K780_Proto or equivalent");

        //    // UpdateMaterial();
        //}

        public override void SetupDefinition()
        {
            base.SetupDefinition();
            KeycapForward = new Vector3(0, 0, 1);//Vector3.forward;
            FlipTriangleOrder = true;
            SwapYZAxesForPosition = true;
        }

    protected override GameObject FindKeyModel(KKey key)
        {
            GameObject KeyModel = null;
            try
            {
                KeyModel = KeyboardModelRoot.transform.FindDeepChild("vk" +  (int)KeyPositionToWindows.Forward(key.pos)).gameObject;
            }
            catch (Exception e)
            {
                Debug.LogError("Couldn't find key model for keyboard " + KeyboardModelRoot + " key " + key);
            }

            return KeyModel;
        }


        // see https://docs.microsoft.com/en-us/windows/desktop/inputdev/virtual-key-codes
        public override List<KKey> GetSupportedKeys()
        {
            List<KKey> codes = new List<KKey>();
            
            // search through the key gameobjects parsing the VK codes out from the names
            foreach (Transform t in KeyboardModelRoot.transform)
            {
                if (t.gameObject.name.Contains("vk"))
                {
                    int vkcode = int.Parse(t.gameObject.name.Substring(2));
                    KKey k = new KKey(KeyPositionToWindows.Reverse((VirtualKeyCodes)vkcode));
                    //Debug.Log("Key " + t.gameObject.name + " mapped to vkcode " + vkcode + " VirtualKeyCode " + k.VKCode + " name " + k.GetKeyName());
                    codes.Add(k);
                }
            }

            return codes;
        }



        ///*
        // * Ripped from KeyboardAnimator...
        // */

        ///// <summary>
        ///// Call UpdateMaterial() after changing this to reflect changes on the keyboard.
        ///// </summary>
        //public Material regularMaterial, floatingKeysMaterial;
        ///// <summary>
        ///// No need to call UpdateMaterial() after changing this one.
        ///// </summary>
        //public Material pressedMaterial;
        ///// <summary>
        ///// Indicates whether the keyboard is in "floating keys" mode or not.
        ///// </summary>
        //public bool IsBodyVisible { get; private set; }

        //public Material m_unpressed;

        ///// <summary>
        ///// Changes the type of keyboard from a regular to a "floating keys" one.
        ///// </summary>
        //public void ToggleBodyVisibility()
        //{
        //    var body = transform.Find("mainBody").GetComponent<MeshRenderer>();
        //    var constellation = transform.Find("constellation").GetComponent<MeshRenderer>();

        //    body.enabled = !body.enabled;
        //    constellation.enabled = !constellation.enabled;

        //    // changing this bool and calling update material will re-skin the kbd
        //    IsBodyVisible = body.enabled;
        //    UpdateMaterial();
        //}

        ///// <summary>
        ///// Call this after changing one of the public materials to make sure
        ///// the new material immediately reflects on the keyboard.
        ///// </summary>
        //public void UpdateMaterial()
        //{
        //    if (IsBodyVisible)
        //        m_unpressed = regularMaterial;
        //    else
        //        m_unpressed = floatingKeysMaterial;

        //    Renderer[] kbdParts = this.KeyboardModelRoot.GetComponentsInChildren<Renderer>();
        //    foreach (Renderer r in kbdParts)
        //        r.material = m_unpressed;
        //}
    }
}