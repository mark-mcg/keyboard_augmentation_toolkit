using BidirectionalMap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BiLookup;

namespace KAT.KeyCodeMappings
{
    public class KeyPositionToString
    {
        public static KeyPosition Reverse(string kc)
        {
            return Convert.ReverseFirst(kc);
        }

        public static string Forward(KeyPosition position)
        {
            return Convert.ForwardFirst(position);
        }

        public static BiLookup<KeyPosition, string> Convert = new BiLookup<KeyPosition, string>(
            
            new KeyValueList<KeyPosition, string>() {

            // Number row
            { KeyPosition.Tilde, "~"},
            { KeyPosition.Tilde, "`"},
            { KeyPosition.Tilde, "Tilde"},
            { KeyPosition.N1, "1"},
            { KeyPosition.N2, "2"},
            { KeyPosition.N3, "3"},
            { KeyPosition.N4, "4"},
            { KeyPosition.N5, "5"},
            { KeyPosition.N6, "6"},
            { KeyPosition.N7, "7"},
            { KeyPosition.N8, "8"},
            { KeyPosition.N9, "9"},
            { KeyPosition.N0, "0"},
            { KeyPosition.Minus, "-"},
            { KeyPosition.Minus, "Minus"},
            { KeyPosition.Plus, "+"},
            { KeyPosition.Plus, "="},
            { KeyPosition.Plus, "Plus"},
            { KeyPosition.Backspace, "Backspace"},

            // Top row
            { KeyPosition.Tab, "Tab"},
            { KeyPosition.Q, "Q"},
            { KeyPosition.W, "W"},
            { KeyPosition.E, "E"},
            { KeyPosition.R, "R"},
            { KeyPosition.T, "T"},
            { KeyPosition.Y, "Y"},
            { KeyPosition.U, "U"},
            { KeyPosition.I, "I"},
            { KeyPosition.O, "O"},
            { KeyPosition.P, "P"},
            { KeyPosition.Bracket_L, "["},
            { KeyPosition.Bracket_R, "]"},
            { KeyPosition.Backslash, "\\"},
            { KeyPosition.Exclaim, "!"},
            { KeyPosition.QuestionMark, "?"},

            // Home row
            { KeyPosition.CapsLock, "CapsLock"},
            { KeyPosition.CapsLock, "Caps Lock"},

            { KeyPosition.A, "A"},
            { KeyPosition.S, "S"},
            { KeyPosition.D, "D"},
            { KeyPosition.F, "F"},
            { KeyPosition.G, "G"},
            { KeyPosition.H, "H"},
            { KeyPosition.J, "J"},
            { KeyPosition.K, "K"},
            { KeyPosition.L, "L"},
            { KeyPosition.Semicolon, ";"},
            { KeyPosition.Apastrophe, "'"},
            { KeyPosition.Enter, "Enter"},

            // generic modifiers
            { KeyPosition.Shift, "Shift"},
            { KeyPosition.Win, "Win"},
            { KeyPosition.Alt, "Alt"},
            { KeyPosition.Control, "Control"},
            { KeyPosition.Alt, "AltGr"},

            // Buttom row
            { KeyPosition.Shift_L, "Shift_L"},
            { KeyPosition.Iso, "Iso"},
            { KeyPosition.Z, "Z"},
            { KeyPosition.X, "X"},
            { KeyPosition.C, "C"},
            { KeyPosition.V, "V"},
            { KeyPosition.B, "B"},
            { KeyPosition.N, "N"},
            { KeyPosition.M, "M"},
            { KeyPosition.Comma, ","},
            { KeyPosition.Period, "."},
            { KeyPosition.Slash, "/"},
            { KeyPosition.Shift_R, "Shift_R"},

            // Space row
            { KeyPosition.Control_L, "Control_L"},
            { KeyPosition.Win_L, "Win_L"},
            { KeyPosition.Alt_L, "Alt_L"},
            { KeyPosition.Space, "Space"},
            { KeyPosition.Alt_R, "Alt_R"},
            { KeyPosition.Win_R, "Win_R"},
            { KeyPosition.Menu, "Menu"},
            { KeyPosition.Control_R, "Control_R"},

            // Function row
            { KeyPosition.Esc, "Escape"},
            { KeyPosition.Esc, "Esc"},
            { KeyPosition.F1, "F1"},
            { KeyPosition.F2, "F2"},
            { KeyPosition.F3, "F3"},
            { KeyPosition.F4, "F4"},
            { KeyPosition.F5, "F5"},
            { KeyPosition.F6, "F6"},
            { KeyPosition.F7, "F7"},
            { KeyPosition.F8, "F8"},
            { KeyPosition.F9, "F9"},
            { KeyPosition.F10, "F10"},
            { KeyPosition.F11, "F11"},
            { KeyPosition.F12, "F12"},
            { KeyPosition.PrintScreen, "PrintScreen"},
            { KeyPosition.ScrollLock, "ScrollLock"},
            { KeyPosition.Pause, "Pause"},

            // Movement
            { KeyPosition.Insert, "Insert"},
            { KeyPosition.Delete, "Delete"},
            { KeyPosition.Home, "Home"},
            { KeyPosition.End, "End"},
            { KeyPosition.PageUp, "PageUp"},
            { KeyPosition.PageDown, "PageDown"},
            { KeyPosition.Up, "Up"},
            { KeyPosition.Left, "Left"},
            { KeyPosition.Down, "Down"},
            { KeyPosition.Right, "Right"},

            // Numpad
            { KeyPosition.NumLock, "NumLock"},
            { KeyPosition.KP_Div, "KP_Div"},
            { KeyPosition.KP_Mult, "KP_Mult"},
            { KeyPosition.KP_Min, "KP_Min"},
            { KeyPosition.KP_7, "KP_7"},
            { KeyPosition.KP_8, "KP_8"},
            { KeyPosition.KP_9, "KP_9"},
            { KeyPosition.KP_Plus, "KP_Plus"},
            { KeyPosition.KP_4, "KP_4"},
            { KeyPosition.KP_5, "KP_5"},
            { KeyPosition.KP_6, "KP_6"},
            { KeyPosition.KP_Comma, "KP_Comma"},
            { KeyPosition.KP_1, "KP_1"},
            { KeyPosition.KP_2, "KP_2"},
            { KeyPosition.KP_3, "KP_3"},
            { KeyPosition.KP_Enter, "KP_Enter"},
            { KeyPosition.KP_0, "KP_0"},
            { KeyPosition.KP_Dec, "KP_Dec"},
            { KeyPosition.KP_Eq, "KP_Eq"},

            // Miscellaneous keys

            // Media control
            { KeyPosition.AudioPlay, "AudioPlay"},
            { KeyPosition.AudioPause, "AudioPause"},
            { KeyPosition.PlayPause, "PlayPause"},
            { KeyPosition.Previous, "Previous"},
            { KeyPosition.Next, "Next"},
            { KeyPosition.Stop, "Stop"},
            { KeyPosition.ToggleRepeat, "ToggleRepeat"},
            { KeyPosition.ToggleRandom, "ToggleRandom"},
            { KeyPosition.AudioRewind, "AudioRewind"},
            { KeyPosition.AudioForward, "AudioForward"},
            { KeyPosition.Mute, "Mute"},
            { KeyPosition.VolumeDown, "VolumeDown"},
            { KeyPosition.VolumeUp, "VolumeUp"},
            { KeyPosition.Eject, "Eject"},

            // Browser control
            { KeyPosition.Browser_Back, "Browser_Back"},
            { KeyPosition.Browser_Forward, "Browser_Forward"},
            { KeyPosition.Browser_Refresh, "Browser_Refresh"},
            { KeyPosition.Browser_Stop, "Browser_Stop"},
            { KeyPosition.Browser_Search, "Browser_Search"},
            { KeyPosition.Browser_Favorites, "Browser_Favorites"},

            // Applications
            { KeyPosition.Calculator, "Calculator"},
            { KeyPosition.MediaPlayer, "MediaPlayer"},
            { KeyPosition.Browser, "Browser"},
            { KeyPosition.Mail, "Mail"},
            { KeyPosition.Search, "Search"},
            { KeyPosition.Explorer, "Explorer"},
            { KeyPosition.WWW, "WWW"},
            { KeyPosition.MyComputer, "MyComputer"},
            { KeyPosition.Help, "Help"},
            { KeyPosition.Launch0, "Launch0"},
            { KeyPosition.Launch1, "Launch1"},
            { KeyPosition.Launch2, "Launch2"},
            { KeyPosition.Launch3, "Launch3"},
            { KeyPosition.Launch4, "Launch4"},
            { KeyPosition.Launch5, "Launch5"},
            { KeyPosition.Launch6, "Launch6"},
            { KeyPosition.Launch7, "Launch7"},
            { KeyPosition.Launch8, "Launch8"},
            { KeyPosition.Launch9, "Launch9"},
            { KeyPosition.LaunchA, "LaunchA"},
            { KeyPosition.LaunchB, "LaunchB"},
            { KeyPosition.LaunchC, "LaunchC"},
            { KeyPosition.LaunchD, "LaunchD"},
            { KeyPosition.LaunchE, "LaunchE"},
            { KeyPosition.LaunchF, "LaunchF"},

            // Power management
            { KeyPosition.Power, "Power"},
            { KeyPosition.Sleep, "Sleep"},
            { KeyPosition.Wake, "Wake"},
            { KeyPosition.BrightnessDown, "BrightnessDown"},
            { KeyPosition.BrightnessUp, "BrightnessUp"},

            // Extra function keys
            { KeyPosition.F13, "F13"},
            { KeyPosition.F14, "F14"},
            { KeyPosition.F15, "F15"},
            { KeyPosition.F16, "F16"},
            { KeyPosition.F17, "F17"},
            { KeyPosition.F18, "F18"},
            { KeyPosition.F19, "F19"},
            { KeyPosition.F20, "F20"},
            { KeyPosition.F21, "F21"},
            { KeyPosition.F22, "F22"},
            { KeyPosition.F23, "F23"},
            { KeyPosition.F24, "F24"},
            { KeyPosition.OEM_8, "!" },

            // International keys
            { KeyPosition.Hash, "Hash"},
            { KeyPosition.Ro, "Ro"},
            { KeyPosition.Yen, "Yen"},
            { KeyPosition.Muhenkan, "Muhenkan"},
            { KeyPosition.Muhenkan, "Mhen"},
            { KeyPosition.Henkan, "Henkan"},
            { KeyPosition.Henkan, "Henk"},
            { KeyPosition.Katakana, "Katakana"},
            { KeyPosition.Katakana, "Kana"}
            });
    }
}