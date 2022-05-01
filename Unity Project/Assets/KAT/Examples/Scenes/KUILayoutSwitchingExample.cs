using KAT;
using KAT.Layouts;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KUILayoutSwitchingExample : MonoBehaviour
{
    public List<KUISerializedLayout> Layouts;
    public KUISerializedLayout current;

    public void Awake()
    {
        Invoke("NextLayout", 0.5f);
    }

    [Button]
    public void NextLayout()
    {
        if (Layouts.Count > 0)
        {
            if (current == null)
            {
                current = Layouts[0];
            }
            else
            {
                current = Layouts[(Layouts.IndexOf(current) + 1) % Layouts.Count];
            }

            if (current != null)
            {
                Debug.LogError("KUILayoutSwitchingTest: Changing layout to " + current);
                FindObjectOfType<KUILayoutManager>().SetLayout(current);
            }
        }
    }
}