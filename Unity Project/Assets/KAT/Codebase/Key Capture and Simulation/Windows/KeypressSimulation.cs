using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WindowsInput;
using WindowsInput.Native;
using System.Linq;
using KAT.KeyCodeMappings;

namespace KAT
{
    /// <summary>
    /// Allows the simulation of arbitrary key combinations and unicode character outputs.
    /// </summary>
    public class KeypressSimulation : MonoBehaviour
    {
        private const int WM_CHAR = 0x0102;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Specifically for Unicode...
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr PostMessageW(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        static InputSimulator sim = new InputSimulator();

        /// <summary>
        /// Outputs Unicode text to the focussed application. N.B. this application needs to support receiving Unicode properly (e.g. Chrome)!
        /// </summary>
        /// <param name="text"></param>
        public static void SimulateUnicodeTextOutput(string text)
        {
            UnityEngine.Debug.Log("Triggering text " + text);
            IntPtr handle = GetForegroundWindow();

            if (handle == IntPtr.Zero)
            {
                UnityEngine.Debug.LogError("Failed to get Foreground Window...");
                return;
            }

            foreach (char unicodeCodePoint in text.ToCharArray())
            {
                UnityEngine.Debug.Log("Result for unicode character " + PostMessageW(handle, WM_CHAR, new IntPtr(unicodeCodePoint), IntPtr.Zero));
            }
        }

        public static void SimulateKeyCombinationPress(List<KKey> keys)
        {
            if (keys != null && keys.Count > 0)
            {
                foreach (KKey key in keys.Where(x => x.pos.IsModifier()))
                {
                    sim.Keyboard.KeyDown((VirtualKeyCode)KeyPositionToWindows.Forward(key.pos));
                }

                foreach (KKey key in keys.Where(x => !x.pos.IsModifier()))
                {
                    sim.Keyboard.KeyPress((VirtualKeyCode)KeyPositionToWindows.Forward(key.pos));
                }

                foreach (KKey key in keys.Where(x => x.pos.IsModifier()))
                {
                    sim.Keyboard.KeyUp((VirtualKeyCode)KeyPositionToWindows.Forward(key.pos));
                }
            }
        }

        public static void SimulateClearAll()
        {
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A).KeyPress(VirtualKeyCode.DELETE).KeyPress(VirtualKeyCode.BACK);
        }

        public static void SimulateKeyUpDown(KeyPosition key, bool down)
        {
            if (down)
                sim.Keyboard.KeyDown((VirtualKeyCode)KeyPositionToWindows.Forward(key));
            else
                sim.Keyboard.KeyUp((VirtualKeyCode)KeyPositionToWindows.Forward(key));
        }

        public static void SimulateKeyPress(KeyPosition key)
        {
            sim.Keyboard.KeyPress((VirtualKeyCode)KeyPositionToWindows.Forward(key));
        }
    }
}