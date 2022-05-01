using KAT.KeyCodeMappings;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using System;
using System.Collections.Generic;

namespace KAT.Interaction
{
    [RequireComponent(typeof(KUIKeyEventsTrigger))]
    public class KUIKeyElementVisibilityController : KUIElementVisibilityController
    {
        public bool debug = true;
        KUIKeyEventsTrigger trigger;
        public bool VisibleOnlyIfModifiersArePressed = false;

        public void Awake()
        {
            trigger = GetComponent<KUIKeyEventsTrigger>();
        }

        public override KUIElement.VisibilityState CheckVisibilityState()
        {
            KUIElement.VisibilityState visState;

            /*
             * If a modifier is pressed, the modifier must be present on one of the triggers for this to be visible
             * 
             */

            List<KeyPosition> currentPressedModifiers = KeyPositionExtensions.ModifiersAll.Where(y => y.IsKeyPressed()).ToList();

            //Debug.Log("currentPressendModifiers " + currentPressedModifiers.Count + string.Join(",", currentPressedModifiers));

            if (trigger.triggers.Count() == 0 || trigger.triggers.Any(x => {

                // if no modifiers are pressed and we have no modifiers
                bool visible = (currentPressedModifiers.Count == 0 && x.Modifiers.Count() == 0);

                if (x.Modifiers.Count() > 0) {
                    bool areOnlyOurModifiersPressed = currentPressedModifiers.Count > 0 && (currentPressedModifiers.All(pressedMod => x.Modifiers.Any(mod => pressedMod.Equals(mod.pos, true, false))));

                    // or if only our modifiers are pressed (assuming other additional modifiers should negate our action)
                    if (VisibleOnlyIfModifiersArePressed)
                    {
                        visible |= areOnlyOurModifiersPressed;
                    }
                    else
                    {
                        // if we have modifiers, and either only they are pressed, or no modifiers are pressed
                        visible |= areOnlyOurModifiersPressed || currentPressedModifiers.Count == 0;
                        //x.Modifiers.All(mod => currentPressedModifiers.Any(pressedMod => pressedMod.Equals(mod.pos)));
                        // 
                    }
                }

                return visible;
                 })) 
            {
                visState = KUIElement.VisibilityState.Visible;
            }
            else
            {
                visState = KUIElement.VisibilityState.Invisible;
            }

            if (debug) 
                Debug.Log(trigger + " visibility state is " + visState);
            return visState;
        }
    }
}
