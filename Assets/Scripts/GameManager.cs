using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerShip;
    public CurveSystem curveManager;

    void Start()
    {

#if UNITY_ANDROID
        playerShip.AddComponent<PhoneController>();
#else
        playerShip.AddComponent<StandaloneController>();
#endif
        curveManager.Generate(playerShip);

    }

}
