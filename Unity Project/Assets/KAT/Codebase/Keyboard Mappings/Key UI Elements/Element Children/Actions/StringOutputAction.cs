using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAT
{
    public class StringOutputAction : KUIBinaryElementAction
    {
        public string TextToSend;

        public override void PerformAction()
        {
            base.PerformAction();
            if (TextToSend != null)
            {
                Debug.Log("KeyboardKeyPressShortcutEvent attempting to trigger keypress");
                KeypressSimulation.SimulateUnicodeTextOutput(TextToSend);
            }
        }
    }
}