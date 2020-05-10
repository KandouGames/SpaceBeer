using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurveTraveller : MonoBehaviour
{
    private CurveSystem curveManager;
    private GameObject world;
    private GameManager gameManager;

    private Curve currentCurve;

    public float playerVelocity = 1.5f;
    private float curveSystemAccumulatedRotation = 0f;
    private float worldRotation = 0f;

    private float deltaTraveledToRotation = 0f;
    private float deltaTraveled = 0f;


    public void Setup(CurveSystem curveManager, GameManager gameManager, GameObject world)
    {
        this.curveManager = curveManager;
        this.gameManager = gameManager;
        this.world = world;

        currentCurve = curveManager.getCurves()[0];

        deltaTraveledToRotation = 360f / (2f * Mathf.PI * currentCurve.GetTorusRadius());

        curveBasePoints = currentCurve.getCurveBasePoints();

    }

    private void Update()
    {
        AdvanceCurve();
    }

    float distanceFromPointToPoint = 0;
    Vector3 toTranslate = Vector3.zero;
    float pointDistDelta = 0;

    int currentPointInCurve = 0;
    int currentCurveID = 0;
    Transform[] curveBasePoints;

    private void AdvanceCurve()
    {


        RotationAdvance();

        /*
        if (currentPointInCurve < curveBasePoints.Length - 1)
        {
            world.transform.position += curveBasePoints[currentPointInCurve].transform.position - curveBasePoints[currentPointInCurve + 1].transform.position;
            currentPointInCurve++;
        }
        else
        {
            if (currentCurveID < curveManager.getCurves().Length - 1)
                currentCurveID++;
            else
            {
                currentCurveID = 0;
                //curveManager.transform.position = Vector3.zero;

            }

            currentCurve = curveManager.getCurves()[currentCurveID];
            curveBasePoints = currentCurve.getCurveBasePoints();
            currentCurve = curveManager.getCurves()[curveManager.getCurrentPlayerCurve()];


            currentPointInCurve = 0;
           
        }
         */

    }


    private void RotationAdvance()
    {
        float delta = playerVelocity * Time.deltaTime;
        deltaTraveled += delta;
        // world.transform.position += -(Vector3.forward * deltaTraveled);

        Curve currentCurve = curveManager.getCurves()[curveManager.getCurrentPlayerCurve()];

        curveSystemAccumulatedRotation += deltaTraveled * deltaTraveledToRotation;

        if (curveSystemAccumulatedRotation >= currentCurve.getCurveAngle())
        {
            //Finished a pipe! Converting the travelled angle to distance
            deltaTraveled = (curveSystemAccumulatedRotation - currentCurve.getCurveAngle()) / deltaTraveledToRotation;
            currentCurve = curveManager.PrepareNextCurve();
            //SetupCurrentCurve();
            curveSystemAccumulatedRotation = deltaTraveled * deltaTraveledToRotation;
        }

        curveManager.transform.localRotation = Quaternion.Euler(0, -90, curveSystemAccumulatedRotation);
    }


    private void SetupCurrentCurve()
    {
        deltaTraveledToRotation = 180 / (Mathf.PI * currentCurve.GetTorusRadius());
        worldRotation += currentCurve.GetRelativeRotation();
        if (worldRotation < 0f)
        {
            worldRotation += 360f;
        }
        else if (worldRotation >= 360f)
        {
            worldRotation -= 360f;
        }
        world.transform.localRotation = Quaternion.Euler(worldRotation, 0f, 0f);
    }
}
