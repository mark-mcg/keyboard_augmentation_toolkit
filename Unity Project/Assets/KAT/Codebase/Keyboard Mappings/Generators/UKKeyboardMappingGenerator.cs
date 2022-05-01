using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput.Native;
using System.Linq;
using KAT.KeyCodeMappings;
using System;
using KAT.Layouts;

namespace KAT
{
    public class UKKeyboardMappingGenerator : KeyboardShortcutGenerator_v2
    {
        protected override string MappingName()
        {
            return "UK Key Labels";
        }

        protected override void Build_internal(KUIMapping collection)
        {
            base.Build_internal(collection);
            List<KKey> Keys = ((IEnumerable<KeyPosition>)Enum.GetValues(typeof(KeyPosition))).Select(x => new KKey(x)).ToList();

            foreach (KKey kc in Keys)
            {
                List<KKey> modifiers = new List<KKey>();

                if (kc.pos.IsNumericNumberPad())
                {
                    modifiers.Add(new KKey(KeyPosition.NumLock));
                }

                // lower case key
                KUIElement element = KUIElementCollectionHelpers.CreateKeyKUIElement(collection, kc);
                KUIElementCollectionHelpers.AddKeyTrigger(element, kc, null);
                KUIElementCollectionHelpers.AddTextDisplay(element, kc.GetKeyName().ToLower());

                // shift state
                element = KUIElementCollectionHelpers.CreateKeyKUIElement(collection, kc);
                KUIElementCollectionHelpers.AddKeyTrigger(element, kc, new List<KKey>() { new KKey(KeyPosition.Shift) });

                string shiftText = kc.GetKeyName().ToUpper();
                switch (kc.pos)
                {
                    case KeyPosition.N0: shiftText = ")"; break;
                    case KeyPosition.N1: shiftText = "!"; break;
                    case KeyPosition.N2: shiftText = "\""; break;
                    case KeyPosition.N3: shiftText = "£"; break;
                    case KeyPosition.N4: shiftText = "$"; break;
                    case KeyPosition.N5: shiftText = "%"; break;
                    case KeyPosition.N6: shiftText = "^"; break;
                    case KeyPosition.N7: shiftText = "&"; break;
                    case KeyPosition.N8: shiftText = "*"; break;
                    case KeyPosition.N9: shiftText = "("; break;
                    case KeyPosition.Minus: shiftText = "_"; break;
                    case KeyPosition.Plus: shiftText = "+"; break;
                    case KeyPosition.Bracket_L: shiftText = "{"; break;
                    case KeyPosition.Bracket_R: shiftText = "}"; break;
                    case KeyPosition.Tilde: shiftText = "¬"; break;
                    case KeyPosition.Backslash: shiftText = "|"; break;
                    case KeyPosition.Semicolon: shiftText = ":"; break;
                    case KeyPosition.Apastrophe: shiftText = "@"; break;
                    case KeyPosition.Iso: shiftText = "Iso"; break;
                    case KeyPosition.Comma: shiftText = "<"; break;
                    case KeyPosition.Period: shiftText = ">"; break;
                    case KeyPosition.Slash: shiftText = "?"; break;
                }

                KUIElementCollectionHelpers.AddTextDisplay(element, shiftText);
            }
        }
    }
}