using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAT.KeyCodeMappings
{
    public class KLEToKeyPosition
    {
        public static KeyPosition Forward(string KLE)
        {
            return KeyPositionToWindows.Reverse(WindowsToKLE.Reverse(KLE));
        }

        public static KeyPosition Forward(string KLE, int rowPosition)
        {
            return KeyPositionToWindows.Reverse(WindowsToKLE.Reverse(KLE, rowPosition));
        }

        public static string Reverse(KeyPosition position)
        {
            return WindowsToKLE.Forward(KeyPositionToWindows.Forward(position));
        }
    }
}
