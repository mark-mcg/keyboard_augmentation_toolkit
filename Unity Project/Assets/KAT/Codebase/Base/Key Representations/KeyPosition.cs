using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BidirectionalMap;
using System;
using System.Linq;

/// <summary>
/// Platform-independent key positions. This is the internal representation we use for keys - platforms should be converted to this.
/// 
/// Based on the stellar work from: https://github.com/39aldo39/klfc/blob/master/src/Layout/Pos.hs
/// </summary>
namespace KAT.KeyCodeMappings
{
    [Serializable]
    public enum KeyPosition
    {
        // for range checks, put these in actual order
        N0, N1, N2, N3, N4, N5, N6, N7, N8, N9,
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,
        F13, F14, F15, F16, F17, F18, F19, F20, F21, F22, F23, F24,
        KP_0, KP_1, KP_2, KP_3, KP_4, KP_5, KP_6, KP_7, KP_8, KP_9

        , NumLock, KP_Div, KP_Mult, KP_Min, KP_Plus, KP_Comma, KP_Enter, KP_Dec, KP_Eq,
        OEM_8,

                Tilde, Minus, Plus, Backspace
        , Tab, Bracket_L, Bracket_R, Backslash
        , CapsLock, Semicolon, Apastrophe, Enter, Exclaim, QuestionMark
        , Shift_L, Iso, Comma, Period, Slash, Shift_R
        , Control_L, Win_L, Alt_L, Space, Alt_R, Win_R, Menu, Control_R, Shift, Win, Alt, Control

        , Esc, PrintScreen, ScrollLock, Pause
        , Insert, Delete, Home, End, PageUp, PageDown, Up, Left, Down, Right

        , AudioPlay, AudioPause, PlayPause, Previous, Next, Stop, ToggleRepeat, ToggleRandom
        , AudioRewind, AudioForward
        , Mute, VolumeDown, VolumeUp
        , Eject

        , Browser_Back, Browser_Forward, Browser_Refresh, Browser_Stop, Browser_Search, Browser_Favorites

        , Calculator, MediaPlayer, Browser, Mail, Search, Explorer, WWW, MyComputer, Help
        , Launch0, Launch1, Launch2, Launch3, Launch4, Launch5, Launch6, Launch7
        , Launch8, Launch9, LaunchA, LaunchB, LaunchC, LaunchD, LaunchE, LaunchF

        , Power, Sleep, Wake, BrightnessDown, BrightnessUp

        , Hash, Ro, Yen, Muhenkan, Henkan, Katakana
    }

    public static class KeyPositionExtensions
    {
        public static bool Equals(this KeyPosition pos, KeyPosition other, bool fuzzyModifierMatch = false, bool fuzzyNumericMatch = false)
        {
            if (fuzzyModifierMatch)
            {
                // if it's a modifier key, match its symmetrical siblings
                if (pos.IsModifierControl())
                    return other.IsModifierControl();
                if (pos.IsModifierAlt())
                    return other.IsModifierAlt();
                if (pos.IsModifierShift())
                    return other.IsModifierShift();
                if (pos.IsModifiersWin())
                     return other.IsModifiersWin();
            }

            if (fuzzyNumericMatch)
            {
                return pos.IsSameNumber(other);
            }

            return pos.Equals(other);
        }

        public static bool IsKeyPressed(this KeyPosition position)
        {
            return KUIKeyboardState.IsKeyPressed(position);
        }

        public static KeyPosition GetKeyPosition(this string str)
        {
            return KeyPositionToString.Reverse(str);
        }

        public static bool OneOf(this KeyPosition position,  List<KeyPosition> keyPositions)
        {
            if (keyPositions == null || keyPositions.Count() == 0)
                return false;
            return keyPositions.Any(x => x.Equals(position));
        }

        public static bool OneOf(this KeyPosition position, params KeyPosition[] keyPositions)
        {
            if (keyPositions == null || keyPositions.Count() == 0)
                return false;
            return keyPositions.Any(x => x.Equals(position));
        }

        public static bool InRange(this KeyPosition position, KeyPosition start, KeyPosition end)
        {
            return (int)position >= (int)start && (int)position <= (int)end;
        }

        public static bool IsNumericNumberPad(this KeyPosition position)
        {
            return position.OneOf(NumericNumberPad);
        }

        public static bool IsNumberPadKey(this KeyPosition position)
        {
            return position.OneOf(NumberPad);

        }

        public static bool IsNumeric(this KeyPosition position, bool includeNumberPad = false)
        {
            if (!includeNumberPad)
                return position.InRange(KeyPosition.N0, KeyPosition.N9);
            else
                return position.InRange(KeyPosition.N0, KeyPosition.N9) || position.IsNumericNumberPad();
        }

        public static bool IsAlpha(this KeyPosition position)
        {
            return position.InRange(KeyPosition.A, KeyPosition.Z);
        }
        public static bool IsAlphaNumeric(this KeyPosition position, bool includeNumberPad = false)
        {
            return position.IsAlpha() || position.IsNumeric(includeNumberPad);
        }
        public static bool IsFunctionKey(this KeyPosition position, bool includeExtended)
        {
            return position.InRange(KeyPosition.F1, includeExtended? KeyPosition.F24 : KeyPosition.F12);
        }
        public static bool IsModifier(this KeyPosition position)
        {
            return position.OneOf(ModifiersAll);
        }
        public static bool IsModifierAction(this KeyPosition position)
        {
            return position.OneOf(ModifiersActions);
        }

        public static bool IsModifierAlt(this KeyPosition position)
        {
            return position.OneOf(ModifiersAlt);
        }

        public static bool IsModifierShift(this KeyPosition position)
        {
            return position.OneOf(ModifiersShift);
        }
        public static bool IsModifierControl(this KeyPosition position)
        {
            return position.OneOf(ModifiersControl);
        }
        public static bool IsModifierMenu(this KeyPosition position)
        {
            return position.OneOf(ModifiersMenu);
        }

        public static bool IsModifiersWin(this KeyPosition position)
        {
            return position.OneOf(ModifiersWinKeys);
        }

        public static bool IsModifierToggle(this KeyPosition position)
        {
            return position.OneOf(ModifiersToggles);
        }

        /// <summary>
        /// Downshifts any number pad numerics to the number key representation
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static KeyPosition DownshiftNumber(this KeyPosition position)
        {
            return position.IsNumericNumberPad() ?
                    (KeyPosition)((int)position - ((int)KeyPosition.KP_0 - (int)KeyPosition.N0)) :
                    position;
        }

        public static bool IsSameNumber(this KeyPosition position, KeyPosition other)
        {
            return position.DownshiftNumber().Equals(other.DownshiftNumber());
        }



        public static List<KeyPosition> GetRange(params (KeyPosition, KeyPosition)[] ranges)
        {
            return GetRange(ranges.Select(x => ((int)x.Item1, (int)x.Item2)).ToArray());
        }

        public static List<KeyPosition> GetRange(params (int, int)[] ranges)
        {
            List<KeyPosition> keyPositions = new List<KeyPosition>();

            foreach ((int, int) range in ranges)
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    keyPositions.Add((KeyPosition)i);
                }
            }
            return keyPositions;
        }

        public static readonly List<KeyPosition> NumericNumberPad = new List<KeyPosition>() {
                KeyPosition.KP_7,KeyPosition. KP_8, KeyPosition.KP_9
                , KeyPosition.KP_4, KeyPosition.KP_5, KeyPosition.KP_6
                , KeyPosition.KP_1, KeyPosition.KP_2, KeyPosition.KP_3
                , KeyPosition.KP_0,
        };

        public static readonly List<KeyPosition> NumberPad = new List<KeyPosition>() {
                KeyPosition.NumLock, KeyPosition.KP_Div, KeyPosition.KP_Mult, KeyPosition.KP_Min
                , KeyPosition.KP_7,KeyPosition. KP_8, KeyPosition.KP_9, KeyPosition.KP_Plus
                , KeyPosition.KP_4, KeyPosition.KP_5, KeyPosition.KP_6, KeyPosition.KP_Comma
                , KeyPosition.KP_1, KeyPosition.KP_2, KeyPosition.KP_3, KeyPosition.KP_Enter
                , KeyPosition.KP_0, KeyPosition.KP_Dec, KeyPosition.KP_Eq,
        };

        public static readonly List<KeyPosition> NumericKeys = new List<KeyPosition>() {
            KeyPosition.N0,
            KeyPosition.N1,
            KeyPosition.N2,
            KeyPosition.N3,
            KeyPosition.N4,
            KeyPosition.N5,
            KeyPosition.N6,
            KeyPosition.N7,
            KeyPosition.N8,
            KeyPosition.N9,
        };

        public static readonly List<KeyPosition> AlphaKeys = new List<KeyPosition>()
        {
            KeyPosition.A,
            KeyPosition.B,
            KeyPosition.C,
            KeyPosition.D,
            KeyPosition.E,
            KeyPosition.F,
            KeyPosition.G,
            KeyPosition.H,
            KeyPosition.I,
            KeyPosition.J,
            KeyPosition.K,
            KeyPosition.L,
            KeyPosition.M,
            KeyPosition.N,
            KeyPosition.O,
            KeyPosition.P,
            KeyPosition.Q,
            KeyPosition.R,
            KeyPosition.S,
            KeyPosition.T,
            KeyPosition.U,
            KeyPosition.V,
            KeyPosition.W,
            KeyPosition.X,
            KeyPosition.Y,
            KeyPosition.Z,
        };

        public static readonly List<KeyPosition> AlphaNumeric = new List<KeyPosition>()
        {
            KeyPosition.A,
            KeyPosition.B,
            KeyPosition.C,
            KeyPosition.D,
            KeyPosition.E,
            KeyPosition.F,
            KeyPosition.G,
            KeyPosition.H,
            KeyPosition.I,
            KeyPosition.J,
            KeyPosition.K,
            KeyPosition.L,
            KeyPosition.M,
            KeyPosition.N,
            KeyPosition.O,
            KeyPosition.P,
            KeyPosition.Q,
            KeyPosition.R,
            KeyPosition.S,
            KeyPosition.T,
            KeyPosition.U,
            KeyPosition.V,
            KeyPosition.W,
            KeyPosition.X,
            KeyPosition.Y,
            KeyPosition.Z,
            KeyPosition.N0,
            KeyPosition.N1,
            KeyPosition.N2,
            KeyPosition.N3,
            KeyPosition.N4,
            KeyPosition.N5,
            KeyPosition.N6,
            KeyPosition.N7,
            KeyPosition.N8,
            KeyPosition.N9,
        };

        public static readonly List<KeyPosition> NormalFunctionKeys = new List<KeyPosition>()
        {
            KeyPosition.F1,
            KeyPosition.F2,
            KeyPosition.F3,
            KeyPosition.F4,
            KeyPosition.F5,
            KeyPosition.F6,
            KeyPosition.F7,
            KeyPosition.F8,
            KeyPosition.F9,
            KeyPosition.F10,
            KeyPosition.F11,
            KeyPosition.F12
        };

        public static readonly List<KeyPosition> ExtendedFunctionKeys = new List<KeyPosition>()
        {
            KeyPosition.F1,
            KeyPosition.F2,
            KeyPosition.F3,
            KeyPosition.F4,
            KeyPosition.F5,
            KeyPosition.F6,
            KeyPosition.F7,
            KeyPosition.F8,
            KeyPosition.F9,
            KeyPosition.F10,
            KeyPosition.F11,
            KeyPosition.F12,
            KeyPosition.F13,
            KeyPosition.F14,
            KeyPosition.F15,
            KeyPosition.F16,
            KeyPosition.F17,
            KeyPosition.F18,
            KeyPosition.F19,
            KeyPosition.F20,
            KeyPosition.F21,
            KeyPosition.F22,
            KeyPosition.F23,
            KeyPosition.F24
        };

        public static readonly List<KeyPosition> ModifiersActions = new List<KeyPosition>()
        {
                KeyPosition.Shift_L,
                KeyPosition.Shift_R,
                KeyPosition.Shift,
                KeyPosition.Menu,
                KeyPosition.Control_L,
                KeyPosition.Control_R,
                KeyPosition.Control,
        };


        public static readonly List<KeyPosition> ModifiersShift = new List<KeyPosition>()
        {
                KeyPosition.Shift_L,
                KeyPosition.Shift_R,
                KeyPosition.Shift
        };


        public static readonly List<KeyPosition> ModifiersControl = new List<KeyPosition>()
        {
                KeyPosition.Control_L,
                KeyPosition.Control_R,
                KeyPosition.Control
        };


        public static readonly List<KeyPosition> ModifiersMenu = new List<KeyPosition>()
        {
                KeyPosition.Menu,
                KeyPosition.Alt_L,
                KeyPosition.Alt_R,
                KeyPosition.Alt
        };

        public static readonly List<KeyPosition> ModifiersAlt = new List<KeyPosition>()
        {
                KeyPosition.Alt_L,
                KeyPosition.Alt_R,
                KeyPosition.Alt
        };

        public static readonly List<KeyPosition> ModifiersToggles = new List<KeyPosition>()
        {
            KeyPosition.CapsLock,
            KeyPosition.NumLock,
        };

        public static readonly List<KeyPosition> ModifiersWinKeys = new List<KeyPosition>()
        {
            KeyPosition.Win_L,
            KeyPosition.Win_R,
            KeyPosition.Win
        };

        public static readonly List<KeyPosition> ModifiersAll = new List<KeyPosition>()
        {
            KeyPosition.CapsLock,
            KeyPosition.NumLock,
            KeyPosition.Menu,
            KeyPosition.Alt_L,
            KeyPosition.Alt_R,
            KeyPosition.Alt,
            KeyPosition.Shift_L,
            KeyPosition.Shift_R,
            KeyPosition.Shift,
            KeyPosition.Menu,
            KeyPosition.Control_L,
            KeyPosition.Control_R,
            KeyPosition.Control
        };
    }
}