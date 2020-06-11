using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Level
{
    SuperEasy,
    Easy,
    Medium,
    Hard,
    God,
}

public class ScoreManager : MonoBehaviour
{
    private const int maxBarrels = 4;
    private const int iniBarrels = 2;

    private int beerCoins;
    private ulong distance;
    [HideInInspector]
    public int barrels;

    [HideInInspector]
    public UIManager uiManager;
    [HideInInspector]
    public GameManager gameManager;
    [HideInInspector]
    public SoundManager soundManager;

    [HideInInspector]
    public Level currentLevel;  //Esto es la maquina de estados

    struct BeerCoinLevels
    {
        public static int SuperEasy = 0;
        public static int Easy = 500; //500
        public static int Medium = 1250; //1250
        public static int Hard = 3000; //3000
        public static int God = 5500; //5500
    }

    void Start()
    {
        StartNewGame();
        gameManager.onDifficultyChange += ShowDifficulty;
    }

    void Update()
    {
        if (!gameManager.paused)
        {
            distance += 1;
            uiManager.SetDistance(distance);

            // Para probar
            if (Input.GetKeyDown(KeyCode.Z))
                EarnBarrel();

            if (Input.GetKeyDown(KeyCode.X))
                LooseBarrel();

            if (Input.GetKeyDown(KeyCode.C))
                DistributeBarrel();
        }

    }

    public void StartNewGame()
    {
        currentLevel = Level.SuperEasy;

        beerCoins = 0;
        distance = 0;
        barrels = iniBarrels;

        uiManager.SetBeerCoins(beerCoins);
        uiManager.SetDistance(distance);
        uiManager.SetBarrels(barrels);

        PlayerShipHandler playerShipHandler = gameManager.playerShip.GetComponent<PlayerShipHandler>();
        playerShipHandler.powerUpsInterface[playerShipHandler.powerUpID].SetActive(false);
    }

    public void EarnBarrel()
    {
        if (barrels < maxBarrels)
        {
            ++barrels;
            uiManager.SetBarrels(barrels);
        }
    }

    public void DistributeBarrel()
    {
        switch (barrels)
        {
            case 0:
                break;

            case 1:
                beerCoins += 225;
                --barrels;
                break;

            case 2:
                beerCoins += 200;
                --barrels;
                break;

            case 3:
                beerCoins += 125;
                --barrels;
                break;

            case 4:
                beerCoins += 100;
                --barrels;
                break;
        }


        uiManager.SetBarrels(barrels);
        uiManager.SetBeerCoins(beerCoins);

        SetLevel();
    }

    public void LooseBarrel()
    {
        --barrels;
        uiManager.SetBarrels(barrels);

        if (barrels == -1)
        {
            SetGameOver();
        }
    }

    public void SetLevel()
    {
        switch (currentLevel)
        {
            case Level.SuperEasy:
                if (beerCoins > BeerCoinLevels.Easy)
                {
                    currentLevel = Level.Easy;
                    LevelChanged();
                }
                break;
            case Level.Easy:
                if (beerCoins > BeerCoinLevels.Medium)
                {
                    currentLevel = Level.Medium;
                    LevelChanged();
                }
                break;
            case Level.Medium:
                if (beerCoins > BeerCoinLevels.Hard)
                {
                    currentLevel = Level.Hard;
                    LevelChanged();
                }
                break;
            case Level.Hard:
                if (beerCoins > BeerCoinLevels.God)
                {
                    currentLevel = Level.God;
                    LevelChanged();
                }
                break;
            case Level.God:

                break;
        }
        if (gameManager.countSlowmo == 0)
            gameManager.SetVelocityPlayerTraveller(currentLevel);
    }

    private void LevelChanged()
    {
        if (!(gameManager.onDifficultyChange is null))
            gameManager.onDifficultyChange.Invoke(currentLevel);
    }

    public ulong getDistance()
    {
        return distance;
    }

    public void SetGameOver()
    {
        gameManager.PauseGame();
        Camera.main.transform?.DOShakeRotation(1.0f, new Vector3(0.0f, 0.0f, 50.0f), 20, 10.0f, false).OnComplete(() =>
        {
            uiManager.ShowGameOver(beerCoins, distance);
            gameManager.playerData.beerCoins += beerCoins;
            soundManager.PlayGameOver();
        }
        );
    }


    private void ShowDifficulty(Level currentLevel)
    {

        string currentLevelStr = "Level: Easy";
        string quoteString = "You got this. Keep it up!";

        switch (currentLevel)
        {
            case Level.SuperEasy:
                currentLevelStr = "Level: Calm";
                quoteString = "Como ronea como ronea, la virgen cervecezera.";
                break;
            case Level.Easy:
                currentLevelStr = "Level: Cool";
                quoteString = "You got this. Keep it up!";
                break;
            case Level.Medium:
                currentLevelStr = "Level: Keep it rollin'";
                quoteString = "Put the pedal to the metal, \nMcFly!";
                break;
            case Level.Hard:
                currentLevelStr = "Level: Despair";
                quoteString = "They're everywhere. Help!";
                break;
            case Level.God:
                currentLevelStr = "Level: God";
                quoteString = "You are the goddam best, McFly";
                break;
        }
        uiManager.ShowCurrentDifficulty(currentLevelStr, quoteString);
    }
}

