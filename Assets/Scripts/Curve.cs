using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Curve : MonoBehaviour
{
    const int QUAD_NUM_OF_VERTEX = 4;

    public float pipeRadius;
    [Tooltip("The number of segments that will form the pipe")]
    public int pipeSegmentCount;

    //Ring distance will be divided by the curve radius to calculate how many chunks of the curve/pipe we will make
    [Tooltip("Length of the curve before starting a new one")]
    [Range(0.5f, 1.8f)]
    public float ringDistance;

    [Tooltip("Determines how much the curve will bend. Random between both")]
    [BoxGroup("Curvature")]
    public float minCurveRadius, maxCurveRadius;

    [Tooltip("The number of segments that will contribute to this curve. Random between both")]
    [BoxGroup("NumOfSegments")]
    public int minCurveSegmentCount, maxCurveSegmentCount;

    public bool renderPipes = false;

    private float curveRadius;
    private int curveSegmentCount;
    private float curveAngle;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    [SerializeField]
    private Transform[] curveBasePoints;

    private MeshFilter curveFilter;

    /// <summary>
    /// Generates a curve
    /// </summary>
    /// <param name="renderPipes">Generate a pipe too</param>
    public void Create(bool renderPipes)
    {
        curveFilter = GetComponent<MeshFilter>();
        curveFilter.mesh = mesh = new Mesh();
        mesh.name = "Pipe";
        this.renderPipes = renderPipes;

        if (!renderPipes)
            GetComponent<MeshRenderer>().enabled = false;

        curveRadius = Random.Range(minCurveRadius, maxCurveRadius);
        curveSegmentCount = Random.Range(minCurveSegmentCount, maxCurveSegmentCount + 1);

        curveBasePoints = new Transform[curveSegmentCount + 1];

        for (int i = 0; i < curveBasePoints.Length; i++)
        {
            curveBasePoints[i] = new GameObject().transform;
            curveBasePoints[i].transform.parent = this.transform;
            curveBasePoints[i].transform.name = "point " + i;
        }

        //TODO: Make curves completely independent of the torus

        if (true /*renderPipes*/)
        {
            SetVertices();
            SetTriangles();
            mesh.RecalculateNormals();
        }
        else
        {
            float curveStep = ringDistance / curveRadius;
            Vector3 transformPosition = Vector3.zero;

            vertices = new Vector3[curveSegmentCount + 1];

            for (int i = 0; i < curveBasePoints.Length; i++)
            {
                curveBasePoints[i].position = vertices[i] = GetPointCurve(i * curveStep);

                transformPosition += curveBasePoints[i].position;
            }
            mesh.vertices = vertices;
        }

    }

    private void SetVertices()
    {
        vertices = new Vector3[pipeSegmentCount * curveSegmentCount * QUAD_NUM_OF_VERTEX];

        float curveStep = ringDistance / curveRadius;
        curveAngle = curveStep * curveSegmentCount * (360f / (2f * Mathf.PI));
        int iDelta = pipeSegmentCount * QUAD_NUM_OF_VERTEX;

        CreateFirstQuadRing(curveStep);

        for (int u = 2, i = iDelta; u <= curveSegmentCount; u++, i += iDelta)
        {
            CreateQuadRing(u, u * curveStep, i);
        }

        mesh.vertices = vertices;
    }

    private void SetTriangles()
    {
        triangles = new int[pipeSegmentCount * curveSegmentCount * 6];
        for (int t = 0, i = 0; t < triangles.Length; t += 6, i += QUAD_NUM_OF_VERTEX)
        {
            triangles[t] = i;
            triangles[t + 1] = triangles[t + 4] = i + 1;
            triangles[t + 2] = triangles[t + 3] = i + 2;
            triangles[t + 5] = i + 3;
        }
        mesh.triangles = triangles;
    }


    private void CreateFirstQuadRing(float curveStep)
    {
        float vStep = (2f * Mathf.PI) / pipeSegmentCount;

        Vector3 vertexA = GetPointTorus(0f, 0f);
        Vector3 vertexB = GetPointTorus(curveStep, 0f);

        curveBasePoints[0].position = GetPointCurve(0);
        faceForward(curveBasePoints[0]);
        curveBasePoints[1].position = GetPointCurve(curveStep);
        faceForward(curveBasePoints[1]);

        for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i += QUAD_NUM_OF_VERTEX)
        {
            vertices[i] = vertexA;
            vertices[i + 1] = vertexA = GetPointTorus(0f, v * vStep);
            vertices[i + 2] = vertexB;
            vertices[i + 3] = vertexB = GetPointTorus(curveStep, v * vStep);
        }
    }

    private void CreateQuadRing(int curveSegment, float curveStep, int i)
    {
        float vStep = (2f * Mathf.PI) / pipeSegmentCount;
        int ringOffset = pipeSegmentCount * QUAD_NUM_OF_VERTEX;

        Vector3 vertex = GetPointTorus(curveStep, 0f);
        curveBasePoints[(int)curveSegment].position = GetPointCurve(curveStep);
        faceForward(curveBasePoints[(int)curveSegment]);

        for (int v = 1; v <= pipeSegmentCount; v++, i += QUAD_NUM_OF_VERTEX)
        {
            vertices[i] = vertices[i - ringOffset + 2];
            vertices[i + 1] = vertices[i - ringOffset + 3];
            vertices[i + 2] = vertex;
            vertices[i + 3] = vertex = GetPointTorus(curveStep, v * vStep);
        }
    }


    private Vector3 GetPointTorus(float curveStep, float curveSegmentLenght)
    {
        Vector3 p;
        float r = (curveRadius + pipeRadius * Mathf.Cos(curveSegmentLenght));
        p.x = r * Mathf.Sin(curveStep);
        p.y = r * Mathf.Cos(curveStep);
        p.z = pipeRadius * Mathf.Sin(curveSegmentLenght);
        return p;
    }

    private Vector3 GetPointCurve(float curveStep)
    {
        Vector3 p;
        float r = (curveRadius);
        p.x = r * Mathf.Sin(curveStep);
        p.y = r * Mathf.Cos(curveStep);
        p.z = 0;
        return p;
    }

    private void faceForward(Transform point)
    {
        Vector3 right = point.right;
        point.forward = point.right;
    }

    public void AlignWith(Curve curve)
    {
        if (true/*renderPipes*/)
        {
            //For aligning meshes
            float relativeRotation = Random.Range(0, curveSegmentCount) * 360f / pipeSegmentCount;

            //Set the previous curve as a parent
            transform.SetParent(curve.transform, false);

            //Place at the beggining of the curve with the opposite rotation
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0f, 0f, -curve.curveAngle);

            //Place the curve
            transform.Translate(0f, curve.curveRadius, 0f);
            transform.Rotate(relativeRotation, 0f, 0f);
            transform.Translate(0f, -curveRadius, 0f);

            //Change parent to curve manager
            transform.SetParent(curve.transform.parent);
        }
        else
        {
            /*Align only the points to the previous curve*/
        }

    }

    public Transform[] getCurveBasePoints()
    {
        return curveBasePoints;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < curveBasePoints.Length; i++)
        {
            Gizmos.DrawSphere(curveBasePoints[i].position, 0.1f);
        }
    }
}
