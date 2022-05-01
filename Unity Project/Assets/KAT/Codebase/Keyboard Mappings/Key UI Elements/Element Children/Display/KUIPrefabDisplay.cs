using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KAT.KeyCodeMappings;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using KAT.Layouts;
using KAT.Interaction;

namespace KAT
{
    public class KUIPrefabDisplay : KUISingleAreaBinaryElementDisplay
    {
        public GameObject PrefabToDisplay;
        private GameObject PrefabInstance;
        private KUIPrefabDisplayServer DisplayServer;
        public override void ShowDisplay()
        {
            base.ShowDisplay();
            DisplayServer = KUIPrefabDisplayServer.GetDisplayServer(parent.GetInteractionAreas()[0]);

            if (PrefabInstance == null)
                PrefabInstance = GameObject.Instantiate(PrefabToDisplay);

            DisplayServer.ShowMesh(new KUIPrefabDisplayServer.MeshDescriptor(PrefabInstance, this));
        }

        public override void HideDisplay()
        {
            base.HideDisplay();
            if (DisplayServer != null)
                DisplayServer.StopShowingMesh(this);
        }

        public override void RefreshLayout()
        {
            base.RefreshLayout();
            DisplayServer = KUIPrefabDisplayServer.GetDisplayServer(parent?.GetPrimaryInteractionRegion());
            if (DisplayServer != null)
                DisplayServer.UpdateDisplay();
        }
    }
}