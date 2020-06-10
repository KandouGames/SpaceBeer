using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerCurveTraveller : MonoBehaviour
{
    private CurveSystem curveManager;
    private GameObject world;
    private GameManager gameManager;

    private Curve currentCurve;

    public float playerVelocity = 7.0f;

    struct LevelVelocities
    {
        public static float SuperEasy = 7.0f;
        public static float Easy = 10.0f;
        public static float Medium = 12.0f;
        public static float Hard = 13.0f;
        public static float God = 14.0f;
    };

    private float curveSystemAccumulatedRotation = 0f;
    private float worldRotation = 0f;

    private float deltaToRotation = 0f;
    private float totalDeltaTraveled = 0f;

    Transform[] curveBasePoints;

    public void Setup(CurveSystem curveManager, GameManager gameManager, GameObject world)
    {
        this.curveManager = curveManager;
        this.gameManager = gameManager;
        this.world = world;

        //The setup is done with the second curve to avoid seeing the pipes disappear
        currentCurve = curveManager.GetCurves()[1];

        curveSystemAccumulatedRotation = 0f;
        worldRotation = 0f;
        deltaToRotation = 0f;
        totalDeltaTraveled = 0f;

        SetupCurrentCurve();

        SetVelocity(Level.SuperEasy);

        //Rotate the curve to see it during the tutorial
        curveManager.transform.localRotation = Quaternion.Euler(0, 0, curveSystemAccumulatedRotation);

    }

    private void Update()
    {
        if (!gameManager.paused)
            RotationAdvance();
    }

    private void RotationAdvance()
    {
        float delta = playerVelocity * Time.deltaTime;
        totalDeltaTraveled += delta;
        curveSystemAccumulatedRotation += delta * deltaToRotation;

        if (curveSystemAccumulatedRotation >= currentCurve.GetCurveAngle())
        {
            //Finished a pipe! Converting the travelled angle to distance
            delta = (curveSystemAccumulatedRotation - currentCurve.GetCurveAngle()) / deltaToRotation;
            currentCurve = curveManager.PrepareNextCurve();
            SetupCurrentCurve();
            curveSystemAccumulatedRotation = delta * deltaToRotation;
        }

        curveManager.transform.localRotation = Quaternion.Euler(0, 0, curveSystemAccumulatedRotation);
    }


    private void SetupCurrentCurve()
    {
        deltaToRotation = 360f / (2f * Mathf.PI * currentCurve.GetTorusRadius());
        worldRotation += currentCurve.GetRelativeRotation();
        if (worldRotation < 0f)
        {
            worldRotation += 360f;
        }
        else if (worldRotation >= 360f)
        {
            worldRotation -= 360f;
        }
        world.transform.localRotation = Quaternion.Euler(worldRotation, -90f, 0f);

    }

    public void SetVelocity(Level level)
    {
        switch (level)
        {
            case Level.SuperEasy:
                playerVelocity = LevelVelocities.SuperEasy;
                break;
            case Level.Easy:
                playerVelocity = LevelVelocities.Easy;
                break;
            case Level.Medium:
                playerVelocity = LevelVelocities.Medium;
                break;
            case Level.Hard:
                playerVelocity = LevelVelocities.Hard;
                break;
            case Level.God:
                playerVelocity = LevelVelocities.God;
                break;
        }
    }


}


