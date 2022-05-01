using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Text;
using WindowsInput;
using WindowsInput.Native;
using KAT.KeyCodeMappings;

namespace KAT
{
    public interface KeyboardStateChanged
    {
        bool OnKeyboardStateChanged(KeyHookEvent keyEvent);
    }

    /// <summary>
    /// Handles all the dirty stuff regarding the interception of keyboard events using the LowLevelKeyboardHook Windows API.
    /// 
    /// NB For some reason (threading?) we get segmentation faults or null references if we try to use this
    /// in a non-static way....
    /// </summary>
    [RequireComponent(typeof(KUIKeyboardState))]
    public class InterceptKeyboard : MonoBehaviour
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [Flags]
        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80,
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private const int WM_CHAR = 0x0102;
        private static IntPtr _hookID = IntPtr.Zero;
        public static InterceptKeyboard componentInstance;        

        public void Start()
        {
            componentInstance = this;
            _hookID = SetHook(HookCallback);
        }

        //public static void RaiseEventStatic(BaseKUIEvent @event)
        //{
        //    if (componentInstance != null) componentInstance.RaiseEvent(@event);
        //}

        public void OnDisable()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        #region Low level hook callback
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam); // HookCallback conforms to this


        /// <summary>
        /// This method intercepts keyboard events and checks to see if any of our shortcuts are triggered by, or suppress, the event.
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN || wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP))
            {
                //UnityEngine.Debug.LogError("HookCallback ------------------------------------ " + nCode + " " + " " + Marshal.ReadInt32(lParam) + " " + wParam);

                KBDLLHOOKSTRUCT kbd = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

                if ((kbd.flags & KBDLLHOOKSTRUCTFlags.LLKHF_INJECTED) == 0)
                {
                    //int vkCode = Marshal.ReadInt32(lParam);

                    try
                    {
                        KKey key = new KKey(KeyPositionToWindows.Reverse((VirtualKeyCodes)kbd.vkCode));
                        KeyHookEvent keyEvent = new KeyHookEvent(key, wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP, nCode, wParam, lParam);

                        // UnityEngine.Debug.Log("Got key " + ((VirtualKeyCodes)kbd.vkCode));
                        // update keyboard state 
                        KUIKeyboardState.singleton.ReceiveKUIEvent(keyEvent);

                        // then inform other listeners and see if the event was consumed
                        // RaiseEventStatic(keyEvent);
                        if (keyEvent.wasConsumed)
                        {
                            //UnityEngine.Debug.Log("++ KeyEvent " + keyEvent + " was consumed, returning -1");
                            return new IntPtr(-1); // suppress the event
                        }
                        else
                        {
                            //UnityEngine.Debug.Log("++KeyEvent " + keyEvent + " was not consumed, forwarding to next hook");
                            return CallNextHookEx(_hookID, nCode, wParam, lParam);
                        }
                }
                    catch (Exception e)
                {
                    UnityEngine.Debug.LogError("++ Exception during HookCallback " + e + e.StackTrace);
                }
            }
                else
                {
                    //UnityEngine.Debug.Log("++ KeyEvent was injected (probably by us), ignoring!");
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
        #endregion

        #region Modifier key handling
        /// <summary>
        /// Gets the state of modifier keys for a given keycode.
        /// </summary>
        /// <param name="keyCode">The keyCode</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);
        #endregion
    }

}