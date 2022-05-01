using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using UnityEngine;

namespace TriangleNet
{
    public static class UnityMeshExtensions
    {
        public static void CopyTransform(this Transform t, Transform from, bool useLocal = false){
            if (useLocal)
            {
                t.localPosition = from.localPosition;
                t.localEulerAngles = from.localEulerAngles;
            } else
            {
                t.position = from.position;
                t.eulerAngles = from.eulerAngles;
            }
            t.localScale = from.localScale;
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (component == null)
                component = gameObject.AddComponent<T>() as T;

            return component;
        }


        /*
         * Own extensions
         */

        public static void AddOrUpdateMesh(this GameObject go, UnityEngine.Mesh mesh, bool addCollider = true, bool addRB = true, bool addRenderer = true, bool isTrigger = true)
        {
            if (go.GetComponent<MeshFilter>() != null)
            {
                UpdateMesh(go, mesh);
            } else
            {
                AddMesh(go, mesh, addCollider, addRB, addRenderer, isTrigger);
            }
        }

        public static void UpdateMesh(this GameObject go, UnityEngine.Mesh mesh)
        {
            MeshFilter mf = go.GetComponent<MeshFilter>();
            if (mf) mf.mesh = mesh;
            MeshCollider mc = go.GetComponent<MeshCollider>();
            if (mc) mc.sharedMesh = mesh;
        }

        public static MeshFilter AddMesh(this GameObject go, UnityEngine.Mesh mesh, bool addCollider=true, bool addRB = true, bool addRenderer = true, bool isTrigger = true)
        {
            MeshCollider col = null;
            MeshFilter meshFilter = go.GetOrAddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            if (addRenderer)
            {
                MeshRenderer meshRenderer = go.GetOrAddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
            }

            if (addCollider)
            {
                col = (MeshCollider)go.GetOrAddComponent< MeshCollider>();
                col.convex = true;
                col.isTrigger = isTrigger;
                col.sharedMesh = mesh;
            }

            if (addRB)
            {
                Rigidbody rb = (Rigidbody)go.GetOrAddComponent< Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            return meshFilter;
        }

        /// <summary>
        /// Given a mesh, attempts to extract the surface of that mesh in the direction specified, with an angle of tolerance.
        /// 
        /// Used e.g. to extract the surface of a keycap model.
        /// 
        //// see https://straypixels.net/delaunay-triangulation-terrain/
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="direction"></param>
        /// <param name="toleranceAngle"></param>
        /// <returns></returns>
        public static UnityEngine.Mesh ExtractSurfaceMesh(UnityEngine.MeshFilter meshFilter, Vector3 direction, float toleranceAngle, bool flipTriOrder = false)
        {
            UnityEngine.Mesh generatedMesh = null;
            try
            {
                HashSet<int> alignedIndices = new HashSet<int>(meshFilter.sharedMesh.triangles.AsEnumerable().Distinct().Where(index =>
                    Vector3.Angle(meshFilter.sharedMesh.normals[index], direction) < toleranceAngle
                ).ToList());

                List<Vector3> extractedVerts = new List<Vector3>();
                foreach (int index in alignedIndices)
                {
                    extractedVerts.Add(//meshFilter.mesh.vertices[index]);
                                       //meshFilter.transform.position - meshFilter.transform.TransformPoint(meshFilter.sharedMesh.vertices[index]));
                        meshFilter.sharedMesh.vertices[index]);

                }

                Polygon polygon = new Polygon();
                extractedVerts.Select(x => new Vertex(x.x, x.y)).ToList().ForEach(x => polygon.Add(x));
                TriangleNet.Meshing.ConstraintOptions options =
                    new TriangleNet.Meshing.ConstraintOptions() { ConformingDelaunay = false, Convex = true, SegmentSplitting = 2 };
                TriangleNet.Mesh tnetMesh = (TriangleNet.Mesh)polygon.Triangulate(options);

                generatedMesh = tnetMesh.GenerateUnityMesh(null, flipTriOrder);
                generatedMesh.vertices = extractedVerts.Select(x => new Vector3(x.x, x.y, extractedVerts.Max(y => y.z))).ToArray();
                generatedMesh.vertices = generatedMesh.vertices.Select(x => Vector3.Scale(x, meshFilter.transform.lossyScale)).ToArray();
                generatedMesh.RecalculateBounds();
                generatedMesh.RecalculateNormals();
                SetUVs(generatedMesh);
                //Debug.Log("Generated mesh has " + generatedMesh.vertices.Count() + " extracted vertices were " + extractedVerts.Count() + " first " + generatedMesh.vertices[0]);
            } catch (Exception e)
            {
                Debug.LogError(e);
            }
            return generatedMesh;
        }

        /// <summary>
        /// Naively generates UV coordinates based on min/max on x/y
        /// </summary>
        /// <param name="mesh"></param>
        public static void SetUVs(UnityEngine.Mesh mesh)
        {
            Vector2[] uv = new Vector2[mesh.vertices.Length];
            float xMax = mesh.vertices.Max(x => x.x);
            float xMin = mesh.vertices.Min(x => x.x);
            float yMax = mesh.vertices.Max(x => x.y);
            float yMin = mesh.vertices.Min(x => x.y);

            int ind = 0;
            foreach (Vector3 vert in mesh.vertices)
            {
                uv[ind] = new Vector2(Mathf.InverseLerp(xMin, xMax, vert.x), Mathf.InverseLerp(yMin, yMax, vert.y));
                ind++;
            }
            mesh.uv = uv;
        }

        public static UnityEngine.Mesh MergeUnityMeshes(List<Tuple<UnityEngine.Mesh, Transform>> meshesAndRelativeTransforms, Transform parentTransform, bool makeMergedPolygon, bool flipTriOrder)
        {
            Debug.Log("Merging " + meshesAndRelativeTransforms.Count + " meshes");
            UnityEngine.Mesh combined = null;

            if (meshesAndRelativeTransforms.Count > 0)
            {
                CombineInstance[] combine = new CombineInstance[meshesAndRelativeTransforms.Count];

                int i = 0;
                foreach (Tuple<UnityEngine.Mesh, Transform> meshCombo in meshesAndRelativeTransforms)
                {
                    Debug.Log("Adding mesh " + meshCombo.Item1 + " to combine list, has vertices " + meshCombo.Item1.vertices.Count());
                    combine[i].mesh = meshCombo.Item1;
                    combine[i].transform = parentTransform.worldToLocalMatrix * meshCombo.Item2.transform.localToWorldMatrix; 

                    if (!meshCombo.Item1.isReadable)
                        Debug.LogError("Mesh " + meshCombo.Item1 + " not readable, set this in the import settings, combination mesh will not be generated correctly.");
                    i++;
                }

                combined = new UnityEngine.Mesh();
                combined.CombineMeshes(combine);

                if (makeMergedPolygon)
                {
                    Vector3[] combinedVertices = (Vector3[])combined.vertices.Clone();
                    Polygon polygon = new Polygon();
                    combined.vertices.Select(x => new Vertex(x.x, x.y)).ToList().ForEach(x => polygon.Add(x));
                    TriangleNet.Meshing.ConstraintOptions options =
                        new TriangleNet.Meshing.ConstraintOptions() { ConformingDelaunay = false, Convex = true, SegmentSplitting = 2 };
                    TriangleNet.Mesh tnetMesh = (TriangleNet.Mesh)polygon.Triangulate(options);
                    UnityEngine.Mesh generatedMesh = tnetMesh.GenerateUnityMesh(null, flipTriOrder);
                    combined = generatedMesh;

                    // need to fix the z coordinates of this generated 2d polygon, and move it fractionally in front of the previous meshes
                    Vector3[] newVerts = new Vector3[combined.vertices.Count()];
                    for (int j = 0; j < combined.vertices.Length; j++)
                    {
                        //newVerts[j] = combined.vertices[j] + new Vector3(0, 0, combinedVertices[j].z + 0.00000001f);
                        // todo - change this to use the planar surfaces...
                        newVerts[j] = combined.vertices[j] + new Vector3(0, 0, combinedVertices.Max(x => x.z) + 0.00000001f);

                    }
                    combined.vertices = newVerts;
                }

                SetUVs(combined);
                combined.RecalculateNormals();
                combined.RecalculateBounds();
            }
            else
            {
                Debug.LogError("Can't create key mesh, no shapes specified");
            }

            return combined;
        }


        /// <summary>
        /// This generates a mesh based on the KeySurfaceShapes of the KeyElement,
        /// generating meshes for, and then combining, each surface shape.
        /// </summary>
        /// <param name="Root"></param>
        /// <param name="elementSceneInstance"></param>
        /// <returns></returns>
        public static UnityEngine.Mesh MergeUnityMeshes(List<UnityEngine.Mesh> meshes, GameObject transformRelativeTo = null)
        {
            Debug.Log("Merging " + meshes.Count + " meshes");
            UnityEngine.Mesh combined = null;

            if (meshes.Count > 0)
            {
                CombineInstance[] combine = new CombineInstance[meshes.Count];

                int i = 0;
                foreach (UnityEngine.Mesh mesh in meshes)
                {
                    combine[i].mesh = mesh;
                    if (transformRelativeTo != null)
                        combine[i].transform = transformRelativeTo.transform.localToWorldMatrix;
                    else
                        combine[i].transform = Matrix4x4.identity; // assuming the mesh will be placed appropriately by whatever object filter is attached to
                    i++;
                }

                combined = new UnityEngine.Mesh();
                combined.CombineMeshes(combine);

                SetUVs(combined);
                combined.RecalculateNormals();
                combined.RecalculateBounds();
            }
            else
            {
                Debug.LogError("Can't create key mesh, no shapes specified");
            }

            return combined;
        }
    
        public static Bounds GetEncapsulatedBounds(GameObject root, bool useMeshFilter = false)
        {
            Bounds bounds = default(Bounds);
            if (useMeshFilter)
            {
                foreach (MeshFilter filter in root.GetComponentsInChildren<MeshFilter>())
                {
                    if (bounds == default(Bounds))
                        bounds = filter.mesh != null ? filter.mesh.bounds : filter.sharedMesh.bounds;
                    else
                        bounds.Encapsulate(filter.mesh != null ? filter.mesh.bounds : filter.sharedMesh.bounds);
                }
            }
            else
            {
                foreach (MeshRenderer filter in root.GetComponentsInChildren<MeshRenderer>())
                {
                    if (bounds == default(Bounds))
                        bounds = filter.bounds;
                    else
                        bounds.Encapsulate(filter.bounds);
                }
            }
            return bounds;
        }

        /// <summary>
        /// This generates a mesh based on the KeySurfaceShapes of the KeyElement,
        /// generating meshes for, and then combining, each surface shape.
        /// </summary>
        /// <param name="Root"></param>
        /// <param name="elementSceneInstance"></param>
        /// <returns></returns>
        public static UnityEngine.Mesh CreateMeshFromVertices(List<Vector3> vertices, bool flipTriOrder)
        {
            UnityEngine.Mesh mesh = null;

            if (vertices.Count > 0)
            {
                Vector2[] vertices2d = vertices.Select(x => new Vector2(x.x, x.y)).ToArray();
                Polygon polygon = new Polygon();
                    vertices.Select(x => new Vertex(x.x, x.y)).ToList().ForEach(x => polygon.Add(x));

                    // ConformingDelaunay is false by default; this leads to ugly long polygons at the edges
                    // because the algorithm will try to keep the mesh convex
                    TriangleNet.Meshing.ConstraintOptions options =
                    new TriangleNet.Meshing.ConstraintOptions() { ConformingDelaunay = true, Convex = false, SegmentSplitting = 2 };
                TriangleNet.Mesh tnetMesh = (TriangleNet.Mesh)polygon.Triangulate(options);

                mesh = GenerateUnityMesh(tnetMesh);

                if (flipTriOrder)
                {
                    var triangles = tnetMesh.Triangles;
                    int[] trisIndex = new int[triangles.Count * 3];
                    int k = 0;
                    var triangleNetVerts = tnetMesh.Vertices.ToList();

                    foreach (var triangle in triangles)
                    {
                        for (int i = 2; i >= 0; i--)
                        {
                            trisIndex[k] = triangleNetVerts.IndexOf(triangle.GetVertex(i));
                            k++;
                        }
                    }
                    mesh.triangles = flipTriOrder ? trisIndex.Reverse().ToArray() : trisIndex;

                }
                mesh.triangles.Reverse();
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                SetUVs(mesh);
            }
            else
            {
                Debug.LogError("Can't create key mesh, no shapes specified");
            }

            return mesh;
        }

        public enum BlendMode { Opaque, Cutout, Fade, Transparent};
        public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
        {
            switch (blendMode)
            {
                case BlendMode.Opaque:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
                case BlendMode.Cutout:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 2450;
                    break;
                case BlendMode.Fade:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
                case BlendMode.Transparent:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
            }
        }

        /*
            * From https://github.com/Nox7atra/Triangle.Net-for-Unity/blob/master/Assets/TriangleNet/Scripts/UnityExtentions.cs
            */
        public static void Add(this Polygon polygon, List<Vector2> contour, bool isHole = false)
        {
            polygon.Add(new Contour(contour.ToTriangleNetVertices()), isHole);
        }

        public static void Add(this Polygon polygon, Vector2 vertex)
        {
            polygon.Add(new Vertex(vertex.x, vertex.y));
        }

        public static UnityEngine.Mesh GenerateUnityMesh(this TriangleNet.Mesh triangleNetMesh, QualityOptions options = null, bool flipTriOrder = false)
        {
            if (options != null)
            {
                triangleNetMesh.Refine(options);
            }
         
            UnityEngine.Mesh mesh = new UnityEngine.Mesh();
            var triangleNetVerts = triangleNetMesh.Vertices.ToList();
  
       
            Vector3[] verts = new Vector3[triangleNetVerts.Count];

            for (int i = 0; i < verts.Length; i++)
            {
                verts[i] = new Vector3((float)triangleNetVerts[i].x, (float)triangleNetVerts[i].y, 0);
            }
            
            int k = 0;

            var triangles = triangleNetMesh.Triangles;
            int[] trisIndex = new int[triangles.Count * 3];

            foreach (var triangle in triangles)
            {
                for (int i = 2; i >= 0; i--)
                {
                    trisIndex[k] = triangleNetVerts.IndexOf(triangle.GetVertex(i));
                    k++;
                }
            }

            mesh.vertices = verts;
            mesh.triangles = flipTriOrder ? trisIndex.Reverse().ToArray() :  trisIndex;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            // invert the normals
            //mesh.SetNormals(mesh.normals.Select(x => Quaternion.Euler(0,180,0) * x).ToArray());

            return mesh;
        }
        
        private static List<Vertex> ToTriangleNetVertices(this List<Vector2> points)
        {
            List<Vertex> vertices = new List<Vertex>();
            foreach (var vec in points)
            {
                vertices.Add(new Vertex(vec.x, vec.y));
            }

            return vertices;
        }
    }
}