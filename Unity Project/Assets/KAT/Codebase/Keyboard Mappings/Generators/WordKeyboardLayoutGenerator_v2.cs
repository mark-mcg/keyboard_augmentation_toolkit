using KAT.KeyCodeMappings;
using KAT.Layouts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput.Native;

namespace KAT
{
    public class WordKeyboardLayoutGenerator_v2 : KeyboardShortcutGenerator_v2
    {
        protected override string MappingName()
        {
            return "MS Word Icons";
        }

        public struct WordShortcut
        {
            public KeyPosition pos;
            public KeyPosition mod;
            public string sprite;
            public string tooltip;

            public WordShortcut(KeyPosition pos, KeyPosition modifier, string sprite, string tooltip)
            {
                this.pos = pos;
                this.mod = modifier;
                this.sprite = sprite;
                this.tooltip = tooltip;
            }
        }

        protected override void Build_internal(KUIMapping collection)
        {
            base.Build_internal(collection);
            List<WordShortcut> wordShortcuts = new List<WordShortcut>
            {
                new WordShortcut(KeyPosition.S, KeyPosition.Control, GetSpriteString("FileSave"), "Save"),
                new WordShortcut(KeyPosition.X, KeyPosition.Control, GetSpriteString("Cut"), "Cut"),
                new WordShortcut(KeyPosition.C, KeyPosition.Control, GetSpriteString("Copy"), "Copy"),
                new WordShortcut(KeyPosition.V, KeyPosition.Control, GetSpriteString("Paste"), "Paste"),
                new WordShortcut(KeyPosition.A, KeyPosition.Control, GetSpriteString("SelectAll"), "SelectAll"),
                new WordShortcut(KeyPosition.B, KeyPosition.Control, GetSpriteString("Bold"), "Bold"),
                new WordShortcut(KeyPosition.I, KeyPosition.Control, GetSpriteString("Italic"), "Italic"),
                new WordShortcut(KeyPosition.U, KeyPosition.Control, GetSpriteString("Underline"), "Underline"),
                new WordShortcut(KeyPosition.Bracket_L, KeyPosition.Control, GetSpriteString("FontSizeDecrease1Point"), "Decrease font by 1 point"),
                new WordShortcut(KeyPosition.Bracket_R, KeyPosition.Control, GetSpriteString("FontSizeIncrease1Point"), "Increase font by 1 point"),
                new WordShortcut(KeyPosition.Esc, KeyPosition.Control, GetSpriteString("CancelRequest"), "Cancel"),
                new WordShortcut(KeyPosition.Z, KeyPosition.Control, GetSpriteString("Undo"), "Undo"),
                new WordShortcut(KeyPosition.Y, KeyPosition.Control, GetSpriteString("Redo"), "Redo"),
                new WordShortcut(KeyPosition.N, KeyPosition.Control, GetSpriteString("NewOfficeDocument"), "Create a new document"),
                new WordShortcut(KeyPosition.O, KeyPosition.Control, GetSpriteString("FileOpen"), "Open a document"),
                new WordShortcut(KeyPosition.W, KeyPosition.Control, GetSpriteString("XDPPClose"), "Close a document"),
                new WordShortcut(KeyPosition.P, KeyPosition.Control, GetSpriteString("FilePrint"), "Print a document"),
                new WordShortcut(KeyPosition.F, KeyPosition.Control, GetSpriteString("SearchUI"), "Open the search box"),
                new WordShortcut(KeyPosition.H, KeyPosition.Control, GetSpriteString("ReplaceDialog"), "Replace text"),
                new WordShortcut(KeyPosition.G, KeyPosition.Control, GetSpriteString("GoTo"), "Go to a page"),
                new WordShortcut(KeyPosition.D, KeyPosition.Control, GetSpriteString("FontSchemes"), "Font dialog"),
                new WordShortcut(KeyPosition.E, KeyPosition.Control, GetSpriteString("AlignCenter"), "Align center/left"),
                new WordShortcut(KeyPosition.J, KeyPosition.Control, GetSpriteString("AlignJustify"), "Align justify/left"),
                new WordShortcut(KeyPosition.L, KeyPosition.Control, GetSpriteString("AlignLeft"), "Align left"),
                new WordShortcut(KeyPosition.M, KeyPosition.Control, GetSpriteString("ParagraphIndentLeft"), "Indent a paragraph left"),

                new WordShortcut(KeyPosition.PageDown, KeyPosition.Control, GetSpriteString("PreviousPage"), "To the top of previous page"),
                new WordShortcut(KeyPosition.PageUp, KeyPosition.Control, GetSpriteString("NextPage"), "To the top of next page"),
                new WordShortcut(KeyPosition.End, KeyPosition.Control, GetSpriteString("EndOfDocument"), "End of document"),
                new WordShortcut(KeyPosition.Home, KeyPosition.Control, GetSpriteString("StartOfDocument"), "Beginning of document"),
                new WordShortcut(KeyPosition.Left, KeyPosition.Control, GetSpriteString("PagePreviousWord"), "One word to the left"),
                new WordShortcut(KeyPosition.Right, KeyPosition.Control, GetSpriteString("PageNextWord"), "One word to the right"),
                //new WordShortcut(KeyPosition.Up, KeyPosition.Control, GetSpriteString(""), "One paragraph up"),
                //new WordShortcut(KeyPosition.Down, KeyPosition.Control, GetSpriteString(""), "One paragraph down"),

                new WordShortcut(KeyPosition.F1, KeyPosition.Control, GetSpriteString("Help"), "Help"),
                //new WordShortcut(KeyPosition.F2, KeyPosition.Control, GetSpriteString(""), "Move (cut and paste)"),
                new WordShortcut(KeyPosition.F4, KeyPosition.Control, GetSpriteString("Repeat"), "Repeat last action"),
                new WordShortcut(KeyPosition.F5, KeyPosition.Control, GetSpriteString("GoTo"), "GoTo"),
                new WordShortcut(KeyPosition.F6, KeyPosition.Control, GetSpriteString("Next"), "Next Page"),
                new WordShortcut(KeyPosition.F7, KeyPosition.Control, GetSpriteString("SpellingMenu"), "Spelling"),

                new WordShortcut(KeyPosition.F8, KeyPosition.Control, GetSpriteString("Group"), "Extend selection"),
                new WordShortcut(KeyPosition.F9, KeyPosition.Control, GetSpriteString("FieldsUpdate"), "Update selected fields"),
                new WordShortcut(KeyPosition.F10, KeyPosition.Control, GetSpriteString("HelpKeyboardShortcuts"), "Show keytips"),
                new WordShortcut(KeyPosition.F11, KeyPosition.Control, GetSpriteString("PageNext"), "Go to next field"),
                new WordShortcut(KeyPosition.F12, KeyPosition.Control, GetSpriteString("FileSaveAs"), "Save as"),
            };

            foreach (WordShortcut shortcut in wordShortcuts)
            {
                KKey location = new KKey(shortcut.pos);
                KKey modifier = new KKey(shortcut.mod);

                KUIElement element = KUIElementCollectionHelpers.CreateKeyKUIElement(collection, location);
                KUIElementCollectionHelpers.AddKeyTrigger(element, location, new List<KKey>() { modifier });
                KUIElementCollectionHelpers.AddTextDisplay(element, shortcut.sprite);
            }
        }

        private string GetSpriteString(string name)
        {
            return string.Format("<sprite=\"office\" name=\"{0}\">", name);
        }
    }
}