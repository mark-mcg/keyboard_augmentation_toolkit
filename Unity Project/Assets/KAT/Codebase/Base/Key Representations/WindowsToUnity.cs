using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BidirectionalMap;
using BiLookup;

namespace KAT.KeyCodeMappings
{
    public class WindowsToUnity
    {
        public static VirtualKeyCodes Reverse(KeyCode kc)
        {
            return Convert.ReverseFirst(kc);
        }

        public static KeyCode Forward(VirtualKeyCodes position)
        {
            return Convert.ForwardFirst(position);
        }

        public static BiLookup<VirtualKeyCodes, KeyCode> Convert = new BiLookup<VirtualKeyCodes, KeyCode>(
                new KeyValueList<VirtualKeyCodes, KeyCode>() {

                {VirtualKeyCodes.ADD,KeyCode.Plus},// 0x6B},//	Numpad +
                {VirtualKeyCodes.BACK,KeyCode.Backspace},// 0x08},//	Backspace
                {VirtualKeyCodes.CANCEL,KeyCode.Break},// 0x03},//	Break
                {VirtualKeyCodes.CLEAR,KeyCode.Clear},//0x0C},//	Clear
                {VirtualKeyCodes.DECIMAL,KeyCode.Numlock},//0x6E},//	Numpad.
                {VirtualKeyCodes.DIVIDE,KeyCode.KeypadDivide},//0x6F},//	Numpad /
                {VirtualKeyCodes.ESCAPE,KeyCode.Escape},//0x1B},//	Esc
                {VirtualKeyCodes.ICO_HELP,KeyCode.Help},//0xE3},//	IcoHlp

                {VirtualKeyCodes.VK_0,KeyCode.Alpha0},//0x30},// ('0')	0
                {VirtualKeyCodes.VK_1,KeyCode.Alpha1},//0x31},// ('1')	1
                {VirtualKeyCodes.VK_2,KeyCode.Alpha2},//0x32},// ('2')	2
                {VirtualKeyCodes.VK_3,KeyCode.Alpha3},//0x33},// ('3')	3
                {VirtualKeyCodes.VK_4,KeyCode.Alpha4},//0x34},// ('4')	4
                {VirtualKeyCodes.VK_5,KeyCode.Alpha5},//0x35},// ('5')	5
                {VirtualKeyCodes.VK_6,KeyCode.Alpha6},//0x36},// ('6')	6
                {VirtualKeyCodes.VK_7,KeyCode.Alpha7},//0x37},// ('7')	7
                {VirtualKeyCodes.VK_8,KeyCode.Alpha8},//0x38},// ('8')	8
                {VirtualKeyCodes.VK_9,KeyCode.Alpha9},//0x39},// ('9')	9
                {VirtualKeyCodes.VK_A,KeyCode.A},//0x41},// ('A')	A
                {VirtualKeyCodes.VK_B,KeyCode.B},//0x42},// ('B')	B
                {VirtualKeyCodes.VK_C,KeyCode.C},//0x43},// ('C')	C
                {VirtualKeyCodes.VK_D,KeyCode.D},//0x44},// ('D')	D
                {VirtualKeyCodes.VK_E,KeyCode.E},//0x45},// ('E')	E
                {VirtualKeyCodes.VK_F,KeyCode.F},//0x46},// ('F')	F
                {VirtualKeyCodes.VK_G,KeyCode.G},//0x47},// ('G')	G
                {VirtualKeyCodes.VK_H,KeyCode.H},//0x48},// ('H')	H
                {VirtualKeyCodes.VK_I,KeyCode.I},//0x49},// ('I')	I
                {VirtualKeyCodes.VK_J,KeyCode.J},//0x4A},// ('J')	J
                {VirtualKeyCodes.VK_K,KeyCode.K},//0x4B},// ('K')	K
                {VirtualKeyCodes.VK_L,KeyCode.L},//0x4C},// ('L')	L
                {VirtualKeyCodes.VK_M,KeyCode.M},//0x4D},// ('M')	M
                {VirtualKeyCodes.VK_N,KeyCode.N},//0x4E},// ('N')	N
                {VirtualKeyCodes.VK_O,KeyCode.O},//0x4F},// ('O')	O
                {VirtualKeyCodes.VK_P,KeyCode.P},//0x50},// ('P')	P
                {VirtualKeyCodes.VK_Q,KeyCode.Q},//0x51},// ('Q')	Q
                {VirtualKeyCodes.VK_R,KeyCode.R},//0x52},// ('R')	R
                {VirtualKeyCodes.VK_S,KeyCode.S},//0x53},// ('S')	S
                {VirtualKeyCodes.VK_T,KeyCode.T},//0x54},// ('T')	T
                {VirtualKeyCodes.VK_U,KeyCode.U},//0x55},// ('U')	U
                {VirtualKeyCodes.VK_V,KeyCode.V},//0x56},// ('V')	V
                {VirtualKeyCodes.VK_W,KeyCode.W},//0x57},// ('W')	W
                {VirtualKeyCodes.VK_X,KeyCode.X},//0x58},// ('X')	X
                {VirtualKeyCodes.VK_Y,KeyCode.Y},//0x59},// ('Y')	Y
                {VirtualKeyCodes.VK_Z,KeyCode.Z},// 0x5A},// ('Z')	Z

                {VirtualKeyCodes.MULTIPLY,KeyCode.KeypadMultiply},// 0x6A},//	Numpad*
                {VirtualKeyCodes.NONAME,KeyCode.None},// 0xFC},//	NoName

                {VirtualKeyCodes.NUMPAD0,KeyCode.Keypad0},//0x60},//	Numpad 0
                {VirtualKeyCodes.NUMPAD1,KeyCode.Keypad1},//0x61},//	Numpad 1
                {VirtualKeyCodes.NUMPAD2,KeyCode.Keypad2},//0x62},//	Numpad 2
                {VirtualKeyCodes.NUMPAD3,KeyCode.Keypad3},//0x63},//	Numpad 3
                {VirtualKeyCodes.NUMPAD4,KeyCode.Keypad4},//0x64},//	Numpad 4
                {VirtualKeyCodes.NUMPAD5,KeyCode.Keypad5},//0x65},//	Numpad 5
                {VirtualKeyCodes.NUMPAD6,KeyCode.Keypad6},//0x66},//	Numpad 6
                {VirtualKeyCodes.NUMPAD7,KeyCode.Keypad7},//0x67},//	Numpad 7
                {VirtualKeyCodes.NUMPAD8,KeyCode.Keypad8},//0x68},//	Numpad 8
                {VirtualKeyCodes.NUMPAD9,KeyCode.Keypad9},//0x69},//	Numpad 9

                {VirtualKeyCodes.OEM_1,KeyCode.Colon},// 0xBA},//	OEM_1(: },)
                {VirtualKeyCodes.OEM_102,KeyCode.Greater},//0xE2},//	OEM_102(> <)
                {VirtualKeyCodes.OEM_2,KeyCode.Question},//0xBF},//	OEM_2(? /)
                {VirtualKeyCodes.OEM_3,KeyCode.BackQuote},//0xC0},//	OEM_3(~ `)
                {VirtualKeyCodes.OEM_4,KeyCode.LeftBracket},//0xDB},//	OEM_4({ [)
                {VirtualKeyCodes.OEM_5,KeyCode.Backslash},//0xDC},//	OEM_5(| \)
                {VirtualKeyCodes.OEM_6,KeyCode.RightBracket},//0xDD},//	OEM_6(} ])
                {VirtualKeyCodes.OEM_7,KeyCode.Hash},//0xDE},//	OEM_7(" ')
                {VirtualKeyCodes.OEM_8,KeyCode.Exclaim},//0xDF},//	OEM_8 (§ !)
                {VirtualKeyCodes.OEM_ATTN,KeyCode.At},//0xF0},//	Oem Attn
                {VirtualKeyCodes.OEM_CLEAR,KeyCode.Clear},//0xFE},//	OemClr
                {VirtualKeyCodes.OEM_COMMA,KeyCode.Comma},//0xBC},//	OEM_COMMA(< ,)
                {VirtualKeyCodes.OEM_MINUS,KeyCode.Minus},//0xBD},//	OEM_MINUS(_ -)
                {VirtualKeyCodes.OEM_PERIOD,KeyCode.Period},//0xBE},//	OEM_PERIOD(> .)
                {VirtualKeyCodes.OEM_PLUS,KeyCode.Plus},//0xBB},//	OEM_PLUS(+ =)
                {VirtualKeyCodes.RETURN,KeyCode.Return},//0x0D},//	Enter
                {VirtualKeyCodes.SPACE,KeyCode.Space},//0x20},//	Space
                {VirtualKeyCodes.SUBTRACT,KeyCode.KeypadMinus},//0x6D},//	Num -
                {VirtualKeyCodes.TAB,KeyCode.Tab},//0x09},//	Tab

                {VirtualKeyCodes._none_,KeyCode.None},//0xFF},//	no VK mapping
                {VirtualKeyCodes.CAPITAL,KeyCode.CapsLock},//0x14},//	Caps Lock
                {VirtualKeyCodes.DELETE,KeyCode.Delete},//0x2E},//	Delete
                {VirtualKeyCodes.DOWN,KeyCode.DownArrow},//0x28},//	Arrow Down
                {VirtualKeyCodes.END,KeyCode.End},//0x23},//	End

                {VirtualKeyCodes.F1,KeyCode.F1},//0x70},//	F1
                {VirtualKeyCodes.F2,KeyCode.F2},//0x71},//	F2
                {VirtualKeyCodes.F3,KeyCode.F3},//0x72},//	F3
                {VirtualKeyCodes.F4,KeyCode.F4},//0x73},//	F4
                {VirtualKeyCodes.F5,KeyCode.F5},//0x74},//	F5
                {VirtualKeyCodes.F6,KeyCode.F6},//0x75},//	F6
                {VirtualKeyCodes.F7,KeyCode.F7},//0x76},//	F7
                {VirtualKeyCodes.F8,KeyCode.F8},//0x77},//	F8
                {VirtualKeyCodes.F9,KeyCode.F9},//0x78},//	F9
                {VirtualKeyCodes.F10,KeyCode.F10},//0x79},//	F10
                {VirtualKeyCodes.F11,KeyCode.F11},//0x7A},//	F11
                {VirtualKeyCodes.F12,KeyCode.F12},//0x7B},//	F12
                {VirtualKeyCodes.F13,KeyCode.F13},//0x7C},//	F13
                {VirtualKeyCodes.F14,KeyCode.F14},//0x7D},//	F14
                {VirtualKeyCodes.F15,KeyCode.F15},//0x7E},//	F15

                {VirtualKeyCodes.HELP,KeyCode.Help},//0x2F},//	Help
                {VirtualKeyCodes.HOME,KeyCode.Home},//0x24},//	Home
                {VirtualKeyCodes.INSERT,KeyCode.Insert},//0x2D},//	Insert
                {VirtualKeyCodes.LCONTROL,KeyCode.LeftControl},//0xA2},//	Left Ctrl
                {VirtualKeyCodes.LEFT,KeyCode.LeftArrow},//0x25},//	Arrow Left
                {VirtualKeyCodes.LMENU,KeyCode.LeftAlt},//0xA4},//	Left Alt
                {VirtualKeyCodes.LSHIFT,KeyCode.LeftShift},//0xA0},//	Left Shift
                {VirtualKeyCodes.LWIN,KeyCode.LeftWindows},//0x5B},//	Left Win
                {VirtualKeyCodes.NEXT,KeyCode.PageDown},//0x22},//	Page Down
                {VirtualKeyCodes.NUMLOCK,KeyCode.Numlock},//0x90},//	Num Lock
                {VirtualKeyCodes.PAUSE,KeyCode.Pause},//0x13},//	Pause
                {VirtualKeyCodes.PRINT,KeyCode.Print},//0x2A},//	Print
                {VirtualKeyCodes.PRIOR,KeyCode.PageUp},//0x21},//	Page Up
                {VirtualKeyCodes.RCONTROL,KeyCode.RightControl},//0xA3},//	Right Ctrl
                {VirtualKeyCodes.RIGHT,KeyCode.RightArrow},//0x27},//	Arrow Right
                {VirtualKeyCodes.RMENU,KeyCode.RightAlt},//0xA5},//	Right Alt
                {VirtualKeyCodes.RSHIFT,KeyCode.RightShift},//0xA1},//	Right Shift
                {VirtualKeyCodes.RWIN,KeyCode.RightWindows},//0x5C},//	Right Win
                {VirtualKeyCodes.SCROLL,KeyCode.ScrollLock},//0x91},//	Scrol Lock
                {VirtualKeyCodes.SNAPSHOT,KeyCode.SysReq},//0x2C},//	Print Screen
                {VirtualKeyCodes.UP,KeyCode.UpArrow},//0x26},//	Arrow Up
        });
    }
}