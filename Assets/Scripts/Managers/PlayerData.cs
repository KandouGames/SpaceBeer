using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public List<GameObject> spaceShips;
    public List<GameObject> weapons;
    public int spaceShipID;
    public int weaponID;

    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
