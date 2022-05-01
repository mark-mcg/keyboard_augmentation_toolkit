﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BidirectionalMap;
using BiLookup;

namespace KAT.KeyCodeMappings
{
    public class KeyPositionToLinux
    {
        public static KeyPosition Reverse(int code)
        {
            return Convert.ReverseFirst(code);
        }

        public static int Forward(KeyPosition position)
        {
            return Convert.ForwardFirst(position);
        }

        public static BiLookup<KeyPosition, int> Convert = new BiLookup<KeyPosition, int>(
                        new KeyValueList<KeyPosition, int>() {

            {KeyPosition.Tilde, 0x29}, 
            {KeyPosition.N1, 0x02}, 
            {KeyPosition.N2, 0x03}, 
            {KeyPosition.N3, 0x04}, 
            {KeyPosition.N4, 0x05}, 
            {KeyPosition.N5, 0x06}, 
            {KeyPosition.N6, 0x07}, 
            {KeyPosition.N7, 0x08}, 
            {KeyPosition.N8, 0x09}, 
            {KeyPosition.N9, 0x0A}, 
            {KeyPosition.N0, 0x0B}, 
            {KeyPosition.Minus, 0x0C}, 
            {KeyPosition.Plus, 0x0D}, 
            {KeyPosition.Backspace, 0x0E}, 

            {KeyPosition.Tab, 0x0F}, 
            {KeyPosition.Q, 0x10}, 
            {KeyPosition.W, 0x11}, 
            {KeyPosition.E, 0x12}, 
            {KeyPosition.R, 0x13}, 
            {KeyPosition.T, 0x14}, 
            {KeyPosition.Y, 0x15}, 
            {KeyPosition.U, 0x16}, 
            {KeyPosition.I, 0x17}, 
            {KeyPosition.O, 0x18}, 
            {KeyPosition.P, 0x19}, 
            {KeyPosition.Bracket_L, 0x1A}, 
            {KeyPosition.Bracket_R, 0x1B}, 
            {KeyPosition.Backslash, 0x2B}, 

            {KeyPosition.CapsLock, 0x3A}, 
            {KeyPosition.A, 0x1E}, 
            {KeyPosition.S, 0x1F}, 
            {KeyPosition.D, 0x20}, 
            {KeyPosition.F, 0x21}, 
            {KeyPosition.G, 0x22}, 
            {KeyPosition.H, 0x23}, 
            {KeyPosition.J, 0x24}, 
            {KeyPosition.K, 0x25}, 
            {KeyPosition.L, 0x26}, 
            {KeyPosition.Semicolon, 0x27}, 
            {KeyPosition.Apastrophe, 0x28}, 
            {KeyPosition.Enter, 0x1C}, 

            {KeyPosition.Shift_L, 0x2A}, 
            {KeyPosition.Iso, 0x56}, 
            {KeyPosition.Z, 0x2C}, 
            {KeyPosition.X, 0x2D}, 
            {KeyPosition.C, 0x2E}, 
            {KeyPosition.V, 0x2F}, 
            {KeyPosition.B, 0x30}, 
            {KeyPosition.N, 0x31}, 
            {KeyPosition.M, 0x32}, 
            {KeyPosition.Comma, 0x33}, 
            {KeyPosition.Period, 0x34}, 
            {KeyPosition.Slash, 0x35}, 
            {KeyPosition.Shift_R, 0x36}, 

            {KeyPosition.Control_L, 0x1D}, 
            {KeyPosition.Win_L, 0x7D}, 
            {KeyPosition.Alt_L, 0x38}, 
            {KeyPosition.Space, 0x39}, 
            {KeyPosition.Alt_R, 0x64}, 
            {KeyPosition.Win_R, 0x7E}, 
            {KeyPosition.Menu, 0x7F}, 
            {KeyPosition.Control_R, 0x61}, 

            {KeyPosition.Esc, 0x01}, 
            {KeyPosition.F1, 0x3B}, 
            {KeyPosition.F2, 0x3C}, 
            {KeyPosition.F3, 0x3D}, 
            {KeyPosition.F4, 0x3E}, 
            {KeyPosition.F5, 0x3F}, 
            {KeyPosition.F6, 0x40}, 
            {KeyPosition.F7, 0x41}, 
            {KeyPosition.F8, 0x42}, 
            {KeyPosition.F9, 0x43}, 
            {KeyPosition.F10, 0x44}, 
            {KeyPosition.F11, 0x57}, 
            {KeyPosition.F12, 0x58}, 
            {KeyPosition.PrintScreen, 0x63}, 
            {KeyPosition.ScrollLock, 0x36}, 
            {KeyPosition.Pause, 0x77}, 

            {KeyPosition.Insert, 0x6E}, 
            {KeyPosition.Delete, 0x6F}, 
            {KeyPosition.Home, 0x56}, 
            {KeyPosition.End, 0x6B}, 
            {KeyPosition.PageUp, 0x68}, 
            {KeyPosition.PageDown, 0x6D}, 
            {KeyPosition.Up, 0x67}, 
            {KeyPosition.Left, 0x6A}, 
            {KeyPosition.Down, 0x6C}, 
            {KeyPosition.Right, 0x69}, 

            {KeyPosition.NumLock, 0x45}, 
            {KeyPosition.KP_Div, 0x62}, 
            {KeyPosition.KP_Mult, 0x37}, 
            {KeyPosition.KP_Min, 0x4A}, 
            {KeyPosition.KP_7, 0x47}, 
            {KeyPosition.KP_8, 0x48}, 
            {KeyPosition.KP_9, 0x49}, 
            {KeyPosition.KP_Plus, 0x4E}, 
            {KeyPosition.KP_4, 0x4B}, 
            {KeyPosition.KP_5, 0x4C}, 
            {KeyPosition.KP_6, 0x4D}, 
            {KeyPosition.KP_Comma, 0x79}, 
            {KeyPosition.KP_1, 0x4F}, 
            {KeyPosition.KP_2, 0x50}, 
            {KeyPosition.KP_3, 0x51}, 
            {KeyPosition.KP_Enter, 0x60}, 
            {KeyPosition.KP_0, 0x52}, 
            {KeyPosition.KP_Dec, 0x53}, 
            {KeyPosition.KP_Eq, 0x75}, 

            {KeyPosition.AudioPlay, 0xCF}, 
            {KeyPosition.AudioPlay, 0xC8}, 
            {KeyPosition.AudioPause, 0xC9}, 
            {KeyPosition.PlayPause, 0xA4}, 
            {KeyPosition.Previous, 0xA5}, 
            {KeyPosition.Next, 0xA3}, 
            {KeyPosition.Stop, 0xA6}, 
        //    {KeyPosition.ToggleRepeat, 0x}, 
        //    {KeyPosition.ToggleRandom, 0x}, 
            {KeyPosition.AudioRewind, 0xA8}, 
            {KeyPosition.AudioForward, 0xD0}, 
            {KeyPosition.Mute, 0x71}, 
            {KeyPosition.VolumeDown, 0x72}, 
            {KeyPosition.VolumeUp, 0x73}, 
            {KeyPosition.BrightnessDown, 0xE5}, 
            {KeyPosition.BrightnessUp, 0xE6}, 
            {KeyPosition.Eject, 0xA1}, 

            {KeyPosition.Browser_Back, 0x9E}, 
            {KeyPosition.Browser_Forward, 0x9F}, 
            {KeyPosition.Browser_Refresh, 0xAD}, 
            {KeyPosition.Browser_Stop, 0x80}, 
            {KeyPosition.Browser_Search, 0xD9}, 
            {KeyPosition.Browser_Favorites, 0x9C}, 

            {KeyPosition.Calculator, 0x8C}, 
            {KeyPosition.MediaPlayer, 0xE2}, 
            {KeyPosition.Browser, 0xAC}, 
            {KeyPosition.Mail, 0x9B}, 
            {KeyPosition.Search, 0xD9}, 
            {KeyPosition.Explorer, 0x90}, 
            {KeyPosition.MyComputer, 0x96}, 
            {KeyPosition.WWW, 0x9D}, 
            {KeyPosition.Help, 0x8A}, 
        //    {KeyPosition.Launch0, 0x}, 
            {KeyPosition.Launch1, 0x94}, 
            {KeyPosition.Launch2, 0x95}, 
            {KeyPosition.Launch3, 0xCA}, 
            {KeyPosition.Launch4, 0xCB}, 
            {KeyPosition.Launch5, 0xC0}, 
            {KeyPosition.Launch6, 0xC1}, 
            {KeyPosition.Launch7, 0xC2}, 
            {KeyPosition.Launch8, 0xC3}, 
            {KeyPosition.Launch9, 0xC4}, 
            {KeyPosition.LaunchA, 0x78}, 
            {KeyPosition.LaunchB, 0xCC}, 
        //    {KeyPosition.LaunchC, 0x}, 
        //    {KeyPosition.LaunchD, 0x}, 
        //    {KeyPosition.LaunchE, 0x}, 
        //    {KeyPosition.LaunchF, 0x}, 

            {KeyPosition.Power, 0xA0}, 
            {KeyPosition.Sleep, 0x8E}, 
            {KeyPosition.Wake, 0x8F}, 

            {KeyPosition.F13, 0xB7}, 
            {KeyPosition.F14, 0xB8}, 
            {KeyPosition.F15, 0xB9}, 
            {KeyPosition.F16, 0xBA}, 
            {KeyPosition.F17, 0xBB}, 
            {KeyPosition.F18, 0xBC}, 
            {KeyPosition.F19, 0xBD}, 
            {KeyPosition.F20, 0xBE}, 
            {KeyPosition.F21, 0xBF}, 
            {KeyPosition.F22, 0xC0}, 
            {KeyPosition.F23, 0xC1}, 
            {KeyPosition.F24, 0xC2}, 
        //    {KeyPosition.Hash, 0x}, 
            {KeyPosition.Ro, 0x59}, 
            {KeyPosition.Yen, 0x7C}, 
            {KeyPosition.Muhenkan, 0x5E}, 
            {KeyPosition.Henkan, 0x5C}, 
            {KeyPosition.Katakana, 0x5B}, 
        });
    }
}
