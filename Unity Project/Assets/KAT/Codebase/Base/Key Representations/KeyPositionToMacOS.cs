using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BidirectionalMap;
using BiLookup;

namespace KAT.KeyCodeMappings
{
    public class KeyPositionToMacOS 
    {
        public static KeyPosition Reverse(int KLE)
        {
            return Convert.ReverseFirst(KLE);
        }

        public static int Forward(KeyPosition position)
        {
            return Convert.ForwardFirst(position);
        }

        public static  BiLookup<KeyPosition, int> Convert = new BiLookup<KeyPosition, int>(
            new KeyValueList<KeyPosition, int>() {

            {KeyPosition.Tilde, 0x0A},  //0x32
            {KeyPosition.N1, 0x12}, 
            {KeyPosition.N2, 0x13}, 
            {KeyPosition.N3, 0x14}, 
            {KeyPosition.N4, 0x15}, 
            {KeyPosition.N5, 0x17}, 
            {KeyPosition.N6, 0x16}, 
            {KeyPosition.N7, 0x1A}, 
            {KeyPosition.N8, 0x1C}, 
            {KeyPosition.N9, 0x19}, 
            {KeyPosition.N0, 0x1D}, 
            {KeyPosition.Minus, 0x1B}, 
            {KeyPosition.Plus, 0x18}, 
            {KeyPosition.Backspace, 0x33}, 

            {KeyPosition.Tab, 0x30}, 
            {KeyPosition.Q, 0x0C}, 
            {KeyPosition.W, 0x0D}, 
            {KeyPosition.E, 0x0E}, 
            {KeyPosition.R, 0x0F}, 
            {KeyPosition.T, 0x11}, 
            {KeyPosition.Y, 0x10}, 
            {KeyPosition.U, 0x20}, 
            {KeyPosition.I, 0x22}, 
            {KeyPosition.O, 0x1F}, 
            {KeyPosition.P, 0x23}, 
            {KeyPosition.Bracket_L, 0x21}, 
            {KeyPosition.Bracket_R, 0x1E}, 
            {KeyPosition.Backslash, 0x2A}, 

            {KeyPosition.CapsLock, 0x39}, 
            {KeyPosition.A, 0x00}, 
            {KeyPosition.S, 0x01}, 
            {KeyPosition.D, 0x02}, 
            {KeyPosition.F, 0x03}, 
            {KeyPosition.G, 0x05}, 
            {KeyPosition.H, 0x04}, 
            {KeyPosition.J, 0x26}, 
            {KeyPosition.K, 0x28}, 
            {KeyPosition.L, 0x25}, 
            {KeyPosition.Semicolon, 0x29}, 
            {KeyPosition.Apastrophe, 0x27}, 
            {KeyPosition.Enter, 0x24}, 

            {KeyPosition.Shift_L, 0x38}, 
            {KeyPosition.Iso, 0x32}, 
            {KeyPosition.Z, 0x06}, 
            {KeyPosition.X, 0x07}, 
            {KeyPosition.C, 0x08}, 
            {KeyPosition.V, 0x09}, 
            {KeyPosition.B, 0x0B}, 
            {KeyPosition.N, 0x2D}, 
            {KeyPosition.M, 0x2E}, 
            {KeyPosition.Comma, 0x2B}, 
            {KeyPosition.Period, 0x2F}, 
            {KeyPosition.Slash, 0x2C}, 
            {KeyPosition.Shift_R, 0x3C}, 

        //    {KeyPosition.Control_L, 0x3B}, 
        //    {KeyPosition.Alt_L, 0x3A}, 
        //    {KeyPosition.Win_L, 0x37}, 
            {KeyPosition.Space, 0x31}, 
        //    {KeyPosition.Win_R, 0x37}, 
        //    {KeyPosition.Alt_R, 0x3D}, 
            //{KeyPosition.Menu, 0x}, 
        //    {KeyPosition.Control_R, 0x3E}, 

            {KeyPosition.Esc, 0x35}, 
            {KeyPosition.F1, 0x7A}, 
            {KeyPosition.F2, 0x78}, 
            {KeyPosition.F3, 0x63}, 
            {KeyPosition.F4, 0x76}, 
            {KeyPosition.F5, 0x60}, 
            {KeyPosition.F6, 0x61}, 
            {KeyPosition.F7, 0x62}, 
            {KeyPosition.F8, 0x64}, 
            {KeyPosition.F9, 0x65}, 
            {KeyPosition.F10, 0x6D}, 
            {KeyPosition.F11, 0x67}, 
            {KeyPosition.F12, 0x6F}, 
            {KeyPosition.PrintScreen, 0x69}, 
            {KeyPosition.ScrollLock, 0x6B}, 
            {KeyPosition.Pause, 0x71}, 

            {KeyPosition.Insert, 0x72}, 
            {KeyPosition.Delete, 0x75}, 
            {KeyPosition.Home, 0x73}, 
            {KeyPosition.End, 0x77}, 
            {KeyPosition.PageUp, 0x74}, 
            {KeyPosition.PageDown, 0x79}, 
            {KeyPosition.Up, 0x7E}, 
            {KeyPosition.Left, 0x7B}, 
            {KeyPosition.Down, 0x7D}, 
            {KeyPosition.Right, 0x7C}, 

            {KeyPosition.NumLock, 0x47}, 
            {KeyPosition.KP_Div, 0x4B}, 
            {KeyPosition.KP_Mult, 0x43}, 
            {KeyPosition.KP_7, 0x59}, 
            {KeyPosition.KP_8, 0x5B}, 
            {KeyPosition.KP_9, 0x5C}, 
            {KeyPosition.KP_Min, 0x4E}, 
            {KeyPosition.KP_4, 0x56}, 
            {KeyPosition.KP_5, 0x57}, 
            {KeyPosition.KP_6, 0x58}, 
            {KeyPosition.KP_Plus, 0x45}, 
            {KeyPosition.KP_1, 0x53}, 
            {KeyPosition.KP_2, 0x54}, 
            {KeyPosition.KP_3, 0x55}, 
            {KeyPosition.KP_0, 0x52}, 
            {KeyPosition.KP_Dec, 0x41}, 
            {KeyPosition.KP_Enter, 0x4C}, 
            {KeyPosition.KP_Eq, 0x51}, 
        });
    }
}
