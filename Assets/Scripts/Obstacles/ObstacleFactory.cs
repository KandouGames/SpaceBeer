using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory : MonoBehaviour
{
    private Curve curve;
    private float pipeRadius;

    private List<GameObject> asteroids;
    private List<GameObject> planetPortals;
    private List<GameObject> barrelPortals;


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

        SetObstacles(Level.SuperEasy);
    }

    public void SetObstacles(Level level)
    {
        SetAsteroids(level);
        SetPlanetPortals(level);
        SetBarrelPortals(level);
    }

    public void GenerateAsteroids()
    {
        asteroids = new List<GameObject>();
        for (int i = 0; i < 8; ++i)
        {
            GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.Asteroid);
            asteroids.Add(obstacle);
            curve.AddObstacleToCurve(obstacle, Obstacle.obstType.Mesh);
        }
    }

    public void GenerateBarrelPortals()
    {
        barrelPortals = new List<GameObject>();
        for (int i = 0; i < 2; ++i)
        {
            GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.PortalBarrel);
            barrelPortals.Add(obstacle);
            curve.AddObstacleToCurve(obstacle, Obstacle.obstType.Texture);
        }
        
    }

    public void GeneratePlanetPortals()
    {
        planetPortals = new List<GameObject>();
        for (int i = 0; i < 2; ++i)
        {
            GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.PortalPlanet);
            planetPortals.Add(obstacle);
            curve.AddObstacleToCurve(obstacle, Obstacle.obstType.Texture);
        }
    }

    public void SetAsteroids(Level level)
    {
        float threshold;
        switch (level)
        {
            case Level.SuperEasy:
                threshold = 0.25f;
                break;
            case Level.Easy:
                threshold = 0.4f;
                break;
            case Level.Medium:
                threshold = 0.6f;
                break;
            case Level.Hard:
                threshold = 0.8f;
                break;
            case Level.God:
                threshold = 0.9f;
                break;
            default:
                threshold = 0.25f;
                break;
        }

        foreach (GameObject asteroid in asteroids)
        {
            asteroid.SetActive(false);
            float random = Random.Range(0.0f, 1.0f);

            if (random < threshold)
                SetObstacle(asteroid);
        }
        
    }

    public void SetPlanetPortals(Level level)
    {
        float threshold;
        switch (level)
        {
            case Level.SuperEasy:
                threshold = 0.5f;
                break;
            case Level.Easy:
                threshold = 0.4f;
                break;
            case Level.Medium:
                threshold = 0.35f;
                break;
            case Level.Hard:
                threshold = 0.3f;
                break;
            case Level.God:
                threshold = 0.2f;
                break;
            default:
                threshold = 0.5f;
                break;
        }

        foreach (GameObject planetPortal in planetPortals)
        {
            planetPortal.SetActive(false);
            float random = Random.Range(0.0f, 1.0f);

            if (random < threshold)
                SetObstacle(planetPortal);
        }

    }

    public void SetBarrelPortals(Level level)
    {
        float threshold;
        switch (level)
        {
            case Level.SuperEasy:
                threshold = 0.7f;
                break;
            case Level.Easy:
                threshold = 0.6f;
                break;
            case Level.Medium:
                threshold = 0.5f;
                break;
            case Level.Hard:
                threshold = 0.4f;
                break;
            case Level.God:
                threshold = 0.3f;
                break;
            default:
                threshold = 0.7f;
                break;
        }

        foreach (GameObject barrelPortal in barrelPortals)
        {
            barrelPortal.SetActive(false);
            float random = Random.Range(0.0f, 1.0f);

            if (random < threshold)
                SetObstacle(barrelPortal);
        }
    }

    public void SetObstacle(GameObject obstacle)
    {
        obstacle.SetActive(true);
        obstacle.GetComponent<CapsuleCollider>().isTrigger = true;

        Transform[] curvePoints = curve.GetCurveBasePoints();
        Transform randCurvePoint = curvePoints[Random.Range(0, curvePoints.Length)];

        float obstacleRadius = obstacle.GetComponent<CapsuleCollider>().radius;
        float angle = Random.Range(0, 2 * Mathf.PI);
        float radius = Random.Range(0, pipeRadius - obstacleRadius);

        float deltaXPos = Mathf.Cos(angle) * radius;
        float deltaYPos = Mathf.Sin(angle) * radius;

        obstacle.transform.position = randCurvePoint.position;
        obstacle.transform.rotation = randCurvePoint.rotation;

        obstacle.transform.position += obstacle.transform.right * deltaXPos + obstacle.transform.up * deltaYPos;

        if (DetectOverlap(obstacle))
        {
            SetObstacle(obstacle);
        }
    }

    public bool DetectOverlap(GameObject obstacle)
    {
        foreach(GameObject asteroid in asteroids)
        {
            if(asteroid.activeSelf && asteroid.GetInstanceID() != obstacle.GetInstanceID())
            {
                if (Vector3.Distance(obstacle.transform.position, asteroid.transform.position) < (obstacle.GetComponent<CapsuleCollider>().radius + asteroid.GetComponent<CapsuleCollider>().radius))
                    return true;
            }
        }

        foreach(GameObject barrelPortal in barrelPortals)
        {
            if (barrelPortal.activeSelf && barrelPortal.GetInstanceID() != obstacle.GetInstanceID())
            {
                if (Vector3.Distance(obstacle.transform.position, barrelPortal.transform.position) < (obstacle.GetComponent<CapsuleCollider>().radius + barrelPortal.GetComponent<CapsuleCollider>().radius))
                    return true;
            }
        }

        foreach(GameObject planetPortal in planetPortals)
        {
            if (planetPortal.activeSelf && planetPortal.GetInstanceID() != obstacle.GetInstanceID())
            {
                if (Vector3.Distance(obstacle.transform.position, planetPortal.transform.position) < (obstacle.GetComponent<CapsuleCollider>().radius + planetPortal.GetComponent<CapsuleCollider>().radius))
                    return true;
            }
        }

        return false;
    }


}

