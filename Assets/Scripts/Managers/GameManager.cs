using System.Collections;
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
    public WarpSpeed speedParticles;

    public GameObject bulletPrefab;
    public ObstaclesPrefabs obstaclesPrefabs;

    private int countSlowmo;
    private int countShield;


    public bool paused = true;
    public string menuScene = "MainMenu";
    public PlayerData playerData;

    private GameObject shield;

    public delegate void OnDifficultyChangeDelegate(Level currentLevel);
    public OnDifficultyChangeDelegate onDifficultyChange;

    void Awake()
    {
        //Loading spaceShip and bullet data
        GameObject PD = GameObject.Find("PlayerData");
        if (PD != null)
        {
            playerData = PD.GetComponent<PlayerData>();
            LoadData(playerData);
            playerData.wasInGameplay = true;

            //Sound of bullet
            soundManager.shootSoundID = playerData.weaponID;
            soundManager.mainVolume = playerData.mainVolume;
            soundManager.playerData = this.playerData;
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

        speedParticles.gameManager = this;

        PickUniverseBackground();

        GenerateDynamicPoolObjects();

        curveManager.Generate(playerShip);
        playerShip.GetComponent<PlayerCurveTraveller>().Setup(curveManager, this, curveWorld);

        skyboxCamera.transform.parent = spaceAtrezzo.transform;
        curveManager.SetupAtrezzo(spaceAtrezzo);
    }

    public void StartNewGame()
    {
        paused = false;
        speedParticles.Disengage();
        speedParticles.particleSpeed = 0.2f;
        scoreManager.StartNewGame();
        curveManager.ResetObstacles();
        SetVelocityPlayerTraveller(Level.SuperEasy);
        playerShip.GetComponent<PlayerShipHandler>().position = Vector2.zero;
        playerShip.GetComponent<PlayerShipHandler>().hasPowerUp = false;

        //Reset PowerUps
        ppManager.PPSlowMotion(false);
        playerShip.GetComponent<PlayerShipHandler>().invincibility = false;
        playerShip.GetComponent<PlayerCurveTraveller>().SetVelocity(scoreManager.currentLevel);

    }

    void LoadData(PlayerData playerData)
    {
        //-----------------------NAVE----------------------
        GameObject spaceShip = playerData.spaceShipsLP[playerData.spaceShipID]; //Low poly SpaceShips
        SceneManager.MoveGameObjectToScene(spaceShip, SceneManager.GetActiveScene());
        spaceShip.transform.parent = GameObject.Find("World").transform.Find("ContainerPlayerShip").transform;

        //Component not needed
        Destroy(spaceShip.GetComponent<RotateShip>());

        //Adjust transform
        // spaceShip.transform.localScale = Vector3.one;
        spaceShip.transform.localPosition = Vector3.zero;
        spaceShip.transform.localEulerAngles = Vector3.zero;
        spaceShip.gameObject.name = "PlayerShip";

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
        playerShip.GetComponent<PlayerCurveTraveller>().playerVelocity = 3.5f;
        ppManager.PPSlowMotion(true);

        countSlowmo++;

        yield return new WaitForSeconds(seconds);

        countSlowmo--;

        if (countSlowmo == 0)
        {
            ppManager.PPSlowMotion(false);
            playerShip.GetComponent<PlayerCurveTraveller>().SetVelocity(scoreManager.currentLevel);
        }
    }

    IEnumerator Shield(int seconds)
    {
        //Ignore Asteroids and Enemies
        playerShip.GetComponent<PlayerShipHandler>().invincibility = true;

        shield.SetActive(true);
        shield.transform.DOScale(new Vector3(2.1882f, 2.1882f, 2.1882f), 0.5f).OnComplete(() =>
            {
                shield.GetComponent<Light>().enabled = true;
            }
        );

        countShield++;

        yield return new WaitForSeconds(seconds);

        countShield--;
        
        if(countShield == 0)
        {
            shield.GetComponent<Light>().enabled = false;
            shield.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.5f).OnComplete(() =>
                {
                    shield.SetActive(false);
                    //Asteroids hit again
                    playerShip.GetComponent<PlayerShipHandler>().invincibility = false;
                }
            );
        }

    }

    public void SetVelocityPlayerTraveller(Level level)
    {
        playerShip.GetComponent<PlayerCurveTraveller>().SetVelocity(level);
    }

    void GenerateDynamicPoolObjects()
    {
        //Pools must be generated before curves because curves place obstacles that need to be in the pools
        DynamicPool.instance.Generate(DynamicPool.objType.Bullet, bulletPrefab);
        DynamicPool.instance.StartWarmParticles();

        DynamicPool.instance.Generate(DynamicPool.objType.Asteroid, obstaclesPrefabs.asteroids[0]);
        for (int i = 1; i < obstaclesPrefabs.asteroids.Count; i++)
        {
            DynamicPool.instance.addToAsteroidList(obstaclesPrefabs.asteroids[i]);
        }
        DynamicPool.instance.Generate(DynamicPool.objType.PortalBarrel, obstaclesPrefabs.portalBarrel);
        DynamicPool.instance.Generate(DynamicPool.objType.PortalPlanet, obstaclesPrefabs.portalPlanet);
        DynamicPool.instance.Generate(DynamicPool.objType.PowerUpShield, obstaclesPrefabs.powerUpPortals.shield);
        DynamicPool.instance.Generate(DynamicPool.objType.PowerUpSnail, obstaclesPrefabs.powerUpPortals.snail);
        DynamicPool.instance.Generate(DynamicPool.objType.PowerUpLaser, obstaclesPrefabs.powerUpPortals.laser);
        DynamicPool.instance.Generate(DynamicPool.objType.PowerUpGrenade, obstaclesPrefabs.powerUpPortals.grenade);
    }

    void PickUniverseBackground()
    {
        for (int i = 0; i < spaceAtrezzo.transform.childCount; i++)
        {
            spaceAtrezzo.transform.GetChild(i).gameObject.SetActive(false);
        }

        int currentUniverse = Random.Range(0, spaceAtrezzo.transform.childCount);

        spaceAtrezzo.transform.GetChild(currentUniverse).gameObject.SetActive(true);

    }

    public void ExitGame()
    {
        Application.Quit();
    }

}

[System.Serializable]
public class ObstaclesPrefabs
{
    public List<GameObject> asteroids;
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

