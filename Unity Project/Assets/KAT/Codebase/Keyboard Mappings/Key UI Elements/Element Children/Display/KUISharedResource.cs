using Microsoft.Collections.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KUISharedResource 
{
    static MultiValueDictionary<Component, object> dictionary = new MultiValueDictionary<Component, object>();

    public static void NoteSharedResource(Component resource, object client)
    {
        if (!dictionary.Contains(resource, client))
            dictionary.Add(resource, client);
    }

    public static bool DoWeOwnSharedResource(Component resource, object client)
    {
        return dictionary.Contains(resource, client);
    }

    public static void ReleaseSharedResource(Component resource, object client)
    {
        dictionary.Remove(resource, client);
    }

    public static bool IsResourceClaimed(Component resource, object client)
    {
        bool doWeOwnResource = DoWeOwnSharedResource(resource, client);

        return dictionary.ContainsKey(resource) && (  dictionary[resource].Count > 0 && !doWeOwnResource );
    }
}
