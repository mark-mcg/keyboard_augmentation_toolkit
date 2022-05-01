using KAT.KeyCodeMappings;
using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace KAT
{
    [RequireComponent(typeof(KUILocationDescriptor))]
    public class KUIKeyLocationMetadata : KUILocationMetadata
    {
        public void Awake()
        {
            if (GetComponent<KUILocationDescriptor>() == null)
                gameObject.AddComponent<KUILocationDescriptor>();
        }

        public List<KKey> metadata = new List<KKey>();

        public override List<KUILocationTag> GetLocationTags()
        {
            return new List<KUILocationTag>() { new KUIKeyLocationTag(metadata.ToArray()) };
        }

        public KeyHookEvent SimulateKeyPress(bool keyPress, bool keyUpState)
        {
            if (metadata.Count == 1)
            {
                KeyHookEvent keyEvent = new KeyHookEvent(metadata[0], keyUpState, 0, IntPtr.Zero, IntPtr.Zero);
                KUIKeyboardState.singleton.ReceiveKUIEvent(keyEvent);
                if (!keyEvent.wasConsumed && false)
                {
                    if (keyPress)
                        KeypressSimulation.SimulateKeyPress(metadata[0].pos);
                    else
                        KeypressSimulation.SimulateKeyUpDown(metadata[0].pos, !keyUpState);
                }
                return keyEvent;
            }

            if (metadata.Count > 1 || metadata.Count == 0)
            {
                Debug.LogErrorFormat("Unsupported depression states with multiple/no keys, metadata count {0}", metadata.Count);
            }
            return null;
        }
    }
}
