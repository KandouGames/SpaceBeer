using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text beerCoinsText;
    public Text distanceText;
    public List<GameObject> arrayBarrels;
    public GameObject gameOverUI;
    public Text gameOverRecordText;


    void Start()
    {

    }

    void Update()
    {
        
    }

    public void setBeerCoins(int beerCoins)
    {
        beerCoinsText.text = beerCoins.ToString();
    }

    public void setDistance(ulong distance)
    {
        distanceText.text = distance.ToString();
    }

    public void setBarrels(int barrels)
    {
        for(int i = 0; i < arrayBarrels.Count; ++i)
        {
            if (i < barrels)
                arrayBarrels[i].SetActive(true);
            else
                arrayBarrels[i].SetActive(false);
        }

    }

    public void showGameOver(int beerCoins, ulong distance)
    {
        gameOverUI.SetActive(true);
        gameOverRecordText.text = "You won " + beerCoins.ToString() + " beercoins \n and have traveled " + distance.ToString() + " km";
    }
}
