using Boo.Lang;
using NaughtyAttributes;
using System.Linq;
using TriangleNet;
using UnityEngine;

namespace KAT.Layouts
{
    public class KUILocationBounds : MonoBehaviour
    {
        public UnityEngine.Mesh mesh;

        public void Awake()
        {
            GetMeshRoot();

            if (meshF != null)
            {
                mesh = meshF.mesh;
                if (mesh == null)
                    mesh = meshF.sharedMesh;
            }
        }

        private GameObject meshRoot;
        private MeshFilter meshF;
        private MeshRenderer meshRenderer;
        public GameObject GetMeshRoot()
        {
            if (meshRoot == null)
            {
                meshF = GetComponentInChildren<MeshFilter>();
                meshRenderer = GetComponentInChildren<MeshRenderer>();

                if (meshF != null)
                {
                    meshRoot = meshF.gameObject;
                }
            }

            return meshRoot;
        }

        public Vector3 GetMeshScale(bool lossyScale = true)
        {
            GetMeshRoot();
            if (meshRoot != null)
            {
                if (lossyScale)
                    return meshRoot.transform.lossyScale;
                else
                    return meshRoot.transform.localScale;
            }
            return Vector3.one;
        }

        [Button]
        public void DebugOutput()
        {
            GetLocalMeshBoundsSize(this.transform);
        }

        /// <summary>
        /// Gives you a point fractionally offset from the surface of the mesh
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetLocalMeshCenterOnSurface(Transform t)
        {
            Vector3 center = GetLocalMeshBoundsCenter(t);
            Vector3 size = GetLocalMeshBoundsSize(t);
            return center + new Vector3(0, 0, size.z / 2 + 0.001f);
        }

        public Vector3 GetLocalMeshBoundsSize(Transform t)
        {
            return t.InverseTransformVector(meshRoot.transform.TransformVector(GetLocalMeshBounds().size));
        }

        public Vector3 GetLocalMeshBoundsCenter(Transform t)
        {
            return t.InverseTransformPoint( meshRoot.transform.TransformPoint(GetLocalMeshBounds().center) );
        }

        public Bounds GetLocalMeshBounds()
        {
            if (meshF != null)
            {
                if (meshF.mesh != null)
                    return meshF.mesh.bounds;
                else
                    return meshF.sharedMesh.bounds;
            }
            return new Bounds(Vector3.one, Vector3.one);
        }

        //public float ApplyScale(int dim, float value, bool invert, bool lossyScale = true)
        //{
        //    if (meshRoot != null)
        //    {
        //        return invert?  (1/ GetMeshScale(lossyScale)[dim]) * value : GetMeshScale(lossyScale)[dim] * value;
        //    }
        //    return value;
        //}

        /// <summary>
        /// NB need to be careful here, as we can end up encapsulating child meshes we didn't intend to...
        /// </summary>
        /// <param name="useMeshFilter"></param>
        /// <returns></returns>
        public Bounds GetEncapsulatedBounds(bool useMeshFilter = true)
        {
            GetMeshRoot();
            return UnityMeshExtensions.GetEncapsulatedBounds(meshRoot, useMeshFilter);
        }

        public bool IsMeshPresent()
        {
            return GetComponentsInChildren<MeshFilter>().Count() > 0;
        }
    }
}
