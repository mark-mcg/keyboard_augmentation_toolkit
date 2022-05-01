using JetBrains.Annotations;
using KAT.KeyCodeMappings;
using KAT.Layouts;
using MiscUtil.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WindowsInput.Native;

namespace KAT
{
    public class EmojiKeyboardLayoutGenerator_v2 : KeyboardShortcutGenerator_v2
    {
        public bool BindPagesAsModifierKeys = false;
        protected override void Build_internal(KUIMapping collection)
        {
            base.Build_internal(collection);
            List<string> Animals = new List<string>();
            List<string> Faces = new List<string>();
            List<string> Food = new List<string>();
            List<string> Vehicles = new List<string>();

            Animals.AddRange(PopulateRange(400, 439, "1f"));
            Animals.AddRange(PopulateRange(980, 997, "1f"));

            Faces.AddRange(PopulateRange(600, 644, "1f"));
            Faces.AddRange(PopulateRange(910, 918, "1f"));
            Faces.AddRange(PopulateRange(493, 499, "1f"));

            Food.AddRange(PopulateRange(345, 383, "1f"));
            Food.AddRange(PopulateRange(950, 969, "1f"));

            Vehicles.AddRange(PopulateRange(680, 699, "1f"));
            Vehicles.AddRange(Populate(new string[] { "6eb", "6ec", "6f4", "6f5", "6f6", "6f7", "6f8" }, "1f"));

            List<KUIMapping> collections = new List<KUIMapping>();
            collections.Add(GenerateShortcutDefinitions(Animals, "Animals"));
            collections.Add(GenerateShortcutDefinitions(Faces, "Faces"));
            collections.Add(GenerateShortcutDefinitions(Food, "Food"));
            collections.Add(GenerateShortcutDefinitions(Vehicles, "Vehicles"));

            string[] labels = new string[] { "Animals", "Faces", "Food", "Vehicles" };
            // then build the controls for the root - this lets us select which of the collections to start viewing
            int i = 0;
            foreach (KeyPosition key in KeyPositionExtensions.NumericKeys.GetRange(1, collections.Count))
            {
                KKey location = new KKey(key);
                KUIElement element = KUIElementCollectionHelpers.CreateKeyKUIElement(collection, location);
                KUIElementCollectionHelpers.AddKeyTrigger(element, location, null);
                KUIElementCollectionHelpers.AddTextDisplay(element, labels[i]);
                KUICollectionAction collectionAction = element.gameObject.AddComponent<KUICollectionAction>();
                collectionAction.ClearMappingsOnActivation = true;
                collectionAction.action = CollectionActionType.SpecificMap;
                collectionAction.SpecificMapToActivate = collections[i];
                collections[i].transform.parent.transform.SetParent(collection.transform);
                i++;
            }

            // now we've built all the maps, set them up so they know their parents/next/previous maps
            collections.ForEach(x => x.RefreshCollection());
            collection.GetComponentsInChildren<KUIMapping>().ToList().ForEach(x => x.RefreshCollection());
        }

        /// <summary>
        /// Given a list of unicode IDs and a name this will generate multiple KeyMaps, each containing up to 26 emojis
        /// with the ability to scroll between the KeyboardMaps at the same level.
        /// </summary>
        /// <param name="unicodeIDs"></param>
        /// <param name="mappingName"></param>
        private KUIMapping GenerateShortcutDefinitions(List<string> unicodeIDs, string mappingName)
        {
            GameObject container = new GameObject(mappingName);
            // each shortcut page can have 26 emojis
            int pages = (int)Math.Ceiling((double)unicodeIDs.Count / 26);

            if (BindPagesAsModifierKeys)
                pages = 1;

            Debug.Log("Got " + unicodeIDs.Count + " emojis, fitting to " + pages + " pages with BindPagesAsModifierKeys=" + BindPagesAsModifierKeys);
            int currentID = 0;
            List<KKey> keys = KeyPositionExtensions.AlphaKeys.Select(x => new KKey(x)).ToList();
            List<KeyPosition> modifiersToUse = new List<KeyPosition>() { KeyPosition.Control, KeyPosition.Shift, KeyPosition.Alt };

            int currentPage = 0;
            KUIMapping[] emojiPages = new KUIMapping[pages];

            // build the keyboard maps for each page
            for (int i = 0; i < pages; i++)
            {
                emojiPages[i] = KUIElementCollectionHelpers.CreateNewCollection("Emojis page " + i);
                emojiPages[i].transform.SetParent(container.transform);

                if (pages > 1)
                {
                    KKey location = new KKey(KeyPosition.Left);
                    KUIElement element = KUIElementCollectionHelpers.CreateKeyKUIElement(emojiPages[i], location);
                    KUIElementCollectionHelpers.AddKeyTrigger(element, location, null);
                    KUIElementCollectionHelpers.AddTextDisplay(element, "Prev Page");
                    KUICollectionAction collectionAction = element.gameObject.AddComponent<KUICollectionAction>();
                    collectionAction.action = CollectionActionType.Previous;
                    collectionAction.ClearMappingsOnActivation = true;

                    location = new KKey(KeyPosition.Right);
                    element = KUIElementCollectionHelpers.CreateKeyKUIElement(emojiPages[i], location);
                    KUIElementCollectionHelpers.AddKeyTrigger(element, location, null);
                    KUIElementCollectionHelpers.AddTextDisplay(element, "Next Page");
                    collectionAction = element.gameObject.AddComponent<KUICollectionAction>();
                    collectionAction.action = CollectionActionType.Next;
                    collectionAction.ClearMappingsOnActivation = true;
                }

            }

            // then generate the shortcuts for each map
            foreach (string unicode in unicodeIDs)
            {
                KKey location = keys[currentID];
                List<KKey> modifiers = new List<KKey>();
                if (currentPage > 0 && BindPagesAsModifierKeys)
                {
                    modifiers.Add(new KKey(modifiersToUse[currentPage - 1]));                    
                }

                CreateEmojiElement(unicode, location, modifiers, emojiPages[BindPagesAsModifierKeys ? 0 : currentPage]);
                currentID++;

                if (currentID == 26)
                {
                    currentID = 0;
                    currentPage++;
                }
            }

            return emojiPages.First();
        }

        public KUIElement CreateEmojiElement(string unicode, KKey location, List<KKey> modifiers, KUIMapping collection)
        {
            //Debug.Log("currentID " + currentID + " keys" + keys.Count + " currentPage " + currentPage + " totalpages " + pages);
            //Debug.Log("Adding emoji unicode " + unicode + " to mapping " + emojiPages[currentPage]);
            KUIElement element = KUIElementCollectionHelpers.CreateKeyKUIElement(collection, location);
            KUIElementCollectionHelpers.AddKeyTrigger(element, location, modifiers, true);
            KUIElementCollectionHelpers.AddTextDisplay(element, unicode);
            KUIElementCollectionHelpers.AddSimulatedStringOutput(element, TextUtils.FormatText(unicode));
            return element;
        }

        /// <summary>
        /// TODO - rewrite this to work properly with hex values (e.g. 1f6eb)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public List<string> PopulateRange(int start, int end, string prefix)
        {
            List<string> temp = new List<string>();
            for (int i = start; i <= end; i++)
            {
                temp.Add(prefix + i);
            }
            return temp;
        }

        public List<string> Populate(string[] values, string prefix)
        {
            List<string> temp = new List<string>();
            foreach (string i in values)
            {
                temp.Add(prefix + i);
            }
            return temp;
        }

        protected override string MappingName()
        {
            return "Emoji Keyboard Layout" + (BindPagesAsModifierKeys ? "BindPagesAsModifierKeys=" + BindPagesAsModifierKeys+")" : "");
        }
    }
}