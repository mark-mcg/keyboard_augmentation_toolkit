using KAT.KeyCodeMappings;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class KLFCRoot
{
    public string fullName;
    public string name;
    public string description;
    public bool qwertyShortcuts;
    public string filter;
    public List<string> shiftLevels;
    public List<SingletonKeys> keys;
    public List<CustomDeadKeys> customDeadKeys;
    public List<KLFCRoot> variants;
    // Mods?
}

[Serializable]
public class SingletonKeys
{
    public string pos;
    public List<string> letters;
    public List<string> shiftLevels;
}

[Serializable]
public class CustomDeadKeys
{
    public string name;
    public string baseChar;
    public Dictionary<string, string> stringMap; 
}

/// <summary>
/// See https://github.com/39aldo39/klfc/blob/master/doc/layout.md
/// </summary>
public class KLFCMappingParser : MonoBehaviour
{
    public KLFCRoot ParseFromAsset(TextAsset asset)
    {
        return ParseFromJSON(asset.text);
    }

    public KLFCRoot ParseFromJSON(string text)
    {
        JsonTextReader reader = new JsonTextReader(new StringReader(text));
        return JsonSerializer.CreateDefault().Deserialize<KLFCRoot>(reader);
    }

}
