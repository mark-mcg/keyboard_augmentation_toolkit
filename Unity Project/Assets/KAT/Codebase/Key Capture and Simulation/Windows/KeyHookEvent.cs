using KAT.KeyCodeMappings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAT
{

    /// <summary>
    /// Used to track the specifics of a given keypress hook event in InterceptKeyboard.
    /// </summary>
    public class KeyHookEvent : BaseKUIEvent
    {
        public KKey key;

        public bool Up = false;

        /// <summary>
        /// If set, indicates the event was processed, and the intercepting class
        /// should prevent further execution of the keypress
        /// </summary>
        public bool wasConsumed = false;

        /// <summary>
        /// Optionally set by KeyboardState to indicate if this is a new unique event
        /// </summary>
        public bool isNewUniqueKey;

        /// <summary>
        /// Set this if we're just providing the initial keyboard state, and the key
        /// hasn't *actually* been pressed right now (e.g. for toggle states on Start)
        /// </summary>
        public bool isExistingState = false;

        #region KeyHookEvents from Windows
        // Windows specific paramaters
        public int nCode;
        public IntPtr wParam;
        public IntPtr lParam;

        /// <summary>
        /// For creating a KeyHookEvent from Windows hooks
        /// </summary>
        /// <param name="key"></param>
        /// <param name="Up"></param>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        public KeyHookEvent(KKey key, bool Up, int nCode, IntPtr wParam, IntPtr lParam)
        {
            this.key = key;
            this.Up = Up;
            this.nCode = nCode;
            this.wParam = wParam;
            this.lParam = lParam;
            if (!this.key.pos.IsModifier())
                this.selectorTag = new KUIKeyLocationTag(null, this.key);
        }
        #endregion

        public bool IsConsumed()
        {
            return wasConsumed;
        }

        public void NoteConsumed()
        {
            wasConsumed = true;
        }

        public override string ToString()
        {
            return "" + key + " IsUp=" + Up;
        }
    }
}