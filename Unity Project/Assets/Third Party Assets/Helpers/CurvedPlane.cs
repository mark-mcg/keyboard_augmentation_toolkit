/*
MIT License
Copyright (c) 2016 Matt Favero
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.


Mark: Minor modifications as the original CurvedPlane script didn't generate UV coordinates.
*/
using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CurvedPlane : MonoBehaviour
{
    [Serializable]
    public class MeshData
    {
        public Vector3[] Vertices { get; set; }
        public int[] Triangles { get; set; }
        public Vector2[] UVs { get; set; }
    }

    public bool DrawDebug = false;

    [SerializeField]
    private float height = 1f;
    [SerializeField]
    private float radius = 2f;

    [SerializeField]
    [Range(1, 1024)]
    private int numSegments = 16;


    // Rough guide - 4096 horizontal resolution equates to roughly 180 degrees as an arc
    [SerializeField]
    [Range(0f, 360f)]
    private float curvatureDegrees = 60f;

    [SerializeField]
    private bool useArc = true;

    public MeshData plane;

    void Start()
    {
        Generate();
    }

    void OnValidate()
    {
        Generate();
    }

    [ContextMenu("Generate")]
    private void Generate()
    {
        GenerateScreen();
        UpdateMeshFilter();
    }

    private void UpdateMeshFilter()
    {
        var filter = GetComponent<MeshFilter>();

        var mesh = new Mesh
        {
            vertices = plane.Vertices,
            triangles = plane.Triangles,
            uv = plane.UVs
        };

        filter.mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if (DrawDebug)
        {
            Vector3[] vertices = plane.Vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawWireSphere(center: vertices[i], radius: 0.1f);
            }

            Vector2[] uvs = plane.UVs;

            for (int i = 0; i < uvs.Length; i++)
            {
                Gizmos.DrawWireSphere(center: new Vector3(uvs[i].x, uvs[i].y, 0), radius: 0.04f);
            }
        }
    }


    private void GenerateScreen()
    {
        plane = new MeshData
        {
            Vertices = new Vector3[(numSegments + 2) * 2],
            Triangles = new int[numSegments * 6],
            UVs = new Vector2[(numSegments +2) * 2]
        };

        //Debug.Log("numSegments=" + numSegments + " vertices=" + plane.Vertices.Length);

        int i, j;
        for (i = j = 0; i < numSegments + 1; i++)
        {
            GenerateVertexPair(ref i, ref j);
            if (i < numSegments)
            {
                GenerateLeftTriangle(ref i, ref j);
                GenerateRightTriangle(ref i, ref j);
            }
        }

        // UV pass
        for (i = j = 0; i < numSegments + 1; i++)
        {
            GenerateUVs(ref i);
        }
    }

    private void GenerateVertexPair(ref int i, ref int j)
    {
        float amt = ((float)i) / numSegments;
        float arcDegrees = curvatureDegrees * Mathf.Deg2Rad;
        float theta = -0.5f + amt;

        var x = useArc ? Mathf.Sin(theta * arcDegrees) * radius : (-0.5f * radius) + (amt * radius);
        var z = Mathf.Cos(theta * arcDegrees) * radius;

        plane.Vertices[i] = new Vector3(x, height / 2f, z);
        plane.Vertices[i + numSegments + 1] = new Vector3(x, -height / 2f, z);
    }

    private void GenerateUVs(ref int i)
    {
        float uvx = Mathf.InverseLerp(0, numSegments, i);
        float uvytop = 1.0f; // (height/2f))
        float uvybottom = 0.0f; // (-height / 2f)
        plane.UVs[i] = new Vector2(uvx, uvytop); //Mathf.InverseLerp(minz, maxz, z));
        plane.UVs[i + numSegments + 1] = new Vector2(uvx, uvybottom);
    }

    private void GenerateLeftTriangle(ref int i, ref int j)
    {
        plane.Triangles[j++] = i;
        plane.Triangles[j++] = i + 1;
        plane.Triangles[j++] = i + numSegments + 1;
    }

    private void GenerateRightTriangle(ref int i, ref int j)
    {
        plane.Triangles[j++] = i + 1;
        plane.Triangles[j++] = i + numSegments + 2;
        plane.Triangles[j++] = i + numSegments + 1;
    }

}