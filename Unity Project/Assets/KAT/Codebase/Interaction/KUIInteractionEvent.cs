using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
namespace KAT.Interaction
{
    public class KUIInteractionEvent : KUIElementEvent
    {
        public enum Interaction
        {
            HoverExit,
            HoverEnter,
            FirstHoverEnter,
            LastHoverExit,
            SelectEnter,
            SelectExit,
            Activate,
            Inactive
        }

        public Interaction id;
        public XRBaseInteractor interactor;

        public KUIInteractionEvent(Interaction id, KUIElement element = null, XRBaseInteractor interactor = null) : base(element)
        {
            this.id = id;
            this.interactor = interactor;
        }

        public override string ToString()
        {
            return "KUIInteractionEvent {" + id + "}";
        }
    }
}