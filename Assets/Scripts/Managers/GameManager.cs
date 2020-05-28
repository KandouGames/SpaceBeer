using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const int Y_ROT_ALIGN_CURVE_AND_PLAYER = -90;

    public GameObject playerShip;
    public CurveSystem curveManager;
    public GameObject curveWorld;

    public Camera skyboxCamera;
    public GameObject spaceAtrezzo;

    public GameObject bulletPrefab;
    public GameObject asteroidPrefab;


    void Awake()
    {
        //Pools must be generated before curves because curves place obstacles that need to be in the pools
        DynamicPool.instance.Generate(DynamicPool.objType.Bullet, bulletPrefab);
        DynamicPool.instance.Generate(DynamicPool.objType.Asteroid, asteroidPrefab);

        curveManager.Generate(playerShip);

        playerShip.GetComponent<PlayerCurveTraveller>().Setup(curveManager, this, curveWorld);

        skyboxCamera.transform.parent = curveManager.getCurves()[0].transform;
        spaceAtrezzo.transform.parent = curveManager.getCurves()[0].transform;


    }

}
