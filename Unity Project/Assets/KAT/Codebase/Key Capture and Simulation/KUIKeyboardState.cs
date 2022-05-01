using KAT.KeyCodeMappings;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAT
{
    /// <summary>
    /// Todo - make this not a singleton, incase we have a multi-user/collaborative scene with multiple states
    /// </summary>
    public class KUIKeyboardState : KUIEventProvider
    {
        public KUIManager kuiManager;
        public static List<KKey> CurrentDepressedKeys = new List<KKey>();

        public void Awake()
        {
            kuiManager = GetComponent<KUIManager>();
        }

        public static bool IsKeyPressed(KeyPosition key)
        {
            return CurrentDepressedKeys.Any(x => x.pos.Equals(key, true, false));
        }

        public static KUIKeyboardState singleton { get
            {
                if (state == null)
                    state = FindObjectOfType<KUIKeyboardState>();
                return state;
            } 
        }
        private static KUIKeyboardState state;

        public void ReceiveKUIEvent(BaseKUIEvent @event)
        {
            if (@event is KeyHookEvent keyEvent)
            {
                bool isNewUniqueKey = UpdateInternalKeyboardState(keyEvent);
                keyEvent.isNewUniqueKey = isNewUniqueKey;
                kuiManager.PublishKeyEvent(@event);
            }
        }

        /// <summary>
        /// Tries to emulate the toggle feature of some keys (e.g. capslock) and notes whether 
        /// a keypress is a new event or a repeat (e.g. key being held down)
        /// </summary>
        /// <param name="keyEvent"></param>
        /// <returns></returns>
        public static bool UpdateInternalKeyboardState(KeyHookEvent keyEvent, bool simulateKeyPressIfNotConsumed = false)
        {
            bool retValue = false;
            if (keyEvent.key.pos.IsModifierToggle())
            {
                if (!keyEvent.Up)
                {
                    if (CurrentDepressedKeys.Contains(keyEvent.key))
                        CurrentDepressedKeys.Remove(keyEvent.key);
                    else
                    {
                        CurrentDepressedKeys.Add(keyEvent.key);
                        retValue= true;
                    }
                }
            }
            else
            {
                if (keyEvent.Up)
                {
                    CurrentDepressedKeys.Remove(keyEvent.key);
                }
                else
                {
                    if (!CurrentDepressedKeys.Contains(keyEvent.key))
                    {
                        CurrentDepressedKeys.Add(keyEvent.key);
                        retValue= true;
                    }
                }
            }
            //UnityEngine.Debug.Log("Processed key event " + keyEvent + " current keyboard state for now=" + string.Join(",", CurrentDepressedKeys));
            return retValue;
        }
    }
}
