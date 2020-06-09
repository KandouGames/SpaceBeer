using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory : MonoBehaviour
{
    private Curve curve;
    private float pipeRadius;

    private int nAsteroids = 40;
    private int nPlanetPortals = 2;
    private int nBarrelPortals = 2;
    private int nSnailPortals = 1;
    private int nShieldPortals = 1;

    private List<GameObject> asteroids;
    private List<GameObject> planetPortals;
    private List<GameObject> barrelPortals;
    private List<GameObject> snailPortals;
    private List<GameObject> shieldPortals;


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
        GenerateSnailPortals();
        GenerateShieldPortals();

        SetObstacles(Level.SuperEasy);
    }

    public void SetObstacles(Level level)
    {
        SetAsteroids(level);
        SetPlanetPortals(level);
        SetBarrelPortals(level);
        SetSnailPortals(level);
        SetShieldPortals(level);
    }

    public void GenerateAsteroids()
    {
        asteroids = new List<GameObject>();
        for (int i = 0; i < nAsteroids; ++i)
        {
            GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.Asteroid);
            asteroids.Add(obstacle);
            curve.AddObstacleToCurve(obstacle, Obstacle.obstType.Mesh);
        }
    }

    public void GenerateBarrelPortals()
    {
        barrelPortals = new List<GameObject>();
        for (int i = 0; i < nBarrelPortals; ++i)
        {
            GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.PortalBarrel);
            barrelPortals.Add(obstacle);
            curve.AddObstacleToCurve(obstacle, Obstacle.obstType.Texture);
        }
        
    }

    public void GeneratePlanetPortals()
    {
        planetPortals = new List<GameObject>();
        for (int i = 0; i < nPlanetPortals; ++i)
        {
            GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.PortalPlanet);
            planetPortals.Add(obstacle);
            curve.AddObstacleToCurve(obstacle, Obstacle.obstType.Texture);
        }
    }

    public void GenerateSnailPortals()
    {
        snailPortals = new List<GameObject>();
        for (int i = 0; i < nSnailPortals; ++i)
        {
            GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.PowerUpSnail);
            snailPortals.Add(obstacle);
            curve.AddObstacleToCurve(obstacle, Obstacle.obstType.Texture);
        }
    }

    public void GenerateShieldPortals()
    {
        shieldPortals = new List<GameObject>();
        for (int i = 0; i < nSnailPortals; ++i)
        {
            GameObject obstacle = DynamicPool.instance.GetObj(DynamicPool.objType.PowerUpShield);
            shieldPortals.Add(obstacle);
            curve.AddObstacleToCurve(obstacle, Obstacle.obstType.Texture);
        }
    }



    public void SetAsteroids(Level level)
    {
        float threshold;
        switch (level)
        {
            case Level.SuperEasy:
                threshold = 0.2f;
                break;
            case Level.Easy:
                threshold = 0.25f;
                break;
            case Level.Medium:
                threshold = 0.3f;
                break;
            case Level.Hard:
                threshold = 0.5f;
                break;
            case Level.God:
                threshold = 0.7f;
                break;
            default:
                threshold = 0.2f;
                break;
        }

        foreach (GameObject asteroid in asteroids)
        {
            asteroid.SetActive(false);
        }

        for (int i = 0; i < nAsteroids / 10; ++i)
        {
            float random = Random.Range(0.0f, 1.0f);

            if (random < threshold)
                SetRandomShapeObstacles(asteroids.GetRange(10 * i, 10));
        }
    }

    public void SetPlanetPortals(Level level)
    {
        float threshold;
        switch (level)
        {
            case Level.SuperEasy:
                threshold = 0.12f;
                break;
            case Level.Easy:
                threshold = 0.1f;
                break;
            case Level.Medium:
                threshold = 0.1f;
                break;
            case Level.Hard:
                threshold = 0.09f;
                break;
            case Level.God:
                threshold = 0.08f;
                break;
            default:
                threshold = 0.1f;
                break;
        }

        foreach (GameObject planetPortal in planetPortals)
        {
            planetPortal.transform.localScale = Vector3.one;
            planetPortal.SetActive(false);
            float random = Random.Range(0.0f, 1.0f);

            if (random < threshold)
                SetRandomPosObstacle(planetPortal);
        }

    }

    public void SetBarrelPortals(Level level)
    {
        float threshold;
        switch (level)
        {
            case Level.SuperEasy:
                threshold = 0.15f;
                break;
            case Level.Easy:
                threshold = 0.1f;
                break;
            case Level.Medium:
                threshold = 0.1f;
                break;
            case Level.Hard:
                threshold = 0.1f;
                break;
            case Level.God:
                threshold = 0.08f;
                break;
            default:
                threshold = 0.5f;
                break;
        }

        foreach (GameObject barrelPortal in barrelPortals)
        {
            barrelPortal.transform.localScale = Vector3.one;
            barrelPortal.SetActive(false);
            float random = Random.Range(0.0f, 1.0f);

            if (random < threshold)
                SetRandomPosObstacle(barrelPortal);
        }
    }

    public void SetShieldPortals(Level level)
    {
        float threshold;
        switch (level)
        {
            case Level.SuperEasy:
                threshold = 0.1f;
                break;
            case Level.Easy:
                threshold = 0.1f;
                break;
            case Level.Medium:
                threshold = 0.09f;
                break;
            case Level.Hard:
                threshold = 0.09f;
                break;
            case Level.God:
                threshold = 0.08f;
                break;
            default:
                threshold = 0.1f;
                break;
        }

        foreach (GameObject shieldPortal in shieldPortals)
        {
            shieldPortal.transform.localScale = Vector3.one;
            shieldPortal.SetActive(false);
            float random = Random.Range(0.0f, 1.0f);

            if (random < threshold)
                SetRandomPosObstacle(shieldPortal);
        }
    }

    public void SetSnailPortals(Level level)
    {
        float threshold;
        switch (level)
        {
            case Level.SuperEasy:
                threshold = 0.1f;
                break;
            case Level.Easy:
                threshold = 0.1f;
                break;
            case Level.Medium:
                threshold = 0.09f;
                break;
            case Level.Hard:
                threshold = 0.09f;
                break;
            case Level.God:
                threshold = 0.08f;
                break;
            default:
                threshold = 0.1f;
                break;
        }

        foreach (GameObject snailPortal in snailPortals)
        {
            snailPortal.transform.localScale = Vector3.one;
            snailPortal.SetActive(false);
            float random = Random.Range(0.0f, 1.0f);

            if (random < threshold)
                SetRandomPosObstacle(snailPortal);
        }
    }

    public void SetRandomPosObstacle(GameObject obstacle)
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
            SetRandomPosObstacle(obstacle);
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

    public void SetRandomShapeObstacles(List<GameObject> obstacles)
    {
        float random = Random.Range(0.0f, 1.0f);

        if (random < 0.33f)
            SetColumnObstacles(obstacles);
        else if (random < 0.66f)
            SetArrowObstacles(obstacles);
        else if (random < 1.0f)
            SetCircledObstacles(obstacles);
    }

    public void SetCircledObstacles(List<GameObject> obstacles)
    {
        Transform[] curvePoints = curve.GetCurveBasePoints();
        Transform randCurvePoint = curvePoints[Random.Range(0, curvePoints.Length)];

        float obstacleRadius = obstacles[0].GetComponent<CapsuleCollider>().radius;
        float radius = Random.Range((pipeRadius - obstacleRadius) * 0.4f, pipeRadius - obstacleRadius);

        Vector3 center = randCurvePoint.position;

        int nObstacles = obstacles.Count;
        for (int i = 0; i < nObstacles; ++i)
        {
            obstacles[i].SetActive(true);
            obstacles[i].GetComponent<CapsuleCollider>().isTrigger = true;

            obstacles[i].transform.position = center;
            obstacles[i].transform.rotation = randCurvePoint.rotation;

            float angle = 2.0f * Mathf.PI * ((float)i / nObstacles);
            float deltaXPos = Mathf.Cos(angle) * radius;
            float deltaYPos = Mathf.Sin(angle) * radius;

            obstacles[i].transform.position += obstacles[i].transform.right * deltaXPos + obstacles[i].transform.up * deltaYPos;
        }
    }

    public void SetArrowObstacles(List<GameObject> obstacles)
    {
        Transform[] curvePoints = curve.GetCurveBasePoints();
        Transform randCurvePoint = curvePoints[Random.Range(0, curvePoints.Length)];

        float obstacleRadius = obstacles[0].GetComponent<CapsuleCollider>().radius;
        float radius = Random.Range(-(pipeRadius - obstacleRadius) * 0.5f, (pipeRadius - obstacleRadius) * 0.5f);
        Vector3 center = randCurvePoint.position + randCurvePoint.up * radius;        

        int nObstacles = obstacles.Count;
        float step = (pipeRadius * 2.0f) / nObstacles;
        for (int i = 0; i < nObstacles; ++i)
        {
            obstacles[i].SetActive(true);
            obstacles[i].GetComponent<CapsuleCollider>().isTrigger = true;

            obstacles[i].transform.position = center;
            obstacles[i].transform.rotation = randCurvePoint.rotation;

            float deltaXPos = -pipeRadius + step * i;

            obstacles[i].transform.position += obstacles[i].transform.right * deltaXPos;

            if (Vector3.Distance(obstacles[i].transform.position, randCurvePoint.position) > (pipeRadius - obstacleRadius))
                obstacles[i].SetActive(false);
        }
    }

    public void SetColumnObstacles(List<GameObject> obstacles)
    {
        Transform[] curvePoints = curve.GetCurveBasePoints();
        Transform randCurvePoint = curvePoints[Random.Range(0, curvePoints.Length)];

        float obstacleRadius = obstacles[0].GetComponent<CapsuleCollider>().radius;
        float radius = Random.Range(-(pipeRadius - obstacleRadius) * 0.5f, (pipeRadius - obstacleRadius) * 0.5f);
        Vector3 center = randCurvePoint.position + randCurvePoint.right * radius;

        int nObstacles = obstacles.Count;
        float step = (pipeRadius * 2.0f) / nObstacles;
        for (int i = 0; i < nObstacles; ++i)
        {
            obstacles[i].SetActive(true);
            obstacles[i].GetComponent<CapsuleCollider>().isTrigger = true;

            obstacles[i].transform.position = center;
            obstacles[i].transform.rotation = randCurvePoint.rotation;

            float deltaYPos = -pipeRadius + step * i;

            obstacles[i].transform.position += obstacles[i].transform.up * deltaYPos;

            if (Vector3.Distance(obstacles[i].transform.position, randCurvePoint.position) > (pipeRadius - obstacleRadius))
                obstacles[i].SetActive(false);
        }
    }


}

