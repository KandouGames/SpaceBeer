using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const int maxBarrels = 4;
    private const int iniBarrels = 2;

    private int beerCoins;
    private ulong distance;
    private int barrels;
    public UIManager uiManager;

    void Start()
    {
        StartNewGame();
    }

    void Update()
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

    public void StartNewGame()
    {
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
}
