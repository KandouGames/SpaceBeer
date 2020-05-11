using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerShip;
    public CurveSystem curveManager;
    public GameObject world;
    public Camera skyboxCamera;

    private float worldAccumulatedRotation = 0f;


    void Start()
    {
        curveManager.Generate(playerShip);

        playerShip.GetComponent<PlayerCurveTraveller>().Setup(curveManager, this, world);
    }

}
