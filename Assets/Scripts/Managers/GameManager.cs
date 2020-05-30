﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //Cargamos datos de la nave y balas
        PlayerData playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        LoadData(playerData);

        //Pools must be generated before curves because curves place obstacles that need to be in the pools
        DynamicPool.instance.Generate(DynamicPool.objType.Bullet, bulletPrefab);
        DynamicPool.instance.Generate(DynamicPool.objType.Asteroid, asteroidPrefab);

        curveManager.Generate(playerShip);

        playerShip.GetComponent<PlayerCurveTraveller>().Setup(curveManager, this, curveWorld);

        skyboxCamera.transform.parent = curveManager.getCurves()[0].transform;
        spaceAtrezzo.transform.parent = curveManager.getCurves()[0].transform;


        

        
        
    }

    void LoadData(PlayerData playerData)
    {
        //-----------------------NAVE----------------------
        GameObject spaceShip = GameObject.Find(playerData.spaceShips[playerData.spaceShipID].name);
        SceneManager.MoveGameObjectToScene(spaceShip, SceneManager.GetActiveScene());
        spaceShip.transform.parent = GameObject.Find("World").transform.Find("ContainerPlayerShip").transform;

        //Nos deshacemos de los objetos no necesarios
        Destroy(spaceShip.GetComponent<RotateShip>());

        //Ajustamos transform
        spaceShip.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        spaceShip.transform.localPosition = Vector3.zero;
        spaceShip.transform.localEulerAngles = Vector3.zero;

        //-----------------------ARMAS----------------------


    }

}
