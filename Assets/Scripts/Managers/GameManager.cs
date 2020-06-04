using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const int Y_ROT_ALIGN_CURVE_AND_PLAYER = -90;

    public ScoreManager scoreManager;

    public GameObject playerShip;

    public CurveSystem curveManager;
    public GameObject curveWorld;

    public Camera skyboxCamera;
    public GameObject spaceAtrezzo;

    public GameObject bulletPrefab;

    public ObstaclesPrefabs obstaclesPrefabs;


    void Awake()
    {
        //Cargamos datos de la nave y balas
        GameObject PD = GameObject.Find("PlayerData");
        PlayerData playerData;
        if (PD != null)
        {
            playerData = PD.GetComponent<PlayerData>();
            LoadData(playerData);
        }

        //Pools must be generated before curves because curves place obstacles that need to be in the pools
        DynamicPool.instance.Generate(DynamicPool.objType.Bullet, bulletPrefab);
        DynamicPool.instance.Generate(DynamicPool.objType.Asteroid, obstaclesPrefabs.asteroid);
        DynamicPool.instance.Generate(DynamicPool.objType.PortalBarrel, obstaclesPrefabs.portalBarrel);
        DynamicPool.instance.Generate(DynamicPool.objType.PortalPlanet, obstaclesPrefabs.portalPlanet);

        curveManager.Generate(playerShip);
        curveManager.SetScoreManager(scoreManager);

        playerShip.GetComponent<PlayerCurveTraveller>().Setup(curveManager, this, curveWorld);
        playerShip.GetComponent<PlayerShipHandler>().scoreManager = this.scoreManager;

        skyboxCamera.transform.parent = spaceAtrezzo.transform;

        curveManager.SetupAtrezzo(spaceAtrezzo);
    }

    public void StartNewGame()
    {
        scoreManager.StartNewGame();
        curveManager.ResetObstacles();
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
        GameObject bullet = GameObject.Find(playerData.bullets[playerData.weaponID].name);
        SceneManager.MoveGameObjectToScene(bullet, SceneManager.GetActiveScene());


        bulletPrefab = bullet;

    }

}

[System.Serializable]
public class ObstaclesPrefabs
{
    public GameObject asteroid;
    public GameObject portalBarrel;
    public GameObject portalPlanet;
    public GameObject darkHole;
}
