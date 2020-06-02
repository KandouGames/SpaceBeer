using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPool : MonoBehaviour
{
    //Not happy about this
    public GameObject bulletPrefab;
    public GameObject asteroidPrefab;
    public GameObject portalBarrelPrefab;
    public GameObject portalPlanetPrefab;
    public GameObject darkHolePrefab;

    public int numToCreateBullets = 10;
    public int numToCreateAsteroids = 10;
    public int numToCreatePortalBarrels = 10;
    public int numToCreatePortalPlanets = 10;
    public int numToCreateDarkHoles = 10;

    private Queue<GameObject> bulletQueue;
    private Queue<GameObject> asteroidQueue;
    private Queue<GameObject> portalBarrelQueue;
    private Queue<GameObject> portalPlanetQueue;
    private Queue<GameObject> darkHoleQueue;

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
        DarkHole
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
                    this.asteroidPrefab = prefab;
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

        obj.SetActive(false);
        currentQueue.Enqueue(obj);
    }

    private GameObject CreateSingleObject(objType objToCreate)
    {

        GameObject currentObj = Instantiate(GetPrefabType(objToCreate));
        //Only bullets are curve independent
        if (objType.Bullet == objToCreate)
            currentObj.transform.parent = bulletElements.transform;
        else
            currentObj.transform.parent = curveElements.transform;


        currentObj.name = Enum.GetName(typeof(objType), objToCreate);
        currentObj.SetActive(false);
        return currentObj;
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
                prefabToGenerate = this.asteroidPrefab;
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
            default:
                Debug.LogError("<color=red> Dynamic pool received an incorrect object type </color>");
                prefabToGenerate = this.asteroidPrefab;
                break;
        }
        return prefabToGenerate;
    }

}