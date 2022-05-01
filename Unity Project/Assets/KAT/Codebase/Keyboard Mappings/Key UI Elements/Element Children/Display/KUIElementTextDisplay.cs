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
    public class KUIElementTextDisplay : KUISingleAreaBinaryElementDisplay
    {
        [BoxGroup("KUIElementTextDisplay")]

        public KUIElementTextDisplayServer.TextDescriptor text = new KUIElementTextDisplayServer.TextDescriptor();
        KUIElementTextDisplayServer textServer;

        public void SetText(string newText)
        {
            text.text = newText;
            if (ShowingDisplay)
                ShowDisplay();
        }

        public override void ShowDisplay()
        {
            //Debug.Log("ShowDisplay called");
            base.ShowDisplay();
            if (GetServer() != null)
            {
                text.client = this;
                textServer.ShowText(text, this);
            }
        }

        public override void HideDisplay()
        {
            base.HideDisplay();
            if (GetServer() != null)
            {
                textServer.StopShowingText(this);
            }
        }

        public override void RefreshLayout()
        {
            base.RefreshLayout();
            GetServer(true);
        }

        private KUIElementTextDisplayServer GetServer(bool forceUpdate = false)
        {
            if ((textServer == null || forceUpdate) && parent != null )
                textServer = KUIElementTextDisplayServer.GetTextDisplayServer(parent.GetPrimaryInteractionRegion());
            return textServer;
        }
    }
}