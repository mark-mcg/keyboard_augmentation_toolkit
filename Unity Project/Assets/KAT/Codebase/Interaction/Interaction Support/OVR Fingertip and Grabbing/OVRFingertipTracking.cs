using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Attach this to the Left/RightHandAnchors in the OVR Prefab and this will
/// add colliders to fingertips for direct touch interactions.
/// </summary>
public class OVRFingertipTracking : MonoBehaviour
{
    public string[] fingertips = { "Hand_ThumbTip", "Hand_IndexTip", "Hand_MiddleTip", "Hand_RingTip", "Hand_PinkyTip" };
    public Dictionary<string, GameObject> fingertipGameObjects = new Dictionary<string, GameObject>();
    public bool fingertipsFound = false;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FingertipCheck", 0.5f, 1.0f);    
    }

    void FingertipCheck()
    {
        if (transform.FindChildRecursive(fingertips[0]))
        {
            fingertipsFound = true;
            CancelInvoke("FingertipCheck");
            Debug.Log("Found fingertips");

            foreach (string s in fingertips)
            {
                // Root of finger tip contains our anchor RB
                GameObject root = transform.FindChildRecursive(s).gameObject;
                fingertipGameObjects.Add(s, root);
                Rigidbody rootRB = root.AddComponent<Rigidbody>();
                rootRB.useGravity = false;
                rootRB.isKinematic = true;

                // finger tip is child of root tip
                GameObject tip = new GameObject("Interactive Tip");
                tip.transform.SetParent(root.transform);
                tip.transform.localPosition = Vector3.zero;
                tip.transform.localEulerAngles = Vector3.zero;

                // we add a collider that roughly covers the finger tip volume
                SphereCollider tipCollider = tip.AddComponent<SphereCollider>();
                tipCollider.radius = 0.005f;
                tipCollider.center = new Vector3(-tipCollider.radius, 0, 0);
                tipCollider.isTrigger = false;
                Rigidbody tipRB = tip.AddComponent<Rigidbody>();
                tipRB.useGravity = false;
                tipRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                //tipRB.constraints = ~RigidbodyConstraints.FreezePositionX;

                //// and a spring, so the finger can somewhat pass through a surface,
                //// but the colliding tip will keep pushing against it
                //SpringJoint springJoint = tip.AddComponent<SpringJoint>();
                //springJoint.spring = 100000;
                //springJoint.damper = 1000;
                //springJoint.tolerance = 0f;
                //springJoint.connectedBody = rootRB;
                tipRB.isKinematic = true;

                XRUIDirectInteractor directTouch = tip.AddComponent<XRUIDirectInteractor>();
                XRController controller = tip.GetComponent<XRController>();
                controller.enabled = false;
            }
        }
    }
}
