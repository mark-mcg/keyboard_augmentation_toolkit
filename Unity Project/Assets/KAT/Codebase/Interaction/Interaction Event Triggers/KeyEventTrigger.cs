using KAT.KeyCodeMappings;
using NaughtyAttributes;
using UnityEngine;
using System;
using System.Linq;

namespace KAT
{
    [Serializable]
    public class KeyEventTrigger
    {
        public KKey key;
        public KKey[] Modifiers = new KKey[0];

        public override string ToString()
        {
            return "KeyEventTrigger {" + key + " mods: " + string.Join(",", Modifiers.AsEnumerable()) + "}"; 
        }

        public bool IsPressed()
        {
            return key.IsKeyPressed() && (Modifiers.Count() == 0 || (Modifiers.Count() > 0 && Modifiers.All(y => y.IsKeyPressed())));

        }
    }
}