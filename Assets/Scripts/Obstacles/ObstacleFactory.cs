using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory : MonoBehaviour
{
    private Curve curve;
    private float pipeRadius;

    public void SetCurve(Curve curve)
    {
        this.curve = curve;
        pipeRadius = curve.pipeRadius;
    }


    public void GenerateObstacles()
    {
        GenerateAsteroids();
        GeneratePlanetPortals();
        GenerateBarrelPortals();
    }

    public void GenerateAsteroids()
    {

        for (int i = 0; i < 2; ++i)
        {
            GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.Asteroid);

            Transform[] curvePoints = curve.GetCurveBasePoints();
            Transform randCurvePoint = curvePoints[Random.Range(0, curvePoints.Length)];

            float obstacleRadius = obstacle.GetComponent<CapsuleCollider>().radius;
            float deltaXPos = Random.Range(-pipeRadius + obstacleRadius, pipeRadius - obstacleRadius);
            float deltaYPos = Random.Range(-pipeRadius + obstacleRadius, pipeRadius - obstacleRadius);

            obstacle.transform.position = randCurvePoint.position;
            obstacle.transform.rotation = randCurvePoint.rotation;

            obstacle.transform.SetParent(randCurvePoint.parent);
            obstacle.transform.position += obstacle.transform.right * deltaXPos + obstacle.transform.up * deltaYPos;
            obstacle.transform.Rotate(new Vector3(0, Random.Range(0, 180), 0));
        }
        
    }

    public void GenerateBarrelPortals()
    {
        GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.PortalBarrel);

        Transform[] curvePoints = curve.GetCurveBasePoints();
        Transform randCurvePoint = curvePoints[Random.Range(0, curvePoints.Length)];

        float obstacleRadius = obstacle.GetComponent<CapsuleCollider>().radius;
        float deltaXPos = Random.Range(-pipeRadius + obstacleRadius, pipeRadius - obstacleRadius);
        float deltaYPos = Random.Range(-pipeRadius + obstacleRadius, pipeRadius - obstacleRadius);

        obstacle.transform.position = randCurvePoint.position;
        obstacle.transform.rotation = randCurvePoint.rotation;

        obstacle.transform.SetParent(randCurvePoint.parent);
        obstacle.transform.position += obstacle.transform.right * deltaXPos + obstacle.transform.up * deltaYPos;
    }

    public void GeneratePlanetPortals()
    {
        GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.PortalPlanet);

        Transform[] curvePoints = curve.GetCurveBasePoints();
        Transform randCurvePoint = curvePoints[Random.Range(0, curvePoints.Length)];

        float obstacleRadius = obstacle.GetComponent<CapsuleCollider>().radius;
        float deltaXPos = Random.Range(-pipeRadius + obstacleRadius, pipeRadius - obstacleRadius);
        float deltaYPos = Random.Range(-pipeRadius + obstacleRadius, pipeRadius - obstacleRadius);

        obstacle.transform.position = randCurvePoint.position;
        obstacle.transform.rotation = randCurvePoint.rotation;

        obstacle.transform.SetParent(randCurvePoint.parent);
        obstacle.transform.position += obstacle.transform.right * deltaXPos + obstacle.transform.up * deltaYPos;
    }


}

