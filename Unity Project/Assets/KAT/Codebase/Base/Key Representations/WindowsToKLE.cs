using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BidirectionalMap;
using BiLookup;

namespace KAT.KeyCodeMappings
{
    public class WindowsToKLE
    {
        public static VirtualKeyCodes Reverse(string value)
        {
            return Convert.ReverseFirst(value);
        }

        public static string Forward(VirtualKeyCodes position)
        {
            return Convert.ForwardFirst(position);
        }

        /// <summary>
        /// As KLE keys are just labels, we can try to extract the underlying key based on the label + row position
        /// </summary>
        /// <param name="KLEText"></param>
        /// <param name="positionOnRow"></param>
        /// <returns></returns>
        public static VirtualKeyCodes Reverse(string KLEText, int positionOnRow)
        {
            if (ContainsAny(KLEText, "Shift"))
            {
                if (positionOnRow < 2) KLEText = "LeftShift"; else KLEText = "RightShift";
            }

            if (ContainsAny(KLEText, "Ctrl"))
            {
                if (positionOnRow < 2) KLEText = "LeftCtrl"; else KLEText = "RightCtrl";
            }

            if (ContainsAny(KLEText, "Alt"))
            {
                if (positionOnRow < 4) KLEText = "LeftAlt"; else KLEText = "RightAlt";
            }

            if (ContainsAny(KLEText, "Ins"))
            {
                KLEText = "Insert";
            }

            if (ContainsAny(KLEText, "Del"))
            {
                KLEText = "Delete";
            }


            if (ContainsAny(KLEText, "Windows", "Win"))
            {
                if (positionOnRow < 4) KLEText = "LeftWindows"; else KLEText = "RightWindows";
            }

            if (ContainsAny(KLEText, "Numlock", "Num Lock", "NumLock"))
            {
                KLEText = "Num Lock";
            }

            if (ContainsAny(KLEText, "Caps Lock", "CpsLck"))
            {
                KLEText = "Caps Lock";
            }

            if ((ContainsAny(KLEText, " ") && KLEText.Length == 1) || KLEText.Length == 0)
            {
                KLEText = "Space";
            }

            if (ContainsAny(KLEText, "/"))
            {
                if (positionOnRow > 12) KLEText = "Divide";
            }

            if (ContainsAny(KLEText, "-"))
            {
                if (positionOnRow > 12) KLEText = "Min";
            }

            int numberpadNo;
            if (int.TryParse(KLEText, out numberpadNo) && positionOnRow > 12)
            {
                KLEText = "Numpad" + KLEText;
            }

            // Debug.Log("KLEToWindows: Checking for key " + KLEText);
            return Convert.ReverseFirst(KLEText);
        }

        public static bool ContainsAny(string haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }

        public static BiLookup<VirtualKeyCodes, string> Convert = new BiLookup<VirtualKeyCodes, string>(
                                    new KeyValueList<VirtualKeyCodes, string>() {

                {VirtualKeyCodes.ADD,"+"},
                {VirtualKeyCodes.BACK,"Backspace"},
                {VirtualKeyCodes.CANCEL,"Break"},
                {VirtualKeyCodes.CLEAR,"Clear"},
                {VirtualKeyCodes.DECIMAL,"Numlock"},
                {VirtualKeyCodes.DIVIDE,"Divide"},
                {VirtualKeyCodes.ESCAPE,"Esc"},
                {VirtualKeyCodes.ICO_HELP,"Help"},

                {VirtualKeyCodes.VK_0,"0"},
                {VirtualKeyCodes.VK_1,"1"},
                {VirtualKeyCodes.VK_2,"2"},
                {VirtualKeyCodes.VK_3,"3"},
                {VirtualKeyCodes.VK_4,"4"},
                {VirtualKeyCodes.VK_5,"5"},
                {VirtualKeyCodes.VK_6,"6"},
                {VirtualKeyCodes.VK_7,"7"},
                {VirtualKeyCodes.VK_8,"8"},
                {VirtualKeyCodes.VK_9,"9"},
                {VirtualKeyCodes.VK_A,"A"},
                {VirtualKeyCodes.VK_B,"B"},
                {VirtualKeyCodes.VK_C,"C"},
                {VirtualKeyCodes.VK_D,"D"},
                {VirtualKeyCodes.VK_E,"E"},
                {VirtualKeyCodes.VK_F,"F"},
                {VirtualKeyCodes.VK_G,"G"},
                {VirtualKeyCodes.VK_H,"H"},
                {VirtualKeyCodes.VK_I,"I"},
                {VirtualKeyCodes.VK_J,"J"},
                {VirtualKeyCodes.VK_K,"K"},
                {VirtualKeyCodes.VK_L,"L"},
                {VirtualKeyCodes.VK_M,"M"},
                {VirtualKeyCodes.VK_N,"N"},
                {VirtualKeyCodes.VK_O,"O"},
                {VirtualKeyCodes.VK_P,"P"},
                {VirtualKeyCodes.VK_Q,"Q"},
                {VirtualKeyCodes.VK_R,"R"},
                {VirtualKeyCodes.VK_S,"S"},
                {VirtualKeyCodes.VK_T,"T"},
                {VirtualKeyCodes.VK_U,"U"},
                {VirtualKeyCodes.VK_V,"V"},
                {VirtualKeyCodes.VK_W,"W"},
                {VirtualKeyCodes.VK_X,"X"},
                {VirtualKeyCodes.VK_Y,"Y"},
                {VirtualKeyCodes.VK_Z,"Z"},

                {VirtualKeyCodes.MULTIPLY,"*"},
                {VirtualKeyCodes.NONAME,"None"},

                {VirtualKeyCodes.NUMPAD0,"Numpad0"},
                {VirtualKeyCodes.NUMPAD1,"Numpad1"},
                {VirtualKeyCodes.NUMPAD2,"Numpad2"},
                {VirtualKeyCodes.NUMPAD3,"Numpad3"},
                {VirtualKeyCodes.NUMPAD4,"Numpad4"},
                {VirtualKeyCodes.NUMPAD5,"Numpad5"},
                {VirtualKeyCodes.NUMPAD6,"Numpad6"},
                {VirtualKeyCodes.NUMPAD7,"Numpad7"},
                {VirtualKeyCodes.NUMPAD8,"Numpad8"},
                {VirtualKeyCodes.NUMPAD9,"Numpad9"},

                // redone using http://www.kbdedit.com/manual/low_level_vk_list.html
                {VirtualKeyCodes.OEM_1,";"},//KeyCode.Colon},// 0xBA},//	OEM_1(: },)
                {VirtualKeyCodes.OEM_2,"/"}, //KeyCode.Question},//0xBF},//	OEM_2(? /)
                {VirtualKeyCodes.OEM_3,"`"},// KeyCode.BackQuote},//0xC0},//	OEM_3(~ `)
                {VirtualKeyCodes.OEM_4,"["},//  KeyCode.LeftBracket},//0xDB},//	OEM_4({ [)
                {VirtualKeyCodes.OEM_5,"\\"},//KeyCode.Backslash},//0xDC},//	OEM_5(| \)
                {VirtualKeyCodes.OEM_6,"]"},// KeyCode.RightBracket},//0xDD},//	OEM_6(} ])
                {VirtualKeyCodes.OEM_7,"'"},// KeyCode.Hash},//0xDE},//	OEM_7(" ')
                {VirtualKeyCodes.OEM_8,"!"}, // KeyCode.Exclaim},//0xDF},//	OEM_8 (§ !)
                {VirtualKeyCodes.OEM_102,"<"},//KeyCode.Greater},//0xE2},//	OEM_102(> <)

                // overloads
                {VirtualKeyCodes.OEM_7,"#"},// KeyCode.Hash},//0xDE},//	OEM_7(" ')


                //{VirtualKeyCodes.OEM_1,";"},//KeyCode.Colon},// 0xBA},//	OEM_1(: },)
                //{VirtualKeyCodes.OEM_7,"'"},// KeyCode.Hash},//0xDE},//	OEM_7(" ')

                //{VirtualKeyCodes.OEM_1,"},"},//KeyCode.Colon},// 0xBA},//	OEM_1(: },)
                //{VirtualKeyCodes.OEM_102,"."},//KeyCode.Greater},//0xE2},//	OEM_102(> <)
                //{VirtualKeyCodes.OEM_2,"/"}, //KeyCode.Question},//0xBF},//	OEM_2(? /)
                //{VirtualKeyCodes.OEM_3,"`"},// KeyCode.BackQuote},//0xC0},//	OEM_3(~ `)
                //{VirtualKeyCodes.OEM_4,"["},//  KeyCode.LeftBracket},//0xDB},//	OEM_4({ [)
                //{VirtualKeyCodes.OEM_5,"\\"},//KeyCode.Backslash},//0xDC},//	OEM_5(| \)
                //{VirtualKeyCodes.OEM_6,"]"},// KeyCode.RightBracket},//0xDD},//	OEM_6(} ])
                //{VirtualKeyCodes.OEM_7,"#"},// KeyCode.Hash},//0xDE},//	OEM_7(" ')
                //{VirtualKeyCodes.OEM_8,"!"}, // KeyCode.Exclaim},//0xDF},//	OEM_8 (§ !)
                {VirtualKeyCodes.OEM_ATTN,"'"},// KeyCode.At},//0xF0},//	Oem Attn
                {VirtualKeyCodes.OEM_CLEAR,"Clear"},// KeyCode.Clear},//0xFE},//	OemClr
                {VirtualKeyCodes.OEM_COMMA,","},// KeyCode.Comma},//0xBC},//	OEM_COMMA(< ,)
                {VirtualKeyCodes.OEM_MINUS,"-"},// KeyCode.Minus},//0xBD},//	OEM_MINUS(_ -)

                {VirtualKeyCodes.OEM_PERIOD,"."}, // KeyCode.Period},//0xBE},//	OEM_PERIOD(> .)
                {VirtualKeyCodes.OEM_PLUS,"="},// KeyCode.Plus},//0xBB},//	OEM_PLUS(+ =)
                {VirtualKeyCodes.RETURN,"Enter"},// KeyCode.Return},//0x0D},//	Enter
                {VirtualKeyCodes.SPACE,"Space"},// KeyCode.Space},//0x20},//	Space
                {VirtualKeyCodes.SUBTRACT,"Min"}, // KeyCode.KeypadMinus},//0x6D},//	Num -
                {VirtualKeyCodes.TAB,"Tab"},// KeyCode.Tab},//0x09},//	Tab

                {VirtualKeyCodes._none_,"None"},// KeyCode.None},//0xFF},//	no VK mapping
                {VirtualKeyCodes.CAPITAL,"Caps Lock"}, // KeyCode.CapsLock},//0x14},//	Caps Lock
                {VirtualKeyCodes.CAPITAL,"CapsLock"}, // KeyCode.CapsLock},//0x14},//	Caps Lock

                {VirtualKeyCodes.DELETE,"Delete"},// KeyCode.Delete},//0x2E},//	Delete
                {VirtualKeyCodes.DOWN,"↓"},// KeyCode.DownArrow},//0x28},//	Arrow Down
                {VirtualKeyCodes.END,"End"},// KeyCode.End},//0x23},//	End

                {VirtualKeyCodes.F1,"F1"},// KeyCode.F1},//0x70},//	F1
                {VirtualKeyCodes.F2,"F2"},// KeyCode.F2},//0x71},//	F2
                {VirtualKeyCodes.F3,"F3"},// KeyCode.F3},//0x72},//	F3
                {VirtualKeyCodes.F4,"F4"},// KeyCode.F4},//0x73},//	F4
                {VirtualKeyCodes.F5,"F5"},// KeyCode.F5},//0x74},//	F5
                {VirtualKeyCodes.F6,"F6"},// KeyCode.F6},//0x75},//	F6
                {VirtualKeyCodes.F7,"F7"},// KeyCode.F7},//0x76},//	F7
                {VirtualKeyCodes.F8,"F8"},// KeyCode.F8},//0x77},//	F8
                {VirtualKeyCodes.F9,"F9"},// KeyCode.F9},//0x78},//	F9
                {VirtualKeyCodes.F10,"F10"},// KeyCode.F10},//0x79},//	F10
                {VirtualKeyCodes.F11,"F11"},// KeyCode.F11},//0x7A},//	F11
                {VirtualKeyCodes.F12,"F12"},// KeyCode.F12},//0x7B},//	F12
                {VirtualKeyCodes.F13,"F13"},// KeyCode.F13},//0x7C},//	F13
                {VirtualKeyCodes.F14,"F14"},// KeyCode.F14},//0x7D},//	F14
                {VirtualKeyCodes.F15,"F15"},// KeyCode.F15},//0x7E},//	F15

                {VirtualKeyCodes.HELP,"Help"},// KeyCode.Help},//0x2F},//	Help
                {VirtualKeyCodes.HOME,"Home"},// KeyCode.Home},//0x24},//	Home
                {VirtualKeyCodes.INSERT,"Insert"},// KeyCode.Insert},//0x2D},//	Insert
                {VirtualKeyCodes.LCONTROL,"LeftCtrl"},// KeyCode.LeftControl},//0xA2},//	Left Ctrl
                {VirtualKeyCodes.LEFT,"←"},// KeyCode.LeftArrow},//0x25},//	Arrow Left
                {VirtualKeyCodes.LMENU,"LeftAlt"},// KeyCode.LeftAlt},//0xA4},//	Left Alt
                {VirtualKeyCodes.LSHIFT,"LeftShift"},// KeyCode.LeftShift},//0xA0},//	Left Shift
                {VirtualKeyCodes.LWIN,"LeftWindows"},// KeyCode.LeftWindows},//0x5B},//	Left Win
                {VirtualKeyCodes.NEXT,"PgDn"},// KeyCode.PageDown},//0x22},//	Page Down
                {VirtualKeyCodes.NUMLOCK,"Num Lock"},// KeyCode.Numlock},//0x90},//	Num Lock
                {VirtualKeyCodes.PAUSE,"Pause"},// KeyCode.Pause},//0x13},//	Pause
                {VirtualKeyCodes.PRINT,"Print"},// KeyCode.Print},//0x2A},//	Print
                {VirtualKeyCodes.PRIOR,"PgUp"},// KeyCode.PageUp},//0x21},//	Page Up
                {VirtualKeyCodes.RCONTROL,"RightCtrl"},// KeyCode.RightControl},//0xA3},//	Right Ctrl
                {VirtualKeyCodes.RIGHT,"→"},// KeyCode.RightArrow},//0x27},//	Arrow Right
                {VirtualKeyCodes.RMENU,"RightAlt"},// KeyCode.RightAlt},//0xA5},//	Right Alt
                {VirtualKeyCodes.RSHIFT,"RightShift"},// KeyCode.RightShift},//0xA1},//	Right Shift
                {VirtualKeyCodes.RWIN,"RightWindows"},// KeyCode.RightWindows},//0x5C},//	Right Win
                {VirtualKeyCodes.SCROLL,"Scroll Lock"},// KeyCode.ScrollLock},//0x91},//	Scrol Lock
                {VirtualKeyCodes.SNAPSHOT,"PrtSc"},// KeyCode.SysReq},//0x2C},//	Print Screen
                {VirtualKeyCodes.UP,"↑"},// KeyCode.UpArrow},//0x26},//	Arrow Up
                {VirtualKeyCodes.APPS,"Menu"},
        });
    }
}