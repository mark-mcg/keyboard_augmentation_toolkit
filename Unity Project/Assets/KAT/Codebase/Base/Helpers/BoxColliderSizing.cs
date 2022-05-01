using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderSizing : MonoBehaviour
{
    public MeshFilter meshFilterForSizing;

    void Start()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.center = meshFilterForSizing.transform.localPosition;
        collider.size = Vector3.Scale( meshFilterForSizing.sharedMesh.bounds.size, meshFilterForSizing.transform.lossyScale);
    }
}
