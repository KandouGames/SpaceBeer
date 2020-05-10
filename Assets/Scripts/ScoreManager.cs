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
        startNewGame();
    }

    void Update()
    {
        distance += 1;
        uiManager.setDistance(distance);

        // Para probar
        if (Input.GetKeyDown(KeyCode.Z))
            earnBarrel();

        if (Input.GetKeyDown(KeyCode.X))
            looseBarrel();

        if (Input.GetKeyDown(KeyCode.C))
            distributeBarrel();
    }

    public void startNewGame()
    {
        beerCoins = 0;
        distance = 0;
        barrels = iniBarrels;

        uiManager.setBeerCoins(beerCoins);
        uiManager.setDistance(distance);
        uiManager.setBarrels(barrels);
    }

    public void earnBarrel()
    {
        
        if (barrels < maxBarrels)
        {
            ++barrels;
            uiManager.setBarrels(barrels);
        }


    }

    public void distributeBarrel()
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

        
        uiManager.setBarrels(barrels);
        uiManager.setBeerCoins(beerCoins);

    }

    public void looseBarrel()
    {
        if (barrels == 0)
        {
            uiManager.showGameOver(beerCoins, distance);
            print("game over");
        } else
        {
            --barrels;
            uiManager.setBarrels(barrels);
        }
            
            
    }
}
