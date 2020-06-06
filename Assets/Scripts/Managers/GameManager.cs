using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const int Y_ROT_ALIGN_CURVE_AND_PLAYER = -90;

    public ScoreManager scoreManager;
    public SoundManager soundManager;
    public UIManager uiManager;

    public GameObject playerShip;

    public CurveSystem curveManager;
    public GameObject curveWorld;

    public Camera skyboxCamera;
    public GameObject spaceAtrezzo;

    public GameObject bulletPrefab;

    public ObstaclesPrefabs obstaclesPrefabs;

    public bool paused = true;


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



        //Pools must be generated before curves because curves place obstacles that need to be in the pools
        DynamicPool.instance.Generate(DynamicPool.objType.Bullet, bulletPrefab);
        DynamicPool.instance.Generate(DynamicPool.objType.Asteroid, obstaclesPrefabs.asteroid);
        DynamicPool.instance.Generate(DynamicPool.objType.PortalBarrel, obstaclesPrefabs.portalBarrel);
        DynamicPool.instance.Generate(DynamicPool.objType.PortalPlanet, obstaclesPrefabs.portalPlanet);

        curveManager.Generate(playerShip);
        playerShip.GetComponent<PlayerCurveTraveller>().Setup(curveManager, this, curveWorld);    
        

        skyboxCamera.transform.parent = spaceAtrezzo.transform;
        curveManager.SetupAtrezzo(spaceAtrezzo);
    }

    public void StartNewGame()
    {
        paused = false;
        scoreManager.StartNewGame();
        curveManager.ResetObstacles();
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

    public void PauseGame()
    {
        paused = true;
    }

    public void ResumeGame()
    {
        paused = false;
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
