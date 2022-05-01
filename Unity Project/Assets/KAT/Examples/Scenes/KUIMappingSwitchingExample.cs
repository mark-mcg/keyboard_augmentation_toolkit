using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KAT;
using NaughtyAttributes;

public class KUIMappingSwitchingExample : MonoBehaviour
{
    public List<KUIMapping> Mappings = new List<KUIMapping>();
    public KUIMapping current;

    public void Start()
    {
        if (Mappings.Count == 0)
        {
            foreach (Transform trans in this.transform)
            {
                Mappings.AddRange(trans.GetComponents<KUIMapping>());
            }
        }
    }

    bool firstUpdate = true;
    public void Update()
    {
        if (firstUpdate)
        {
            NextMapping();
            firstUpdate = false;
        }
    }

    [Button]
    public void NextMapping()
    {
        if (Mappings.Count > 0)
        {
            if (current == null)
            {
                current = Mappings[0];
            }
            else
            {
                current = Mappings[(Mappings.IndexOf(current) + 1) % Mappings.Count];
            }

            if (current != null)
            {
                Debug.LogError("KUIMappingSwitchingTest: Changing mapping to " + current);
                FindObjectOfType<KUIMappingManager>().ActivateMapping(current, true);
            }
        }
    }
}
