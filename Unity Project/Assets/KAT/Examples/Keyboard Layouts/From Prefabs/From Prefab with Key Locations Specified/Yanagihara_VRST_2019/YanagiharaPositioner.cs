using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class YanagiharaPositioner : MonoBehaviour
{
    [Button]
    public void Position()
    {
        List<MeshFilter> cubes = GetComponentsInChildren<MeshFilter>().ToList();
        cubes.Reverse();
        GameObject pivot = transform.Find("Pivot Point").gameObject;

        List<List<MeshFilter>> rows = new List<List<MeshFilter>>();
        rows.Add(cubes.GetRange(0, 10));
        rows.Add(cubes.GetRange(10, 9));
        rows.Add(cubes.GetRange(19, 7));
        rows.Reverse();

        float totalAngle = 160.0f;
        float yPositionOffset = 0.08f;
        float yPosition = -1.5f * yPositionOffset;

        foreach (List<MeshFilter> row in rows)
        {
            float anglePerCube = totalAngle / row.Count();
            float currentAngle = totalAngle / 2 * -1;
            foreach (MeshFilter key in row)
            {
                key.transform.position = RotateAroundPivotExtensions.RotateAroundPivot(new Vector3(0,0,-0.2f), pivot.transform.position, Quaternion.Euler(0, currentAngle, 0)) + new Vector3(0,yPosition,0);
                currentAngle += anglePerCube;
                key.transform.LookAt(pivot.transform);
                
            }
            yPosition += yPositionOffset;
            totalAngle += 10;
        }
    }
}
