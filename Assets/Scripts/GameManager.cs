using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const int Y_ROT_ALIGN_CURVE_AND_PLAYER = -90;

    public GameObject playerShip;
    public CurveSystem curveManager;
    public GameObject world;
    public Camera skyboxCamera;


    private float worldAccumulatedRotation = 0f;


    void Awake()
    {
        curveManager.Generate(playerShip);

        playerShip.GetComponent<PlayerCurveTraveller>().Setup(curveManager, this, world);

        skyboxCamera.transform.parent = curveManager.getCurves()[0].transform;
    }

}
