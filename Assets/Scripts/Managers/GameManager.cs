﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    const int Y_ROT_ALIGN_CURVE_AND_PLAYER = -90;

    public ScoreManager scoreManager;
    public SoundManager soundManager;
    public UIManager uiManager;
    public PostProcessManager ppManager;

    public GameObject playerShip;

    public CurveSystem curveManager;
    public GameObject curveWorld;

    public Camera skyboxCamera;
    public GameObject spaceAtrezzo;

    public GameObject bulletPrefab;

    public ObstaclesPrefabs obstaclesPrefabs;

    public bool paused = true;

    private GameObject shield;

    void Awake()
    {
        //Loading spaceShip and bullet data
        GameObject PD = GameObject.Find("PlayerData");
        PlayerData playerData;
        if (PD != null)
        {
            playerData = PD.GetComponent<PlayerData>();
            LoadData(playerData);

            //Sound of bullet
            soundManager.shootSoundID = playerData.weaponID;
        }

        //Initialize needed scripts
        scoreManager.gameManager = this;
        scoreManager.uiManager = this.uiManager;
        scoreManager.soundManager = this.soundManager;

        soundManager.gameManager = this;
        soundManager.scoreManager = this.scoreManager;
        soundManager.uiManager = this.uiManager;

        uiManager.gameManager = this;

        PlayerShipHandler playerShipHandler = playerShip.GetComponent<PlayerShipHandler>(); 
        playerShipHandler.gameManager = this;
        playerShipHandler.scoreManager = this.scoreManager;
        playerShipHandler.soundManager = this.soundManager;

        playerShipHandler.radius = curveManager.curvePrefab.pipeRadius - 2.0f;
        curveManager.scoreManager = this.scoreManager;

        shield = playerShip.transform.Find("Shield").gameObject;
        shield.transform.localScale = Vector3.zero;
        shield.GetComponent<Light>().enabled = false;
        shield.SetActive(false);

        //Pools must be generated before curves because curves place obstacles that need to be in the pools
        DynamicPool.instance.Generate(DynamicPool.objType.Bullet, bulletPrefab);
        DynamicPool.instance.Generate(DynamicPool.objType.Asteroid, obstaclesPrefabs.asteroid);
        DynamicPool.instance.Generate(DynamicPool.objType.PortalBarrel, obstaclesPrefabs.portalBarrel);
        DynamicPool.instance.Generate(DynamicPool.objType.PortalPlanet, obstaclesPrefabs.portalPlanet);

        //Falta acabar dynamicpool para que pueda generar los prefabs de los portales powerup
        //DynamicPool.instance.Generate(DynamicPool.objType.PowerUpGrenade, obstaclesPrefabs.powerUpPortals.grenade);
        //DynamicPool.instance.Generate(DynamicPool.objType.PowerUpLaser, obstaclesPrefabs.powerUpPortals.laser);
        //DynamicPool.instance.Generate(DynamicPool.objType.PowerUpShield, obstaclesPrefabs.powerUpPortals.shield);
        //DynamicPool.instance.Generate(DynamicPool.objType.PowerUpSnail, obstaclesPrefabs.powerUpPortals.snail);

        curveManager.Generate(playerShip);
        playerShip.GetComponent<PlayerCurveTraveller>().Setup(curveManager, this, curveWorld);    

        skyboxCamera.transform.parent = spaceAtrezzo.transform;
        curveManager.SetupAtrezzo(spaceAtrezzo);
    }

    public void StartNewGame()
    {
        paused = false;
        scoreManager.StartNewGame();
        curveManager.ResetObstacles(); //hay que poner las dos primeras curvas vacías
        SetVelocityPlayerTraveller(Level.SuperEasy);
    }

    void LoadData(PlayerData playerData)
    {
        //-----------------------NAVE----------------------
        GameObject spaceShip = GameObject.Find(playerData.spaceShipsLP[playerData.spaceShipID].name); //Low poly SpaceShips
        SceneManager.MoveGameObjectToScene(spaceShip, SceneManager.GetActiveScene());
        spaceShip.transform.parent = GameObject.Find("World").transform.Find("ContainerPlayerShip").transform;

        //Component not needed
        Destroy(spaceShip.GetComponent<RotateShip>());

        //Adjust transform
        spaceShip.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        spaceShip.transform.localPosition = Vector3.zero;
        spaceShip.transform.localEulerAngles = Vector3.zero;

        //-----------------------ARMAS----------------------
        bulletPrefab = playerData.bullets[playerData.weaponID];

    }

    public void PowerUp(int powerUpID)
    {
        int powerUpDuration = 5;
        switch (powerUpID)
        {
            case 0: //Slow Motion
                StartCoroutine(SlowMo(powerUpDuration));
                break;

            case 1: //Shield
                StartCoroutine(Shield(powerUpDuration));
                break;

        }
    }

    public void PauseGame()
    {
        paused = true;
    }

    public void ResumeGame()
    {
        paused = false;
    }

    IEnumerator SlowMo(int seconds)
    {
        float velocity = playerShip.GetComponent<PlayerCurveTraveller>().playerVelocity;
        playerShip.GetComponent<PlayerCurveTraveller>().playerVelocity = 1;
        ppManager.PPSlowMotion(true);
        yield return new WaitForSeconds(seconds);
        ppManager.PPSlowMotion(false);
        playerShip.GetComponent<PlayerCurveTraveller>().playerVelocity = velocity;
    }

    IEnumerator Shield(int seconds)
    {
        shield.SetActive(true);
        shield.transform.DOScale(new Vector3(2.1882f, 2.1882f, 2.1882f), 0.5f).OnComplete(() =>
            {
                shield.GetComponent<Light>().enabled = true;
            }
        );
        //Ignore Asteroids and Enemies
        playerShip.GetComponent<PlayerShipHandler>().invincibility = true;

        yield return new WaitForSeconds(seconds);
        shield.GetComponent<Light>().enabled = false;
        shield.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.5f);
        shield.SetActive(false);

        //Asteroids hits again
        playerShip.GetComponent<PlayerShipHandler>().invincibility = false;

        
    }

    public void SetVelocityPlayerTraveller(Level level)
    {
        playerShip.GetComponent<PlayerCurveTraveller>().SetVelocity(level);
    }

}

[System.Serializable]
public class ObstaclesPrefabs
{
    public GameObject asteroid;
    public GameObject portalBarrel;
    public GameObject portalPlanet;
    public GameObject darkHole;
    public PowerUpPortals powerUpPortals;

    [System.Serializable]
    public class PowerUpPortals
    {
        public GameObject shield;
        public GameObject snail;
        public GameObject laser;
        public GameObject grenade;
    }
    
}

