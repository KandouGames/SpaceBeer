using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData : MonoBehaviour
{
    public List<GameObject> spaceShips;
    public List<GameObject> spaceShipsLP;
    public List<GameObject> weapons;
    public List<GameObject> bullets;
    public int spaceShipID;
    public int weaponID;
    public int beerCoins = 0;

    [HideInInspector]
    public bool wasInGameplay;

    public SoundManager soundManager;
    public float mainVolume;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        wasInGameplay = false;

        mainVolume = soundManager.mainVolume;

        PlayerData[] playersData = FindObjectsOfType<PlayerData>();
        foreach (PlayerData playerData in playersData)
        {
            if (playerData.wasInGameplay)
            {
                beerCoins = playerData.beerCoins;
                mainVolume = playerData.mainVolume;
                spaceShipID = playerData.spaceShipID;
                Destroy(playerData.gameObject);
            }
        }

        soundManager.mainVolume = mainVolume;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
