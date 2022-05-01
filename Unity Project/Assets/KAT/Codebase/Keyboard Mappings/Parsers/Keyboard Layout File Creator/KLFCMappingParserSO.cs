using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using KAT.KeyCodeMappings;

namespace KAT.Layouts
{
    [CreateAssetMenu(fileName = "KLFC Mapping Parser", menuName = "KAT/Mappings/KLFC Mapping Parser", order = 1)]
    public class KLFCMappingParserSO : ScriptableObject
    {
        public TextAsset JsonFile;
        public KLFCRoot parsedRoot;
        public int maxShiftLevels = 0;
        public bool consumeKeyPress = true;


        [Button]
        public void Parse()
        {
            KLFCMappingParser parser = new KLFCMappingParser();
            parsedRoot = parser.ParseFromAsset(JsonFile);
            KUIMapping collectionParent = KUIElementCollectionHelpers.CreateNewCollection("KLFC from "+ JsonFile.name + " parsed name " + parsedRoot.fullName);

            foreach (SingletonKeys key in parsedRoot.keys)
            {
                List<string> shiftLevels = new List<string>();
                if (parsedRoot.shiftLevels != null && parsedRoot.shiftLevels.Count > 0)
                    shiftLevels.AddRange(parsedRoot.shiftLevels);

                if (key.shiftLevels != null && key.shiftLevels.Count > 0)
                {
                    shiftLevels.Clear();
                    shiftLevels.AddRange(key.shiftLevels);
                }

                Debug.Log("shiftLevels contains " + shiftLevels.Count);

                // if we want to arbitrarily exclude some shift levels
                if (maxShiftLevels != 0 && shiftLevels.Count > maxShiftLevels)
                    shiftLevels.RemoveRange(shiftLevels.Count - (shiftLevels.Count - maxShiftLevels), shiftLevels.Count -maxShiftLevels);

                foreach (string shiftLevel in shiftLevels)
                {
                    // if there's a key letter assigned to this combo of key and modifier state...
                    if (key.letters.Count() > shiftLevels.IndexOf(shiftLevel))
                    {
                        Debug.Log("Creating key for shiftlevel " + shiftLevel + " position " + key.pos);
                        KKey keyPosition = new KKey(key.pos.GetKeyPosition());
                        List<KKey> modifierkeys = new List<KKey>();

                        if (shiftLevel != "None")
                        {
                            shiftLevel.Split('+').ToList().ForEach(x => modifierkeys.Add(new KKey(x.GetKeyPosition())));
                        }

                        KUIElement element = KUIElementCollectionHelpers.CreateKeyKUIElement(collectionParent, keyPosition);
                        KUIElementCollectionHelpers.AddKeyTrigger(element, keyPosition, modifierkeys, consumeKeyPress);
                        KUIElementCollectionHelpers.AddTextDisplay(element, key.letters[shiftLevels.IndexOf(shiftLevel)] + " ");
                        KUIElementCollectionHelpers.AddSimulatedStringOutput(element, key.letters[shiftLevels.IndexOf(shiftLevel)]);
                    }
                }
            }

        }
    }
}
