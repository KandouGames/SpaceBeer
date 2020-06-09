using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not happy about this, too difficult to extend
public class DynamicPool : MonoBehaviour
{
    private GameObject bulletPrefab;
    private List<GameObject> asteroidPrefabList;
    private GameObject portalBarrelPrefab;
    private GameObject portalPlanetPrefab;
    private GameObject darkHolePrefab;
    private GameObject powerUpShieldPrefab;
    private GameObject powerUpSnailPrefab;
    private GameObject powerUpLaserPrefab;
    private GameObject powerUpGrenadePrefab;

    private int numToCreateBullets = 10;
    [Tooltip("num of asteroids to create of <bold> each asteroid type </bold>")]
    private int numToCreateAsteroids = 40;
    private int numToCreatePortalBarrels = 10;
    private int numToCreatePortalPlanets = 10;
    private int numToCreateDarkHoles = 10;
    private int numToCreatePowerUpShield = 5;
    private int numToCreatePowerUpSnail = 5;
    private int numToCreatePowerUpLaser = 5;
    private int numToCreatePowerUpGrenade = 5;

    private Queue<GameObject> bulletQueue;
    private Queue<GameObject> asteroidQueue;
    private Queue<GameObject> portalBarrelQueue;
    private Queue<GameObject> portalPlanetQueue;
    private Queue<GameObject> darkHoleQueue;
    private Queue<GameObject> powerUpShieldQueue;
    private Queue<GameObject> powerUpSnailQueue;
    private Queue<GameObject> powerUpLaserQueue;
    private Queue<GameObject> powerUpGrenadedQueue;

    private GameObject curveElements;
    private GameObject bulletElements;

    private static DynamicPool _instance = null;
    private bool shouldExpand = true;
    public enum objType
    {
        Bullet,
        Asteroid,
        PortalBarrel,
        PortalPlanet,
        DarkHole,
        PowerUpShield,
        PowerUpSnail,
        PowerUpLaser,
        PowerUpGrenade,
    };


    public static DynamicPool instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<DynamicPool>();
                _instance.name = "DynamicPool";
            }
            return _instance;
        }
    }

    private void Awake()
    {
        curveElements = new GameObject();
        curveElements.transform.parent = this.transform;
        curveElements.name = "CurveElements";

        bulletElements = new GameObject();
        bulletElements.transform.parent = this.transform;
        bulletElements.name = "Bullets";
    }

    public void Generate(objType objToCreate, GameObject prefab)
    {
        bool queueWasNull = false;
        Queue<GameObject> currentQueue = GetObjQueue(objToCreate, ref queueWasNull);
        if (queueWasNull)
        {
            int numToCreate = 0;

            switch (objToCreate)
            {
                case objType.Bullet:
                    numToCreate = numToCreateBullets;
                    this.bulletPrefab = prefab;
                    break;
                case objType.Asteroid:
                    numToCreate = numToCreateAsteroids;
                    this.asteroidPrefabList = new List<GameObject>();
                    this.asteroidPrefabList.Add(prefab);
                    break;
                case objType.PortalBarrel:
                    numToCreate = numToCreatePortalBarrels;
                    this.portalBarrelPrefab = prefab;
                    break;
                case objType.PortalPlanet:
                    numToCreate = numToCreatePortalPlanets;
                    this.portalPlanetPrefab = prefab;
                    break;
                case objType.DarkHole:
                    numToCreate = numToCreateDarkHoles;
                    this.darkHolePrefab = prefab;
                    break;
                case objType.PowerUpShield:
                    numToCreate = numToCreatePowerUpShield;
                    this.powerUpShieldPrefab = prefab;
                    break;
                case objType.PowerUpSnail:
                    numToCreate = numToCreatePowerUpSnail;
                    this.powerUpSnailPrefab = prefab;
                    break;
                case objType.PowerUpLaser:
                    numToCreate = numToCreatePowerUpLaser;
                    this.powerUpLaserPrefab = prefab;
                    break;
                case objType.PowerUpGrenade:
                    numToCreate = numToCreatePowerUpGrenade;
                    this.powerUpGrenadePrefab = prefab;
                    break;
                default:
                    Debug.LogError("<color=red> Dynamic pool received an incorrect object type </color>");
                    break;
            }

            for (int i = 0; i < numToCreate; i++)
            {
                currentQueue.Enqueue(CreateSingleObject(objToCreate));
            }
        }
    }


    public GameObject GetObj(objType objToCreate)
    {
        GameObject currentObj;
        bool checkIfWasNull = false;
        Queue<GameObject> currentQueue = GetObjQueue(objToCreate, ref checkIfWasNull);

        if (currentQueue.Count != 0)
        {
            currentObj = currentQueue.Dequeue();
            currentObj.SetActive(true);
        }
        else
        {
            currentObj = CreateSingleObject(objToCreate);
            currentObj.SetActive(true);
        }
        return currentObj;
    }

    public void ResetObj(GameObject obj, objType objTypeOfObjToReset)
    {
        bool checkIfWasNull = false;
        Queue<GameObject> currentQueue = GetObjQueue(objTypeOfObjToReset, ref checkIfWasNull);

        SetInactiveObjParent(objTypeOfObjToReset, obj);

        obj.SetActive(false);
        currentQueue.Enqueue(obj);
    }

    private GameObject CreateSingleObject(objType objToCreate)
    {
        GameObject currentObj = Instantiate(GetPrefabType(objToCreate));

        SetInactiveObjParent(objToCreate, currentObj);

        currentObj.name = Enum.GetName(typeof(objType), objToCreate);
        currentObj.SetActive(false);
        return currentObj;
    }

    private void SetInactiveObjParent(objType objToCreate, GameObject currentObj)
    {
        //Only bullets are curve independent
        if (objType.Bullet == objToCreate)
            currentObj.transform.parent = bulletElements.transform;
        else
            currentObj.transform.parent = curveElements.transform;
    }

    //FIXME:Should move the check and creation to another function 
    private Queue<GameObject> GetObjQueue(objType objToCreate, ref bool queueWasNull)
    {
        Queue<GameObject> currentQueue;

        switch (objToCreate)
        {
            case objType.Asteroid:
                if (asteroidQueue is null)
                {
                    queueWasNull = true;
                    asteroidQueue = new Queue<GameObject>();
                }
                currentQueue = asteroidQueue;
                break;
            case objType.Bullet:
                if (bulletQueue is null)
                {
                    queueWasNull = true;
                    bulletQueue = new Queue<GameObject>();
                }
                currentQueue = bulletQueue;
                break;
            case objType.PortalBarrel:
                if (portalBarrelQueue is null)
                {
                    queueWasNull = true;
                    portalBarrelQueue = new Queue<GameObject>();
                }
                currentQueue = portalBarrelQueue;
                break;
            case objType.PortalPlanet:
                if (portalPlanetQueue is null)
                {
                    queueWasNull = true;
                    portalPlanetQueue = new Queue<GameObject>();
                }
                currentQueue = portalPlanetQueue;
                break;
            case objType.DarkHole:
                if (darkHoleQueue is null)
                {
                    queueWasNull = true;
                    darkHoleQueue = new Queue<GameObject>();
                }
                currentQueue = darkHoleQueue;
                break;
            case objType.PowerUpShield:
                if (powerUpShieldQueue is null)
                {
                    queueWasNull = true;
                    powerUpShieldQueue = new Queue<GameObject>();
                }
                currentQueue = powerUpShieldQueue;
                break;
            case objType.PowerUpSnail:
                if (powerUpSnailQueue is null)
                {
                    queueWasNull = true;
                    powerUpSnailQueue = new Queue<GameObject>();
                }
                currentQueue = powerUpSnailQueue;
                break;
            case objType.PowerUpLaser:
                if (powerUpLaserQueue is null)
                {
                    queueWasNull = true;
                    powerUpLaserQueue = new Queue<GameObject>();
                }
                currentQueue = powerUpLaserQueue;
                break;
            case objType.PowerUpGrenade:
                if (powerUpGrenadedQueue is null)
                {
                    queueWasNull = true;
                    powerUpGrenadedQueue = new Queue<GameObject>();
                }
                currentQueue = powerUpGrenadedQueue;
                break;
            default:
                Debug.LogError("<color=red> Dynamic pool received an incorrect object type </color>");
                currentQueue = asteroidQueue;
                break;
        }
        return currentQueue;
    }


    private GameObject GetPrefabType(objType objToCreate)
    {
        GameObject prefabToGenerate;
        switch (objToCreate)
        {
            case objType.Asteroid:
                int randomAsteroid = UnityEngine.Random.Range(0, asteroidPrefabList.Count);
                prefabToGenerate = asteroidPrefabList[randomAsteroid];
                break;
            case objType.Bullet:
                prefabToGenerate = this.bulletPrefab;
                break;
            case objType.PortalBarrel:
                prefabToGenerate = this.portalBarrelPrefab;
                break;
            case objType.PortalPlanet:
                prefabToGenerate = this.portalPlanetPrefab;
                break;
            case objType.DarkHole:
                prefabToGenerate = this.darkHolePrefab;
                break;
            case objType.PowerUpShield:
                prefabToGenerate = this.powerUpShieldPrefab;
                break;
            case objType.PowerUpSnail:
                prefabToGenerate = this.powerUpSnailPrefab;
                break;
            case objType.PowerUpLaser:
                prefabToGenerate = this.powerUpLaserPrefab;
                break;
            case objType.PowerUpGrenade:
                prefabToGenerate = this.powerUpGrenadePrefab;
                break;
            default:
                Debug.LogError("<color=red> Dynamic pool received an incorrect object type </color>");
                prefabToGenerate = asteroidPrefabList[0];
                break;
        }
        return prefabToGenerate;
    }


    public void addToAsteroidList(GameObject asteroidPrefabsToAdd)
    {
        asteroidPrefabList.Add(asteroidPrefabsToAdd);
        for (int i = 0; i < numToCreateAsteroids; i++)
        {
            asteroidQueue.Enqueue(CreateSingleObject(objType.Asteroid));
        }
    }

    public void StartWarmParticles()
    {
        StartCoroutine("WarmParticles");
    }

    IEnumerator WarmParticles()
    {
        foreach (GameObject bullet in bulletQueue)
        {
            bullet.transform.position = new Vector3(100, 100, 100);
            bullet.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject bullet in bulletQueue)
        {
            bullet.transform.position = Vector3.zero;
            bullet.SetActive(false);
        }
    }
}