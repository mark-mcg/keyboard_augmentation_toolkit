using KAT;
using KAT.KeyCodeMappings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAT
{
    public class KeyPressOutputAction : KUIBinaryElementAction
    {

        public List<KKey> keysToTrigger;

        public override void PerformAction()
        {
            base.PerformAction();
            if (keysToTrigger != null && keysToTrigger.Count > 0)
            {
                Debug.Log("KeyboardKeyPressShortcutEvent attempting to trigger keypress");
                KeypressSimulation.SimulateKeyCombinationPress(keysToTrigger);
            }
        }
    }
}