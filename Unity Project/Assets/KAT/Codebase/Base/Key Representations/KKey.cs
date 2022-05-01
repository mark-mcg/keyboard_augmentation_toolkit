using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WindowsInput.Native;
using System.Linq;

namespace KAT.KeyCodeMappings
{
    /// <summary>
    /// Abstracts away the internal representation of the keys - was originally KeyCodes, then switched to VKCodes.
    /// </summary>
    [Serializable]
    public class KKey : IEquatable<KKey>
    {
        public KeyPosition pos;

        public KKey(KeyPosition k)
        {
            this.pos = k;
        }

        public bool IsKeyPressed()
        {
            return pos.IsKeyPressed();
        }

        public bool Equals(KKey other, bool fuzzyModifierMatch = false, bool fuzzyNumericMatch = false)
        {
            if (other == null)
                return false;

            return this.pos.Equals(other.pos, fuzzyModifierMatch, fuzzyNumericMatch);
        }

        public bool Equals(KKey other)
        {
            return this.Equals(other, false, false);
        }

        /// <summary>
        /// Provides some default names for the VirtualKeyCodes that are vaguely legible in VR.
        /// </summary>
        /// <returns></returns>
        public string GetKeyName()
        {
            return KeyPositionToString.Forward(this.pos);
        }

        public override string ToString()
        {
            return string.Format("[Key {0}]", GetKeyName());
        }
    }
}