using JetBrains.Annotations;
using KAT.KeyCodeMappings;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;
using KAT.Interaction;

namespace KAT
{
    public class KUIElementUnityUIDisplay : KUISingleAreaBinaryElementDisplay
    {
        [BoxGroup("KUIElementUnityUIDisplay")]
        public bool ScaleToFitInteractionRegion = true;
        [BoxGroup("KUIElementUnityUIDisplay")]
        public bool AlignToInteractionRegion = true;
        [BoxGroup("KUIElementUnityUIDisplay")]
        public Canvas canvas;
        [BoxGroup("KUIElementUnityUIDisplay")]
        public List<Selectable> Selectables = new List<Selectable>();
        [BoxGroup("KUIElementUnityUIDisplay")]
        public string selectablesCollisionTag = "Depression";

        private List<UISizeHelper> uiSizeHelpers = new List<UISizeHelper>();

        Vector3 localPos, localRot;
        public void Awake()
        {
            uiSizeHelpers.AddRange(GetComponentsInChildren<UISizeHelper>());
            if (Selectables.Count == 0)
                Selectables = canvas.GetComponentsInChildren<Selectable>().ToList();
            localPos = canvas.transform.localPosition;
            localRot = canvas.transform.localEulerAngles;
        }

        public override void SetKUIParent(KUIElement parent)
        {
            base.SetKUIParent(parent);
            Debug.Log("SetKUIParent for KUIElementUnityUIDisplay");
            parent.hub.Subscribe<CollisionEvent>(@event =>
            {
                Debug.Log("KUIElementUnityUIDisplay Got collision event " + @event);
                if (@event.tag.Equals(selectablesCollisionTag))
                    TriggerSelectables(@event);
            });

        }

        Vector3 linePoint1, linePoint2;
        public void Update()
        {
            Debug.DrawLine(linePoint1, linePoint2, Color.cyan);
        }

        public void TriggerSelectables(CollisionEvent @event)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            //pointerData.position = @event.descriptor.CollisionPositions.Last().Item1;
            //pointerData.position = Camera.main.WorldToViewportPoint(@event.descriptor.CollisionPositions.Last().Item1);
            //pointerData.worldPosition = @event.descriptor.CollisionPositions.Last().Item1;
            //Vector3 worldPoint = new Vector3(-0.5062f, 0.4781f, 0.5667f);
            //pointerData.worldPosition = @event.descriptor.CollisionPositions.Last().Item1;

            //Debug.Log("Got collision data for collision with " + @event.collidingCollider.gameObject + " and " + @event.eventProviderCollider.gameObject);
            //Vector3 closestPoint = @event.eventProviderCollider.ClosestPointOnBounds(@event.collidingCollider.transform.position);

            Vector3 fingerPosition = @event.collidingCollider.transform.position;
            linePoint1 = fingerPosition;
            linePoint2 = fingerPosition + (@event.eventProviderCollider.transform.forward.normalized * -0.1f);

            pointerData.worldPosition = fingerPosition;
            pointerData.position = fingerPosition; // this.canvas.transform.InverseTransformPoint(pointerData.worldPosition);
            //Debug.LogError("pointer position is " + pointerData.position);

            GraphicRaycaster raycaster = canvas.GetComponentInChildren<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);
            XRUIInputModule inputModule = FindObjectOfType<XRUIInputModule>();


            BaseEventData selectionData = new BaseEventData(EventSystem.current);
            selectionData.selectedObject = Selectables[0].gameObject;

            foreach (Selectable selectable in Selectables)
            {
                if (@event.type == CollisionType.Enter)
                {
                    //Debug.Log("Sending pointerdown event to " + selectable + " for position " + pointerData.position.ToString("F4"));
                    selectable.OnPointerEnter(pointerData);
                    selectable.OnPointerDown(pointerData);
                    selectable.OnSelect(selectionData);
                    @event.eventProvider.SetOnStayUpdates(true);

                } 
                else if (@event.type == CollisionType.Continue)
                {
                    selectable.OnPointerDown(pointerData);
                    selectable.OnSelect(selectionData);
                }
                else if (@event.type == CollisionType.Exit)
                {
                    @event.eventProvider.SetOnStayUpdates(false);
                    selectable.OnPointerExit(pointerData);
                    selectable.OnDeselect(selectionData);
                }
            }
        }

        public override void ShowDisplay()
        {
            base.ShowDisplay();
            canvas.gameObject.SetActive(true);
        }

        public override void HideDisplay()
        {
            base.HideDisplay();
            canvas.gameObject.SetActive(false);
        }

        [Button]
        public override void RefreshLayout()
        {
            base.RefreshLayout();
            KUILocationInteractionRegions CurrentInteractionArea = parent?.GetPrimaryInteractionRegion();

            if (CurrentInteractionArea != null)
            {
                Debug.LogError("Updating KUIElementUnityUIDisplay with the layout for " + CurrentInteractionArea.gameObject.GetFullName());
                canvas.transform.SetParent(CurrentInteractionArea.transform);
                canvas.transform.localEulerAngles = new Vector3(0, 180, 0);

                Vector3 localMeshSize = CurrentInteractionArea.GetLocationBounds().GetLocalMeshBoundsSize(CurrentInteractionArea.transform);
                Vector3 localMeshCenter = CurrentInteractionArea.GetLocationBounds().GetLocalMeshCenterOnSurface(CurrentInteractionArea.transform);

                Debug.Log("localMeshCenter is " + localMeshCenter.ToString("F5"));

                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                canvas.transform.localScale = Vector3.one;

                Debug.LogFormat("localPos {0} localRot {1}", localPos, localRot);
                if (AlignToInteractionRegion)
                {
                    canvas.transform.localPosition = localMeshCenter;
                } else
                {
                    canvas.transform.localPosition = localMeshCenter + localPos;
                    canvas.transform.localEulerAngles += localRot;
                }

                // for the canvas size, we want to keep the units in the 100s order of magnitude
                // so set the size based on the aspect ratio
                float aspRatio = Mathf.Abs(localMeshSize.x / localMeshSize.y);
                canvasRect.sizeDelta = new Vector2(100 * aspRatio, 100);

                // if the UI element has a box collider, assume this is a collider for the whole plane
                // of the canvas for interactions with selectables, and size to fit canvas
                BoxCollider boxCollider = canvas.GetComponentInChildren<BoxCollider>();
                if (boxCollider != null)
                    boxCollider.size = canvasRect.sizeDelta;

                if (ScaleToFitInteractionRegion)
                {
                    // now scale the canvas so that the width and height fit the layout descriptor
                    canvas.transform.localScale = new Vector3(
                        Mathf.Abs(localMeshSize.x / canvasRect.sizeDelta.x),
                        Mathf.Abs(localMeshSize.y / canvasRect.sizeDelta.y),
                        Mathf.Abs(localMeshSize.y / canvasRect.sizeDelta.y));
                }

                // set canvas width/height based on aspect ratio of canvas
                // assuming 100x scale
                //canvasRect.sizeDelta = new Vector2(intArea.layoutDescriptor.Width * 100, intArea.layoutDescriptor.Height * 100);
                //canvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

                //canvasRect.sizeDelta = new Vector2(intArea.layoutDescriptor.UIMesh.bounds.size.x, intArea.layoutDescriptor.UIMesh.bounds.size.y);

                //uiSizeHelpers.ForEach(x => x.SetWidthHeight(new Vector2(intArea.layoutDescriptor.Width, intArea.layoutDescriptor.Height)));

                // or we set the mesh renderer?

            }
            else
            {
                canvas.transform.SetParent(this.transform);
            }
        }
    }
}