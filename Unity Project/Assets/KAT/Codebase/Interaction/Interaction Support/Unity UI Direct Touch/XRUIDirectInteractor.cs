using KAT;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace UnityEngine.XR.Interaction.Toolkit
{
    /// <summary>
    /// Detects collisions with colliders and attempts to select any UI elements on those colliders.
    /// 
    /// N.B. For this to work, the UI needs a collider aligned (but not necessarily attached) to it.
    /// </summary>
    public class XRUIDirectInteractor : XRDirectInteractor//, IUIInteractable
    {
        //[SerializeField]
        //LayerMask m_RaycastMask = -1;
        ///// <summary>Gets or sets layer mask used for limiting raycast targets.</summary>
        //public LayerMask raycastMask { get { return m_RaycastMask; } set { m_RaycastMask = value; } }

        //// Input Module for fast access to UI systems.
        //XRUIInputModule m_InputModule;

        //// Used by UpdateUIModel to retrieve the line points to pass along to Unity UI.
        //static Vector3[] s_CachedLinePoints = new Vector3[2];

        //[ReadOnly]
        //public Vector3 lastFrom, lastTo;

        //[ReadOnly]
        //public bool colliding = false;
        //[ReadOnly]
        //public bool isSelecting = false;
        //public float dwellTimeToSelect = 0.0f;
        //protected Coroutine dwellCoroutine;
        //protected bool exited = false;

        ///// <summary>
        ///// Extend the contact point in the direction of from-to by this (useful for colliders that sit above the surface of the UI)
        ///// </summary>
        //public float ExtendRayDistance = 1f;

        //protected override void Awake()
        //{
        //    base.Awake();
        //    if (m_EnableUIInteraction)
        //    {
        //        FindOrCreateXRUIInputModule();
        //        enableUIInteraction = m_EnableUIInteraction;
        //        Debug.Log("Registered " + this + " as interactable");
        //    }
        //}

        //void FindOrCreateXRUIInputModule()
        //{
        //    var eventSystem = Object.FindObjectOfType<EventSystem>();
        //    if (eventSystem == null)
        //        eventSystem = new GameObject("Event System", typeof(EventSystem)).GetComponent<EventSystem>();

        //    m_InputModule = eventSystem.GetComponent<XRUIInputModule>();
        //    if (m_InputModule == null)
        //        m_InputModule = eventSystem.gameObject.AddComponent<XRUIInputModule>();
        //}

        //[SerializeField]
        //bool m_EnableUIInteraction = true;
        ///// <summary>Gets or sets whether this interactor is able to affect UI.</summary>
        //public bool enableUIInteraction
        //{
        //    get
        //    {
        //        return m_EnableUIInteraction;
        //    }
        //    set
        //    {
        //        if (m_EnableUIInteraction != value)
        //        {
        //            m_EnableUIInteraction = value;
        //            if (enabled)
        //            {
        //                if (m_EnableUIInteraction)
        //                {
        //                    m_InputModule.RegisterInteractable(this);
        //                }
        //                else
        //                {
        //                    m_InputModule.UnregisterInteractable(this);
        //                }
        //            }
        //        }
        //    }
        //}

        //#region Basic touch handling
        //protected void StartTouch(Vector3 from, Vector3 to)
        //{
        //    colliding = true;
        //    lastFrom = from;
        //    lastTo = to;

        //    if (dwellTimeToSelect > 0.0f)
        //    {
        //        dwellCoroutine = StartCoroutine(DwellSelection(dwellTimeToSelect));
        //    }
        //    else
        //    {
        //        isSelecting = true;
        //    }
        //}

        //protected void ContinueTouch(Vector3 from, Vector3 to)
        //{
        //    colliding = true;
        //    lastFrom = from;
        //    lastTo = to;
        //}

        //protected void FinishTouch()
        //{
        //    exited = colliding;
        //    colliding = false;
        //    isSelecting = false;
        //    if (dwellCoroutine != null)
        //    {
        //        StopCoroutine(dwellCoroutine);
        //        dwellCoroutine = null;
        //    }
        //}

        //public IEnumerator DwellSelection(float dwellTime)
        //{
        //    yield return new WaitForSeconds(dwellTime);
        //    if (colliding)
        //        isSelecting = true;
        //}
        //#endregion


        //#region Collider touch handling

        //[ReadOnly]
        //public Collider currentInteractingCollider;
        //public bool InteractionAreaCollisionsOnly = true;
        //protected new void OnTriggerEnter(Collider col)
        //{
        //    base.OnTriggerEnter(col);
        //    //Debug.Log("OnTriggerEnter for " + col);

        //    if (((InteractionAreaCollisionsOnly && col.GetComponent<KUIInteractionManager>() != null) || !InteractionAreaCollisionsOnly) && currentInteractingCollider == null)
        //    {
        //        currentInteractingCollider = col;
        //        Debug.Log("StartTouch for " + col);
        //        StartTouch(col.ClosestPointOnBounds(transform.position), this.transform.position);
        //    }
        //}

        //protected virtual void OnTriggerStay(Collider col)
        //{
        //    if (col == currentInteractingCollider)
        //    {
        //        //Debug.Log("Continue touch for " + col);
        //        ContinueTouch(col.ClosestPointOnBounds(transform.position), this.transform.position);
        //    }
        //}

        //protected new void OnTriggerExit(Collider col)
        //{
        //    base.OnTriggerExit(col);
        //    //Debug.Log("Trigger exit");
        //    if (col == currentInteractingCollider)
        //    {
        //        Debug.Log("FinishTouch for " + col);
        //        FinishTouch();
        //        currentInteractingCollider = null;
        //    }
        //}
        //#endregion

        //#region External fake touch handling (e.g. for a key press)
        //public void EnactSingleDirectTouch(Vector3 from, Vector3 to)
        //{
        //    StartTouch(from, to);
        //    Invoke("FinishTouch", 0.03f);
        //}
        //#endregion

        //#region EventSystem connections
        ///// <summary>
        ///// Updates the current UI Model to match the state of the Interactor
        ///// </summary>
        ///// <param name="model">The model that will match this Interactor</param>
        //public void UpdateUIModel(ref TrackedDeviceModel model)
        //{
        //    if (colliding || (!colliding && exited))
        //    {
        //        model.position = lastFrom;
        //        model.orientation = Quaternion.identity;
        //        model.select = isSelecting;
        //        model.position = s_CachedLinePoints[0];

        //        int numPoints = 0;
        //        GetLinePoints(ref s_CachedLinePoints, ref numPoints);

        //        List<Vector3> raycastPoints = model.raycastPoints;
        //        raycastPoints.Clear();
        //        if (numPoints > 0 && s_CachedLinePoints != null)
        //        {
        //            Debug.DrawLine(s_CachedLinePoints[0], s_CachedLinePoints[1], Color.red);

        //            raycastPoints.Capacity = raycastPoints.Count + numPoints;
        //            for (int i = 0; i < numPoints; i++)
        //                raycastPoints.Add(s_CachedLinePoints[i]);
        //        }
        //        model.raycastLayerMask = raycastMask;
        //        exited = false;
        //    }
        //    else if (!colliding && !exited)
        //    {
        //        model.raycastPoints.Clear();
        //        model.position = Vector3.positiveInfinity;
        //        model.select = false;
        //    }
        //}

        ///// <summary>
        ///// Attempts to retrieve the current UI Model.  Returns false if not available.
        ///// </summary>
        ///// <param name="model"> The UI Model that matches that Interactor.</param>
        ///// <returns></returns>
        //public bool TryGetUIModel(out TrackedDeviceModel model)
        //{
        //    if (m_InputModule != null)
        //    {
        //        if (m_InputModule.GetTrackedDeviceModel(this, out model))
        //            return true;
        //    }

        //    model = new TrackedDeviceModel(-1);
        //    return false;
        //}

        ///// <summary> This function implements the ILineRenderable interface, 
        ///// if there is a raycast hit, it will return the world position and the normal vector
        ///// of the hit point, and its position in linePoints. </summary>
        //public bool TryGetHitInfo(ref Vector3 position, ref Vector3 normal, ref int positionInLine, ref bool isValidTarget)
        //{
        //    position = lastFrom;
        //    positionInLine = 1;
        //    isValidTarget = colliding;
        //    normal = Vector3.forward;//s_CachedLinePoints[2] - s_CachedLinePoints[0];
        //    return isValidTarget;
        //}

        ///// <summary> This function implements the ILineRenderable interface and returns the sample points of the line. </summary>
        //public bool GetLinePoints(ref Vector3[] linePoints, ref int noPoints)
        //{
        //    s_CachedLinePoints[1] = lastTo + (ExtendRayDistance * (lastTo - lastFrom).normalized);
        //    s_CachedLinePoints[0] = lastFrom + (ExtendRayDistance * (lastFrom - lastTo).normalized);

        //    Array.Copy(s_CachedLinePoints, linePoints, 2);
        //    noPoints = 2;
        //    return true;
        //    //}
        //}

        //public override void GetValidTargets(List<XRBaseInteractable> validTargets)
        //{
        //    validTargets.Clear();
        //}
        //#endregion
    }
}