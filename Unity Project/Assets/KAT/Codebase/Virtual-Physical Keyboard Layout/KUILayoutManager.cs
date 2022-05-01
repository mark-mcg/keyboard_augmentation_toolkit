using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KAT.Interaction;
using KAT.Layouts;
using NaughtyAttributes;

namespace KAT
{
    public class KUILayoutManager : MonoBehaviour
    {
        [BoxGroup("Debug")]
        public bool LogEvents = false;
        [BoxGroup("Debug")]
        public KUISerializedLayout currentKeyboardLayout;
        protected KUIManager kuiManager;

        public void Awake()
        {
            kuiManager = FindObjectOfType<KUIManager>();
        }

        public void SetLayout(KUISerializedLayout locationCollection)
        {
            if (LogEvents) Debug.Log("<b>KUILayoutManager SetLayout</b> to new location " + locationCollection);
            if (locationCollection != null)
            {
                if (this.currentKeyboardLayout != locationCollection)
                {
                    if (locationCollection.InstanceRoot == null)
                    {
                        GameObject root = locationCollection.BuildLayout();
                        root.transform.SetParent(this.transform, false);
                    }

                    if (this.currentKeyboardLayout != null && this.currentKeyboardLayout.InstanceRoot != null)
                        this.currentKeyboardLayout.InstanceRoot.SetActive(false);

                    this.currentKeyboardLayout = locationCollection;
                }

                this.currentKeyboardLayout.InstanceRoot.SetActive(true);

                kuiManager.NoteLayoutChanged();
            }
            if (LogEvents) Debug.Log("<b>KUILayoutManager SetLayout</b> finished changing to " + locationCollection);
        }

        public bool HasActiveLayout()
        {
            return currentKeyboardLayout != null;
        }

        public KUISerializedLayout GetCurrentLayoutElementLocationCollection()
        {
            return currentKeyboardLayout;
        }
    }
}
