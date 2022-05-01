using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace KAT
{
    [System.Serializable]
    public class ContextMap
    {
        public string targetProcess;
        public string titleContains;
        public KUIMapping mapping;
    }

    /// <summary>
    /// Demonstrates how the keyboard map can be changed based on the current application/web context.
    /// 
    /// Can use the title to crudely switch within an application e.g. enabling showing the website url in the title
    /// bar of firefox means you can parse out and switch based on websites.
    /// </summary>
    public class KUIContextSwitcher : MonoBehaviour
    {
        public KUIMappingManager manager;

        public List<ContextMap> contextMaps = new List<ContextMap>();
        ActiveApplicationMonitor contextUtils;

        public void Awake()
        {
            contextUtils = FindObjectOfType<ActiveApplicationMonitor>();
            contextUtils.OnContextChanged += ContextUtils_OnContextChanged;
        }

        private void ContextUtils_OnContextChanged()
        {
            try
            {
                if (contextMaps != null)
                {
                    ContextMap context = contextMaps.First(
                        x => x.targetProcess.ToLower().Contains(contextUtils.CurrentWindowProcessName.ToLower()) &&
                            (x.titleContains.Length == 0 ||
                            x.titleContains.Length > 0 && contextUtils.CurrentWindowTitle.ToLower().Contains(x.titleContains.ToLower()))
                        );

                    if (manager != null)
                    {
                        if (context != null)
                        {
                            Debug.Log("Retrieved mapping for process " + context.targetProcess + " title contains " + context.titleContains);
                            manager.ActivateMapping(context.mapping);
                        }
                        else
                        {
                            // no mapping should be enabled
                            manager.ActivateMapping(null);
                        }
                    }
                    else
                    {
                        Debug.LogError("AugmentedKeyboardMapManager not set!");
                    }
                }
            }
            catch (Exception e) { }
        }
    }
}