using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KAT.KeyCodeMappings;
using KAT.Interaction;

namespace KAT.Layouts
{
    public class KUIElementCollectionHelpers
    {
        public static KUIMapping CreateNewCollection(string name)
        {
            GameObject blep = new GameObject("KUIElementCollection " + name);
            KUIMapping collectionParent = blep.AddComponent<KUIMapping>();
            return collectionParent;
        }

        public static KUIElement CreateKeyKUIElement(KUIMapping collection, KKey location)
        {
            GameObject keyGO = new GameObject(location.GetKeyName());
            keyGO.transform.SetParent(collection.transform);
            KUIElement kuiElement = keyGO.AddComponent<KUIElement>();
            kuiElement.MappingParent = collection;

            // apply a tag based on this prime key location
            KUIKeyLocationMetadata tag = keyGO.AddComponent<KUIKeyLocationMetadata>();
            tag.metadata.Add(location);

            return kuiElement;
        }

        public static void AddKeyTrigger(KUIElement element, KKey key, List<KKey> modifiers, bool consumeKeypress = false)
        {
            if (modifiers == null)
                modifiers = new List<KKey>();

            KUIKeyEventsTrigger trigger = element.gameObject.GetComponent<KUIKeyEventsTrigger>();
            if (trigger == null)
                trigger = element.gameObject.AddComponent<KUIKeyEventsTrigger>();

            trigger.consumeKeyPress = consumeKeypress;
            List<KeyEventTrigger> triggers = new List<KeyEventTrigger>();
            if (trigger.triggers != null)
                triggers.AddRange(trigger.triggers);

            KeyEventTrigger keyTrigger = new KeyEventTrigger();
            keyTrigger.key = key;
            keyTrigger.Modifiers = modifiers.ToArray();
            triggers.Add(keyTrigger);

            // if the key is an alpha key with a shift modifier, we also need a trigger for caps lock...
            if (KeyPositionExtensions.AlphaKeys.Contains(key.pos) && modifiers.Contains(new KKey(KeyPosition.Shift)) && modifiers.Count() == 1)
            {
                keyTrigger = new KeyEventTrigger();
                keyTrigger.key = key;
                keyTrigger.Modifiers = new KKey[1];
                keyTrigger.Modifiers[0] = new KKey(KeyPosition.CapsLock);
                triggers.Add(keyTrigger);
            }

            trigger.triggers = triggers.ToArray();
            element.gameObject.name = key.GetKeyName();
            if (modifiers.Count > 0)
                element.gameObject.name += " + (" + string.Join(",", modifiers) + ")";  
        }

        public static void AddTextDisplay(KUIElement element, string text)
        {
            KUIElementTextDisplay textDisplay = element.gameObject.GetComponent<KUIElementTextDisplay>();
            if (textDisplay == null)
                textDisplay = element.gameObject.AddComponent<KUIElementTextDisplay>();

            textDisplay.text.text = text;
        }

        public static void AddSimulatedKeypressOutput(KUIElement element, List<KKey> keys)
        {
            KeyPressOutputAction action = element.gameObject.AddComponent<KeyPressOutputAction>();
            action.keysToTrigger = keys;
        }

        public static void AddSimulatedStringOutput(KUIElement element, string output)
        {
            StringOutputAction action = element.gameObject.AddComponent<StringOutputAction>();
            action.TextToSend = output;
        }
    }
}
