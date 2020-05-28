using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPool : MonoBehaviour
{
    //Not happy about this
    private GameObject bulletPrefab;
    private GameObject asteroidPrefab;
    private GameObject coinsPrefab;
    private GameObject barrelPrefab;
    private GameObject portalPrefab;

    public int numToCreateBullets = 10;
    public int numToCreateAsteroids = 10;
    public int numToCreateCoins = 10;
    public int numToCreateBarrels = 10;
    public int numToCreatePortals = 10;

    private Queue<GameObject> bulletQueue;
    private Queue<GameObject> asteroidQueue;
    private Queue<GameObject> coinQueue;
    private Queue<GameObject> barrelQueue;
    private Queue<GameObject> portalQueue;

    private GameObject curveElements;
    private GameObject bulletElements;

    private static DynamicPool _instance = null;
    private bool shouldExpand = true;
    public enum objType
    {
        Bullet,
        Asteroid,
        Coin,
        Barrel,
        Portal
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
                case objType.Asteroid:
                    numToCreate = numToCreateAsteroids;
                    this.asteroidPrefab = prefab;
                    break;
                case objType.Bullet:
                    numToCreate = numToCreateBullets;
                    this.bulletPrefab = prefab;
                    break;
                case objType.Coin:
                    numToCreate = numToCreateCoins;
                    this.coinsPrefab = prefab;
                    break;
                case objType.Portal:
                    numToCreate = numToCreatePortals;
                    this.portalPrefab = prefab;
                    break;
                case objType.Barrel:
                    numToCreate = numToCreateBarrels;
                    this.barrelPrefab = prefab;
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

    public void ResetBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletQueue.Enqueue(bullet);
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
            case objType.Coin:
                if (coinQueue is null)
                {
                    queueWasNull = true;
                    coinQueue = new Queue<GameObject>();
                }
                currentQueue = coinQueue;
                break;
            case objType.Portal:
                if (portalQueue is null)
                {
                    queueWasNull = true;
                    portalQueue = new Queue<GameObject>();
                }
                currentQueue = portalQueue;
                break;
            case objType.Barrel:
                if (barrelQueue is null)
                {
                    queueWasNull = true;
                    barrelQueue = new Queue<GameObject>();
                }
                currentQueue = barrelQueue;
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
            case objType.Coin:
                prefabToGenerate = this.coinsPrefab;
                break;
            case objType.Portal:
                prefabToGenerate = this.portalPrefab;
                break;
            case objType.Barrel:
                prefabToGenerate = this.barrelPrefab;
                break;
            default:
                Debug.LogError("<color=red> Dynamic pool received an incorrect object type </color>");
                prefabToGenerate = this.asteroidPrefab;
                break;
        }
        return prefabToGenerate;
    }

}