using KAT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using KAT.Layouts;
using KAT.Interaction;

namespace KAT
{
    public class KUIElementTextDisplayServer : MonoBehaviour
    {
        protected KUILocationInteractionRegions interactionArea;
        public static KUIElementTextDisplayServer GetTextDisplayServer(KUILocationInteractionRegions intArea)
        {
            if (intArea != null)
            {
                KUIElementTextDisplayServer server = intArea.gameObject.GetComponentInChildren<KUIElementTextDisplayServer>();
                
                if (server == null)
                {
                    /*
                     * Note - for the text mesh pro element, we do some funky things with scaling and sizing
                     * so that the component can appropriately scale its text, namely we make the scale 0.1
                     * and the size 10x. Otherwise, we run into the limits of the autofit sizing on key caps!
                     * 
                     */

                    //Debug.Log("Adding TextMeshPro component to " + intArea);
                    GameObject tmpLocation = new GameObject("TMP");
                    tmpLocation.transform.SetParent(intArea.transform);
                    tmpLocation.transform.localPosition = Vector3.zero;
                    tmpLocation.transform.localScale = Vector3.one / 10;
                    tmpLocation.transform.localEulerAngles = new Vector3(0, 180, 0);
                    TextMeshPro tmpText = tmpLocation.AddComponent<TextMeshPro>();
                    TextContainer tmpContainer = tmpText.GetComponent<TextContainer>();
                    server = tmpLocation.AddComponent<KUIElementTextDisplayServer>();

                    tmpText.text = "blep";
                    tmpText.enableAutoSizing = true;
                    tmpText.fontSizeMin = 0.0001f;
                    tmpText.alignment = TextAlignmentOptions.Center;
                    server.interactionArea = intArea;
                }
                return server;
            }
            return null;
        }

        public void UpdateLayoutAndRefreshText()
        {
            UpdateLayout();
            UpdateTextDisplay();
        }

        public void UpdateLayout()
        {
            Vector3 localMeshSize = interactionArea.GetLocationBounds().GetLocalMeshBoundsSize(interactionArea.transform) * 10;
            Vector3 localMeshCenter = interactionArea.GetLocationBounds().GetLocalMeshBoundsCenter(interactionArea.transform);
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Mathf.Abs(localMeshSize.x) * 0.85f, Mathf.Abs(localMeshSize.y));

            // text mesh doesn't obey the scale on z, so use the unscaled mesh z size
            rectTransform.localPosition = (localMeshCenter) + new Vector3(0, 0, (localMeshSize.z / 10) / 2 + 0.001f);
        }


        TextMeshPro tmpText;
        [Serializable]
        public class Layout
        {
            public Centering centering = Centering.center;
            public Position position = Position.middle;

            public Layout()
            {
            }

            public Layout(Centering centering, Position position)
            {
                this.centering = centering;
                this.position = position;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                if (obj is Layout layout)
                    return this.centering == layout.centering && this.position == layout.position;
                return base.Equals(obj);
            }
        }

        public enum Centering { no_preference, left, center, right };
        public enum Position { no_preference, top, middle, bottom };

        public List<Layout> layouts = new List<Layout>()
    {
        new Layout(Centering.center, Position.middle),
        new Layout(Centering.left, Position.top),
        new Layout(Centering.left, Position.bottom),
        new Layout(Centering.right, Position.top),
        new Layout(Centering.right, Position.bottom),
        new Layout(Centering.right, Position.middle),

        //new Layout(Centering.center, Position.top),
        //new Layout(Centering.center, Position.bottom),
        //new Layout(Centering.left, Position.middle),
        //new Layout(Centering.right, Position.middle),

    };

        [Serializable]
        public class TextDescriptor
        {
            public string text;
            private string formattedText;
            public Layout preferredLayout = new Layout(Centering.no_preference, Position.no_preference);
            [HideInInspector]
            public Layout assignedLayout;
            [HideInInspector]
            public float timeEntered;
            [HideInInspector]
            public KUIElementTextDisplay client;

            public TextDescriptor() { }

            // auto layout
            public TextDescriptor(string text)
            {
                this.text = text;
            }

            public string GetFormattedText()
            {
                return TextUtils.FormatText(text);
            }

            public TextDescriptor(string text, Centering preferredCentering, Position preferredPosition)
            {
                this.text = text;
                this.preferredLayout = new Layout(preferredCentering, preferredPosition);
            }



            public bool MatchesPosition(TextDescriptor desc)
            {
                if (desc == null)
                    return false;
                return MatchesPosition(desc.preferredLayout);
            }

            public bool MatchesPosition(Layout layout)
            {
                if (layout == null)
                    return false;
                return this.preferredLayout.Equals(layout);
            }
        }

        public Dictionary<object, TextDescriptor> CurrentText = new Dictionary<object, TextDescriptor>();

        // Start is called before the first frame update
        void Awake()
        {
            tmpText = GetComponent<TextMeshPro>();
        }

        public void ShowText(TextDescriptor text, object client)
        {
            CurrentText[client] = text;
            text.timeEntered = Time.time;
            UpdateLayoutAndRefreshText();
        }

        public void StopShowingText(object client)
        {
            CurrentText.Remove(client);
            //Debug.Log("Removed text for client " + client);
            UpdateTextDisplay();
        }

        public void UpdateTextDisplay()
        {
            KUISerializedLayout collectionLayout = interactionArea.GetLocation().kuiManager.layoutManager.GetCurrentLayoutElementLocationCollection();

            List<Layout> usedLayouts = new List<Layout>();
            usedLayouts.AddRange(layouts);

            // assign layout positions for each text descriptor
            foreach (TextDescriptor descriptor in CurrentText.Values.OrderBy(x => x.client.parent.depth).ThenBy(x => x.client.transform.GetSiblingIndex()))
            {
                descriptor.assignedLayout = null;

                if (descriptor.preferredLayout != null)
                {
                    if (usedLayouts.Any(x => x.Equals(descriptor.preferredLayout)))
                    {
                        usedLayouts.Remove(descriptor.preferredLayout);
                        descriptor.assignedLayout = descriptor.preferredLayout;
                    }
                }

                if (descriptor.preferredLayout == null || descriptor.assignedLayout == null && usedLayouts.Count > 0)
                {
                    descriptor.assignedLayout = usedLayouts[0];
                    usedLayouts.Remove(descriptor.assignedLayout);
                }
            }

            /*
             *  <size=50%><line-height=150%><align=left>B<pos=75%>1 
                <size=100%><line-height=50%><align=center>& 
                <size=50%><line-height=150%><align=left>1<pos=75%>a
             * 
             */

            string text = "";
            foreach (Layout layout in layouts.OrderBy(x => x.position).ThenBy(x => x.centering))
            {
                TextDescriptor descriptor = CurrentText.Values.FirstOrDefault(x => x.assignedLayout.Equals(layout));
                if (descriptor != null)
                {
                    if (descriptor.assignedLayout.position == Position.middle)
                    {
                        if (CurrentText.Values.Count == 1)
                        {
                            text += "<size=150%><line-height=50%><align=center><color=#" + ColorUtility.ToHtmlStringRGB(collectionLayout.DefaultTextColor) + ">";
                        }
                        else
                        {
                            text += "<size=100%><line-height=50%><align=center><color=#" + ColorUtility.ToHtmlStringRGB(collectionLayout.DefaultTextColor) + "><alpha=#FF>";
                        }
                    }
                    else
                    {
                        text += "<size=50%><line-height=150%><color=#" + ColorUtility.ToHtmlStringRGB(collectionLayout.DefaultTextColor) + "><alpha=#88><alpha=#FF><mark=#" + ColorUtility.ToHtmlStringRGB(collectionLayout.DefaultHighlightColor) + "44>";
                    }

                    switch (descriptor.assignedLayout.centering)
                    {
                        case Centering.left:
                            text += "<align=left>";
                            break;
                        case Centering.center:
                            text += "<align=center>";
                            break;
                        case Centering.right:
                            text += "<pos=75%>"; // can't have two alignment commands on the same line, so force a right alignment
                            break;
                    }
                    text += descriptor.GetFormattedText();

                    if (descriptor.assignedLayout.position != Position.middle)
                    {
                        text += "</mark>";
                    }
                }

                if (layout.centering == Centering.right)
                {
                    text += "\n";
                }
            }
            tmpText.SetText(text);
        }
    }
}
