using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BidirectionalMap;
using BiLookup;

namespace KAT.KeyCodeMappings
{
    public class KeyPositionToWindows 
    {
        public static KeyPosition Reverse(VirtualKeyCodes code)
        {
            return Convert.ReverseFirst(code);
        }

        public static VirtualKeyCodes Forward(KeyPosition position)
        {
            return Convert.ForwardFirst(position);
        }

        public static BiLookup<KeyPosition, VirtualKeyCodes> Convert = new BiLookup<KeyPosition, VirtualKeyCodes>(
            new KeyValueList<KeyPosition, VirtualKeyCodes>() {

            { KeyPosition.Esc, (VirtualKeyCodes) 0x1B },
            { KeyPosition.F1, (VirtualKeyCodes) 0x70 },
            { KeyPosition.F2, (VirtualKeyCodes) 0x71 },
            { KeyPosition.F3, (VirtualKeyCodes) 0x72 },
            { KeyPosition.F4, (VirtualKeyCodes) 0x73 },
            { KeyPosition.F5, (VirtualKeyCodes) 0x74 },
            { KeyPosition.F6, (VirtualKeyCodes) 0x75 },
            { KeyPosition.F7, (VirtualKeyCodes) 0x76 },
            { KeyPosition.F8, (VirtualKeyCodes) 0x77 },
            { KeyPosition.F9, (VirtualKeyCodes) 0x78 },
            { KeyPosition.F10, (VirtualKeyCodes) 0x79 },
            { KeyPosition.F11, (VirtualKeyCodes) 0x7A },
            { KeyPosition.F12, (VirtualKeyCodes) 0x7B },
            { KeyPosition.PrintScreen, (VirtualKeyCodes) 0x2C },
            { KeyPosition.ScrollLock, (VirtualKeyCodes) 0x91 },
            { KeyPosition.Pause, (VirtualKeyCodes) 0x13 },

            { KeyPosition.Tilde, (VirtualKeyCodes) 0xC0 },
            { KeyPosition.N1, (VirtualKeyCodes) 0x31 },
            { KeyPosition.N2, (VirtualKeyCodes) 0x32 },
            { KeyPosition.N3, (VirtualKeyCodes) 0x33 },
            { KeyPosition.N4, (VirtualKeyCodes) 0x34 },
            { KeyPosition.N5, (VirtualKeyCodes) 0x35 },
            { KeyPosition.N6, (VirtualKeyCodes) 0x36 },
            { KeyPosition.N7, (VirtualKeyCodes) 0x37 },
            { KeyPosition.N8, (VirtualKeyCodes) 0x38 },
            { KeyPosition.N9, (VirtualKeyCodes) 0x39 },
            { KeyPosition.N0, (VirtualKeyCodes) 0x30 },
            { KeyPosition.Minus, (VirtualKeyCodes) 0xBD },
            { KeyPosition.Plus, (VirtualKeyCodes) 0xBB },
            { KeyPosition.Backspace, (VirtualKeyCodes) 0x08 },

            { KeyPosition.Tab, (VirtualKeyCodes) 0x09 },
            { KeyPosition.Q, (VirtualKeyCodes) 0x51 },
            { KeyPosition.W, (VirtualKeyCodes) 0x57 },
            { KeyPosition.E, (VirtualKeyCodes) 0x45 },
            { KeyPosition.R, (VirtualKeyCodes) 0x52 },
            { KeyPosition.T, (VirtualKeyCodes) 0x54 },
            { KeyPosition.Y, (VirtualKeyCodes) 0x59 },
            { KeyPosition.U, (VirtualKeyCodes) 0x55 },
            { KeyPosition.I, (VirtualKeyCodes) 0x49 },
            { KeyPosition.O, (VirtualKeyCodes) 0x4F },
            { KeyPosition.P, (VirtualKeyCodes) 0x50 },
            { KeyPosition.Bracket_L, (VirtualKeyCodes) 0xDB },
            { KeyPosition.Bracket_R, (VirtualKeyCodes) 0xDD },
            { KeyPosition.Backslash, (VirtualKeyCodes) 0xDC },
            { KeyPosition.Exclaim, (VirtualKeyCodes) 0xDF },

            { KeyPosition.CapsLock, (VirtualKeyCodes) 0x14 },
            { KeyPosition.A, (VirtualKeyCodes) 0x41 },
            { KeyPosition.S, (VirtualKeyCodes) 0x53 },
            { KeyPosition.D, (VirtualKeyCodes) 0x44 },
            { KeyPosition.F, (VirtualKeyCodes) 0x46 },
            { KeyPosition.G, (VirtualKeyCodes) 0x47 },
            { KeyPosition.H, (VirtualKeyCodes) 0x48 },
            { KeyPosition.J, (VirtualKeyCodes) 0x4A },
            { KeyPosition.K, (VirtualKeyCodes) 0x4B },
            { KeyPosition.L, (VirtualKeyCodes) 0x4C },
            { KeyPosition.Semicolon, (VirtualKeyCodes) 0xBA },
            { KeyPosition.Apastrophe, (VirtualKeyCodes) 0xDE },
            { KeyPosition.OEM_8, VirtualKeyCodes.OEM_8 },

            { KeyPosition.Enter, (VirtualKeyCodes) 0x0D },

            { KeyPosition.Shift_L, (VirtualKeyCodes) 0xA0 },
            { KeyPosition.Iso, (VirtualKeyCodes) 0xE2 },
            { KeyPosition.Z, (VirtualKeyCodes) 0x5A },
            { KeyPosition.X, (VirtualKeyCodes) 0x58 },
            { KeyPosition.C, (VirtualKeyCodes) 0x43 },
            { KeyPosition.V, (VirtualKeyCodes) 0x56 },
            { KeyPosition.B, (VirtualKeyCodes) 0x42 },
            { KeyPosition.N, (VirtualKeyCodes) 0x4E },
            { KeyPosition.M, (VirtualKeyCodes) 0x4D },
            { KeyPosition.Comma, (VirtualKeyCodes) 0xBC },
            { KeyPosition.Period, (VirtualKeyCodes) 0xBE },
            { KeyPosition.Slash, (VirtualKeyCodes) 0xBF },
            { KeyPosition.Shift_R, (VirtualKeyCodes) 0xA1 },

            { KeyPosition.Control_L, (VirtualKeyCodes) 0xA2 },
            { KeyPosition.Win_L, (VirtualKeyCodes) 0x5B },
            { KeyPosition.Alt_L, (VirtualKeyCodes) 0xA4 },
            { KeyPosition.Space, (VirtualKeyCodes) 0x20 },
            { KeyPosition.Alt_R, (VirtualKeyCodes) 0xA5 },
            { KeyPosition.Win_R, (VirtualKeyCodes) 0x5C },
            { KeyPosition.Menu, (VirtualKeyCodes) 0x5D },
            { KeyPosition.Control_R, (VirtualKeyCodes) 0xA3 },

            { KeyPosition.Insert, (VirtualKeyCodes) 0x2D },
            { KeyPosition.Delete, (VirtualKeyCodes) 0x2E },
            { KeyPosition.Home, (VirtualKeyCodes) 0x24 },
            { KeyPosition.End, (VirtualKeyCodes) 0x23 },
            { KeyPosition.PageUp, (VirtualKeyCodes) 0x21 },
            { KeyPosition.PageDown, (VirtualKeyCodes) 0x22 },
            { KeyPosition.Up, (VirtualKeyCodes) 0x26 },
            { KeyPosition.Left, (VirtualKeyCodes) 0x25 },
            { KeyPosition.Down, (VirtualKeyCodes) 0x28 },
            { KeyPosition.Right, (VirtualKeyCodes) 0x27 },

            { KeyPosition.NumLock, (VirtualKeyCodes) 0x90 },
            { KeyPosition.KP_Div, (VirtualKeyCodes) 0x6F },
            { KeyPosition.KP_Mult, (VirtualKeyCodes) 0x6A },
            { KeyPosition.KP_Min, (VirtualKeyCodes) 0x6D },
            { KeyPosition.KP_7, (VirtualKeyCodes) 0x67 },
            { KeyPosition.KP_8, (VirtualKeyCodes) 0x68 },
            { KeyPosition.KP_9, (VirtualKeyCodes) 0x69 },
            { KeyPosition.KP_Plus, (VirtualKeyCodes) 0x6B },
            { KeyPosition.KP_4, (VirtualKeyCodes) 0x64 },
            { KeyPosition.KP_5, (VirtualKeyCodes) 0x65 },
            { KeyPosition.KP_6, (VirtualKeyCodes) 0x66 },
            { KeyPosition.KP_1, (VirtualKeyCodes) 0x61 },
            { KeyPosition.KP_2, (VirtualKeyCodes) 0x62 },
            { KeyPosition.KP_3, (VirtualKeyCodes) 0x63 },
            { KeyPosition.KP_Enter, (VirtualKeyCodes) 0x0D },
            { KeyPosition.KP_0, (VirtualKeyCodes) 0x60 },
            { KeyPosition.KP_Dec, (VirtualKeyCodes) 0x6E },

            { KeyPosition.PlayPause, (VirtualKeyCodes) 0xB3 },
            { KeyPosition.Previous, (VirtualKeyCodes) 0xB1 },
            { KeyPosition.Next, (VirtualKeyCodes) 0xB0 },
            { KeyPosition.Stop, (VirtualKeyCodes) 0xB2 },
            { KeyPosition.Mute, (VirtualKeyCodes) 0xAD },
            { KeyPosition.VolumeDown, (VirtualKeyCodes) 0xAE },
            { KeyPosition.VolumeUp, (VirtualKeyCodes) 0xAF },

            { KeyPosition.Browser_Back, (VirtualKeyCodes) 0xA6 },
            { KeyPosition.Browser_Forward, (VirtualKeyCodes) 0xA7 },
            { KeyPosition.Browser_Refresh, (VirtualKeyCodes) 0xA8 },
            { KeyPosition.Browser_Stop, (VirtualKeyCodes) 0xA9 },
            { KeyPosition.Browser_Search, (VirtualKeyCodes) 0xAA },
            { KeyPosition.Browser_Favorites, (VirtualKeyCodes) 0xAB },

            { KeyPosition.Calculator, (VirtualKeyCodes) 0xB7 },
            { KeyPosition.MediaPlayer, (VirtualKeyCodes) 0xB5 },
            { KeyPosition.Browser, (VirtualKeyCodes) 0xAC },
            { KeyPosition.Mail, (VirtualKeyCodes) 0xB4 },
            { KeyPosition.Help, (VirtualKeyCodes) 0x2F },
            { KeyPosition.Launch1, (VirtualKeyCodes) 0xB6 },
            { KeyPosition.Launch2, (VirtualKeyCodes) 0xB7 },

            { KeyPosition.Sleep, (VirtualKeyCodes) 0x5F },

            { KeyPosition.F13, (VirtualKeyCodes) 0x7C },
            { KeyPosition.F14, (VirtualKeyCodes) 0x7D },
            { KeyPosition.F15, (VirtualKeyCodes) 0x7E },
            { KeyPosition.F16, (VirtualKeyCodes) 0x7F },
            { KeyPosition.F17, (VirtualKeyCodes) 0x80 },
            { KeyPosition.F18, (VirtualKeyCodes) 0x81 },
            { KeyPosition.F19, (VirtualKeyCodes) 0x82 },
            { KeyPosition.F20, (VirtualKeyCodes) 0x83 },
            { KeyPosition.F21, (VirtualKeyCodes) 0x84 },
            { KeyPosition.F22, (VirtualKeyCodes) 0x85 },
            { KeyPosition.F23, (VirtualKeyCodes) 0x86 },
            { KeyPosition.F24, (VirtualKeyCodes) 0x87 }
        });
    }
}