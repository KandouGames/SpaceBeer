using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int barrels;

    [HideInInspector]
    public UIManager uiManager;
    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public Level currentLevel;  //Esto es la maquina de estados

    struct BeerCoinLevels
    {
        public static int SuperEasy = 0;
        public static int Easy = 1500;
        public static int Medium = 4000;
        public static int Hard = 7500;
        public static int God = 10000;
    }

    void Start()
    {
        StartNewGame();
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
        if (barrels == 0)
        {
            uiManager.ShowGameOver(beerCoins, distance);
            print("game over");
        } else
        {
            --barrels;
            uiManager.SetBarrels(barrels);
        }
    }

    public void SetLevel()
    {
        switch(currentLevel)
        {
            case Level.SuperEasy:
                if (beerCoins > BeerCoinLevels.Easy)
                    currentLevel = Level.Easy;
                break;
            case Level.Easy:
                if (beerCoins > BeerCoinLevels.Medium)
                    currentLevel = Level.Medium;
                break;
            case Level.Medium:
                if (beerCoins > BeerCoinLevels.Hard)
                    currentLevel = Level.Hard;
                break;
            case Level.Hard:
                if (beerCoins > BeerCoinLevels.God)
                    currentLevel = Level.God;
                break;
            case Level.God:

                break;
        }
    }
    
    public ulong getDistance()
    {
        return distance;
    }
}

