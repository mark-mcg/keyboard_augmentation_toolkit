using KAT.KeyCodeMappings;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using KAT.Interaction;

namespace KAT
{
    /// <summary>
    /// See http://digitalnativestudios.com/textmeshpro/docs/rich-text/
    /// </summary>
    public class KUISingleCalloutDisplay : KUISingleAreaBinaryElementDisplay
    {
        public string text;
        private Label CurrentLabel;

        public override void ShowDisplay()
        {
            base.ShowDisplay();
            if (!KUISharedResource.IsResourceClaimed(CurrentLabel, this))
            {
                KUISharedResource.NoteSharedResource(CurrentLabel, this);
                CurrentLabel.SetText(text);
                CurrentLabel.StartFadingIn();
            }
        }

        public override void HideDisplay()
        {
            base.HideDisplay();

            if (CurrentLabel != null)
            {
                Debug.Log("Hiding label");
                if (KUISharedResource.DoWeOwnSharedResource(CurrentLabel, this))
                {
                    KUISharedResource.ReleaseSharedResource(CurrentLabel, this);
                    CurrentLabel.StartFadingOut();
                }
            }  
        }

        public override void RefreshLayout()
        {
            CurrentLabel = GetCalloutTextLabel(parent?.GetPrimaryInteractionRegion(), true);
        }

        protected Label GetCalloutTextLabel(KUILocationInteractionRegions intArea, bool addIfNotPresent)
        {
            if (intArea != null)
            {
                Transform label = intArea.transform.Find("Name Label");
                if (label == null && addIfNotPresent)
                {
                    GameObject labelPrefab = Instantiate(Resources.Load("NameLabelWithCanvas") as GameObject);
                    labelPrefab.name = "Name Label";
                    labelPrefab.transform.SetParent(intArea.transform);
                    labelPrefab.transform.localPosition = Vector3.zero;
                    labelPrefab.transform.localEulerAngles = Vector3.zero;
                    label = labelPrefab.transform;
                }
                return label?.GetComponentInChildren<Label>();
            }
            return null;
        }
    }
}