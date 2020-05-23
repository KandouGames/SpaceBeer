using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject Generate(Curve curve)
    {
        Transform[] curvePoints = curve.getCurveBasePoints();
        Transform randPoint = curvePoints[Random.Range(0, curvePoints.Length)];
        float xPos = Random.Range(-curve.pipeRadius, curve.pipeRadius);
        float yPos = Random.Range(-curve.pipeRadius, curve.pipeRadius);

        GameObject obstacle = Instantiate(asteroid, randPoint.position, randPoint.rotation);

        obstacle.transform.parent = curve.transform;
        obstacle.transform.forward = randPoint.forward;
        obstacle.transform.localPosition = new Vector3(obstacle.transform.localPosition.x + curve.pipeRadius, obstacle.transform.localPosition.y, obstacle.transform.localPosition.z);

        return obstacle;
    }
}
