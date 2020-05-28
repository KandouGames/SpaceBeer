using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject Generate(Curve curve)
    {
        Transform[] curvePoints = curve.GetCurveBasePoints();
        Transform randCurvePoint = curvePoints[Random.Range(0, curvePoints.Length)];

        float deltaXPos = Random.Range(-curve.pipeRadius * 0.8f, curve.pipeRadius * 0.8f);
        float deltaYPos = Random.Range(-curve.pipeRadius * 0.8f, curve.pipeRadius * 0.8f);

        // GameObject obstacle = Instantiate(asteroid, randCurvePoint.position, randCurvePoint.rotation);

        GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.Asteroid);
        Debug.Log(obstacle);
        obstacle.transform.position = randCurvePoint.position;
        obstacle.transform.rotation = randCurvePoint.rotation;

        obstacle.transform.SetParent(randCurvePoint.parent);
        obstacle.transform.position += obstacle.transform.right * deltaXPos + obstacle.transform.up * deltaYPos;
        obstacle.transform.Rotate(new Vector3(0, Random.Range(0, 180), 0));

        return obstacle;
    }
}
