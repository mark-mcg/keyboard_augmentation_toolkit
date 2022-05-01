using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class UtilityExtensions
{
    public static T[] GetComponentsInImmediateChildren<T>(this MonoBehaviour script) where T : class
    {
        List<T> group = new List<T>();

        //collect only if its an interface or a Component
        if (typeof(T).IsInterface
         || typeof(T).IsSubclassOf(typeof(Component))
         || typeof(T) == typeof(Component))
        {
            foreach (Transform child in script.transform)
            {
                group.AddRange(child.GetComponents<T>());
            }
        }

        return group.ToArray();
    }

    public static T[] GetComponentsInImmediateChildren<T>(this GameObject script) where T : class
    {
        //Debug.Log("Getting components for " + script.name); 
        List<T> group = new List<T>();

        //collect only if its an interface or a Component
        if (typeof(T).IsInterface
         || typeof(T).IsSubclassOf(typeof(Component))
         || typeof(T) == typeof(Component))
        {
            //Debug.Log("Searching children");
            foreach (Transform child in script.transform)
            {
                //Debug.Log("Checking child " + child.gameObject.name);
                group.AddRange(child.GetComponents<T>());
            }
        }

        return group.ToArray();
    }
}