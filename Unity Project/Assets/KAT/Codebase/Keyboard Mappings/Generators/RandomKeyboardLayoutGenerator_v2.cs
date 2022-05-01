using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using WindowsInput.Native;
using KAT.KeyCodeMappings;
using KAT.Layouts;

namespace KAT
{
    public static class IListExtensions
    {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }

    /// <summary>
    /// Creates a random(ish) alpha-numeric keyboard. The Alpha bits are hardcoded, so it's not truely random
    /// but good enough for an evaluation of the concept.
    /// </summary>
    public class RandomKeyboardLayoutGenerator_v2 : KeyboardShortcutGenerator_v2
    {
        protected override string MappingName()
        {
            return "Random AlphaNumeric Qwerty Keyboard Layout";
        }


        public void AddRandomizedRangeToCollection(KUIMapping collection, List<KKey> ToRandomize)
        {
            List<KKey> PotentialLocations;
            KKey[] TempLocations = new KKey[ToRandomize.Count];
            ToRandomize.CopyTo(TempLocations);
            PotentialLocations = TempLocations.ToList();
            PotentialLocations.Shuffle();

            Dictionary<KeyPosition, KeyPosition> AssignedKeys = new Dictionary<KeyPosition, KeyPosition>();

            for (int i = 0; i < ToRandomize.Count; i++)
            {
                // lower case key
                KUIElement element = KUIElementCollectionHelpers.CreateKeyKUIElement(collection, PotentialLocations[i]);
                KUIElementCollectionHelpers.AddKeyTrigger(element, PotentialLocations[i], null, true);
                KUIElementCollectionHelpers.AddTextDisplay(element, ToRandomize[i].GetKeyName().ToLower());
                KUIElementCollectionHelpers.AddSimulatedStringOutput(element, ToRandomize[i].GetKeyName().ToLower());
                Debug.Log("Added lower case " + ToRandomize[i].GetKeyName().ToLower());

                // upper case key (if applicable)
                if (PotentialLocations[i].pos.IsAlpha() && ToRandomize[i].pos.IsAlpha())
                {
                    element = KUIElementCollectionHelpers.CreateKeyKUIElement(collection, PotentialLocations[i]);
                    KUIElementCollectionHelpers.AddKeyTrigger(element, PotentialLocations[i], new List<KKey>() { new KKey(KeyPosition.Shift) }, true);
                    KUIElementCollectionHelpers.AddTextDisplay(element, ToRandomize[i].GetKeyName().ToUpper());
                    KUIElementCollectionHelpers.AddSimulatedStringOutput(element, ToRandomize[i].GetKeyName().ToUpper());
                    Debug.Log("Added upper case " + ToRandomize[i].GetKeyName().ToUpper());
                }
            }
        }


        protected override void Build_internal(KUIMapping collection)
        {
            base.Build_internal(collection);
            List<KKey> ToRandomize = new List<KKey>();

            // For the keys we are randomizing, we're going to iterate through them 
            // selecting a new location. This will be done for both standard and shift-modified alpha keys.
            // (so for example "h" might produce "z", whilst "shift-h" might produce X (no relation between shift and non-shift event)

            // 97 = A, 122 = Z
            ToRandomize.AddRange(KeyPositionExtensions.AlphaKeys.Select(x => new KKey(x)));
            AddRandomizedRangeToCollection(collection, ToRandomize);

            ToRandomize.Clear();
            ToRandomize.AddRange(KeyPositionExtensions.NumericKeys.Select(x => new KKey(x)));
            AddRandomizedRangeToCollection(collection, ToRandomize);
        }
    }
}