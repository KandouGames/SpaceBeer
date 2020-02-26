using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{
    public float pipeRadius;
    public int pipeSegmentCount;
    public float ringDistance;

    public float minCurveRadius, maxCurveRadius;
    public int minCurveSegmentCount, maxCurveSegmentCount;

    private float curveRadius;
    private int curveSegmentCount;
    private float curveAngle;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Pipe";

        curveRadius = Random.Range(minCurveRadius, maxCurveRadius);
        curveSegmentCount =
            Random.Range(minCurveSegmentCount, maxCurveSegmentCount + 1);

        SetVertices();
        SetTriangles();
        mesh.RecalculateNormals();
    }

    private void CreateFirstQuadRing(float u)
    {
        float vStep = (2f * Mathf.PI) / pipeSegmentCount;

        Vector3 vertexA = GetPointTorus(0f, 0f);
        Vector3 vertexB = GetPointTorus(u, 0f);
        for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i += 4)
        {
            vertices[i] = vertexA;
            vertices[i + 1] = vertexA = GetPointTorus(0f, v * vStep);
            vertices[i + 2] = vertexB;
            vertices[i + 3] = vertexB = GetPointTorus(u, v * vStep);
        }
    }
    private void CreateQuadRing(float u, int i)
    {
        float vStep = (2f * Mathf.PI) / pipeSegmentCount;
        int ringOffset = pipeSegmentCount * 4;

        Vector3 vertex = GetPointTorus(u, 0f);
        for (int v = 1; v <= pipeSegmentCount; v++, i += 4)
        {
            vertices[i] = vertices[i - ringOffset + 2];
            vertices[i + 1] = vertices[i - ringOffset + 3];
            vertices[i + 2] = vertex;
            vertices[i + 3] = vertex = GetPointTorus(u, v * vStep);
        }
    }


    /// <summary>
    /// Generates a specific point in a torus 
    /// </summary>
    /// <param name="theta">Angle along the curve</param>
    /// <param name="phi">angle along the pipe</param>
    /// <returns></returns>
    private Vector3 GetPointTorus(float theta, float phi)
    {
        Vector3 p;
        float r = (curveRadius + pipeRadius * Mathf.Cos(phi));
        p.x = r * Mathf.Sin(theta);
        p.y = r * Mathf.Cos(theta);
        p.z = pipeRadius * Mathf.Sin(phi);
        return p;
    }

    public void AlignWith(Curve curve)
    {
        float relativeRotation = Random.Range(0, curveSegmentCount) * 360f / pipeSegmentCount;

        transform.SetParent(curve.transform, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, -curve.curveAngle);
        transform.Translate(0f, curve.curveRadius, 0f);
        transform.Rotate(relativeRotation, 0f, 0f);
        transform.Translate(0f, -curveRadius, 0f);
        transform.SetParent(curve.transform.parent);
    }

    private void SetVertices()
    {
        vertices = new Vector3[pipeSegmentCount * curveSegmentCount * 4];

        float uStep = ringDistance / curveRadius;
        curveAngle = uStep * curveSegmentCount * (360f / (2f * Mathf.PI));
        int iDelta = pipeSegmentCount * 4;
        CreateFirstQuadRing(uStep);
        for (int u = 2, i = iDelta; u <= curveSegmentCount; u++, i += iDelta)
        {
            CreateQuadRing(u * uStep, i);
        }
        mesh.vertices = vertices;
    }

    private void SetTriangles()
    {
        triangles = new int[pipeSegmentCount * curveSegmentCount * 6];
        for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4)
        {
            triangles[t] = i;
            triangles[t + 1] = triangles[t + 4] = i + 1;
            triangles[t + 2] = triangles[t + 3] = i + 2;
            triangles[t + 5] = i + 3;
        }
        mesh.triangles = triangles;
    }

    /* private void OnDrawGizmos()
     {
         float uStep = (2f * Mathf.PI) / curveSegmentCount;
         float vStep = (2f * Mathf.PI) / pipeSegmentCount;

         for (int u = 0; u < curveSegmentCount; u++)
         {
             for (int v = 0; v < pipeSegmentCount; v++)
             {
                 Vector3 point = GetPointTorus(u * uStep, v * vStep);
                 Gizmos.color = new Color(
                     1f,
                     (float)v / pipeSegmentCount,
                     (float)u / curveSegmentCount);
                 Gizmos.DrawSphere(point, 0.1f);
             }
         }
     }*/
}
