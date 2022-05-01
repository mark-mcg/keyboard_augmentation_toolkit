using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAT.KeyCodeMappings
{
    public class KeyPositionToUnityKeyCode 
    {
        public static KeyPosition Reverse(KeyCode kc)
        {
            return KeyPositionToWindows.Reverse(WindowsToUnity.Reverse(kc));
        }

        public static KeyCode Forward(KeyPosition position)
        {
            return WindowsToUnity.Forward(KeyPositionToWindows.Forward(position));
        }

        /// <summary>
        /// https://github.com/jamesjlinden/unity-decompiled/blob/master/UnityEngine/UnityEngine/KeyCode.cs
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<KeyCode> GetKeyCodeRange(int start, int end)
        {
            List<KeyCode> keys = new List<KeyCode>();
            for (int i = start; i <= end; i++)
            {
                keys.Add((KeyCode)i);
            }
            return keys;
        }
    }
}
