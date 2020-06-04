using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


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
    public float minCurveRadius, maxCurveRadius;

    [Tooltip("The number of segments that will contribute to this curve. Random between both")]
    public int minCurveSegmentCount, maxCurveSegmentCount;

    public bool renderPipes = false;

    private float torusRadius;
    private int curveSegmentCount;
    private float curveAngle;
    private float relativeRotation = 0;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private Transform[] curveBasePoints;

    private MeshFilter curveFilter;

    private ObstacleFactory obstacleFactory;

    [HideInInspector]
    public GameObject obstacleHolder;
    public List<Obstacle> obstacleList { get; private set; }


    /// <summary>
    /// Generates a curve
    /// </summary>
    /// <param name="renderPipes">Generate a pipe too</param>
    public void Create(bool renderPipes)
    {

        InitializeData(renderPipes);
        SetVertices();
        SetTriangles();
        mesh.RecalculateNormals();

        AlignCurveBasePoints();

        obstacleFactory = this.gameObject.AddComponent<ObstacleFactory>();
        obstacleFactory.SetCurve(this);
    }

    private void InitializeData(bool renderPipes)
    {
        curveFilter = GetComponent<MeshFilter>();
        curveFilter.mesh = mesh = new Mesh();

        mesh.name = "Pipe";
        this.renderPipes = renderPipes;

        if (!renderPipes)
            GetComponent<MeshRenderer>().enabled = false;

        torusRadius = Random.Range(minCurveRadius, maxCurveRadius);
        curveSegmentCount = Random.Range(minCurveSegmentCount, maxCurveSegmentCount + 1);

        curveBasePoints = new Transform[curveSegmentCount + 1];
        GameObject curveBasePointsHolder = new GameObject();
        curveBasePointsHolder.name = "CurvePoints";
        curveBasePointsHolder.transform.parent = this.transform;

        for (int i = 0; i < curveBasePoints.Length; i++)
        {
            curveBasePoints[i] = new GameObject().transform;
            curveBasePoints[i].transform.parent = curveBasePointsHolder.transform;
            curveBasePoints[i].transform.name = "point " + i;
        }

        obstacleList = new List<Obstacle>();
        obstacleHolder = new GameObject();
        obstacleHolder.name = "CurveObstacles";
        obstacleHolder.transform.parent = this.transform;
    }

    private void SetVertices()
    {
        vertices = new Vector3[pipeSegmentCount * curveSegmentCount * QUAD_NUM_OF_VERTEX];

        float curveStep = ringDistance / torusRadius;
        curveAngle = curveStep * curveSegmentCount * (360f / (2f * Mathf.PI));
        CreateFirstQuadRing(curveStep);

        int iDelta = pipeSegmentCount * QUAD_NUM_OF_VERTEX;

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
        curveBasePoints[1].position = GetPointCurve(curveStep);

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
        float r = (torusRadius + pipeRadius * Mathf.Cos(curveSegmentLenght));
        p.x = r * Mathf.Sin(curveStep);
        p.y = r * Mathf.Cos(curveStep);
        p.z = pipeRadius * Mathf.Sin(curveSegmentLenght);
        return p;
    }

    private Vector3 GetPointCurve(float curveStep)
    {
        Vector3 p;
        float r = (torusRadius);
        p.x = r * Mathf.Sin(curveStep);
        p.y = r * Mathf.Cos(curveStep);
        p.z = 0;
        return p;
    }

    private void AlignCurveBasePoints()
    {
        for (int i = 0; i < curveBasePoints.Length - 1; i++)
        {
            curveBasePoints[i].transform.right = Vector3.right;
            curveBasePoints[i].transform.forward = (curveBasePoints[i + 1].transform.position - curveBasePoints[i].transform.position).normalized;
        }

        curveBasePoints[curveBasePoints.Length - 1].right = curveBasePoints[curveBasePoints.Length - 2].right;
        curveBasePoints[curveBasePoints.Length - 1].forward = curveBasePoints[curveBasePoints.Length - 2].forward;
    }

    public void AlignWith(Curve curve)
    {

        //For aligning meshes
        relativeRotation = Random.Range(0, curveSegmentCount) * 360f / pipeSegmentCount;

        //Set the previous curve as a parent
        transform.SetParent(curve.transform, false);

        //Place at the beggining of the curve with the opposite rotation
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, -curve.curveAngle);

        //Place the curve
        transform.Translate(0f, curve.torusRadius, 0f);
        transform.Rotate(relativeRotation, 0f, 0f);
        transform.Translate(0f, -torusRadius, 0f);

        //Change parent to curve manager
        transform.SetParent(curve.transform.parent);

        transform.localScale = Vector3.one;

        //Align last basePoint of curve
        curve.curveBasePoints[curve.curveBasePoints.Length - 1].right = this.curveBasePoints[0].right;
        curve.curveBasePoints[curve.curveBasePoints.Length - 1].forward = this.curveBasePoints[0].forward;

        transform.localScale = Vector3.one;

    }

    public void GenerateObstacles()
    {
        obstacleFactory.GenerateObstacles();
    }

    public void SetObstacles(Level level)
    {
        obstacleFactory.SetObstacles(level);
    }

    public void AddObstacleToCurve(GameObject obstacle, Obstacle.obstType obstType)
    {
        obstacle.transform.parent = obstacleHolder.transform;
        Obstacle currentObstacle = new Obstacle(obstacle, false, obstType);
        obstacleList.Add(currentObstacle);
    }

    public void DeleteObstacleFromCurve(Obstacle obstacle)
    {
        if (obstacleList.Contains(obstacle))
        {
            obstacleList.Remove(obstacle);
        }
        else
        {
            Debug.LogWarning("<color=yellow>You are trying to remove" + obstacle +
            "from the curve but it is not stored as an obstacle</color>");
        }
        obstacle.obstacleObj.transform.parent = obstacleHolder.transform;
    }

    public Transform[] GetCurveBasePoints()
    {
        return curveBasePoints;
    }

    public float GetTorusRadius()
    {
        return torusRadius;
    }

    public float GetCurveAngle()
    {
        return curveAngle;
    }

    public float GetRelativeRotation()
    {
        return relativeRotation;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < curveBasePoints.Length; i++)
        {
            Gizmos.DrawSphere(curveBasePoints[i].position, 0.1f);
        }
    }
}


public class Obstacle
{
    public enum obstType { Mesh, Texture }

    public Obstacle(GameObject obstacle, bool faded, obstType obstType)
    {
        this.obstacleObj = obstacle;
        this.hasFaded = faded;
        this.type = obstType;
    }
    public float fadeOutTime = 0.5f;
    public float fadeInTime = 3f;

    public GameObject obstacleObj { get; private set; }
    public bool hasFaded;
    public obstType type { get; private set; }

    public void FadeOutObstacle()
    {
        Material matColor;
        if (type == obstType.Mesh)
            matColor = obstacleObj.GetComponentInChildren<MeshRenderer>().material;
        else
        {
            matColor = obstacleObj.GetComponentInChildren<SpriteRenderer>().material;
            obstacleObj.GetComponentInChildren<PortalAnimation>()?.Deactivate(fadeOutTime);
        }

        matColor?.DOFade(0.25f, fadeOutTime);

        hasFaded = true;
    }

    public void FadeInObstacle()
    {
        Material matColor;
        if (type == obstType.Mesh)
            matColor = obstacleObj.GetComponentInChildren<MeshRenderer>().material;
        else
        {
            matColor = obstacleObj.GetComponentInChildren<SpriteRenderer>().material;
            obstacleObj.GetComponentInChildren<PortalAnimation>()?.Activate(fadeInTime);
        }
        matColor?.DOFade(1, fadeInTime);
        hasFaded = false;
    }
}
