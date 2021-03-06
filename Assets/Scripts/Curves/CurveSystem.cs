﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CurveSystem : MonoBehaviour
{
    const int THIRD_CURVE = 2;
    const int SECOND_CURVE = 1;
    const int FIRST_CURVE = 0;

    public Curve curvePrefab;
    private GameObject spaceAtrezzo;

    [HideInInspector]
    public Transform playerShip;

    public int curveCount;
    public bool generatePipes = false;
    public float dissapearObstaclesZ = -1f;

    private Curve[] curves;

    private uint currentPlayerCurve = 0;
    bool finishedGenerating = false;
    bool firstCurveWithoutObstacles = true;
    bool secondCurveWithoutObstacles = true;

    [HideInInspector]
    public ScoreManager scoreManager;


    public void Generate(GameObject playerShip)
    {
        if (playerShip == null)
        {
            Debug.LogError("<color=red> WARNING</color> PlayerShip never found");
        }
        else
        {
            this.playerShip = playerShip.transform;
            Generate();
        }
    }

    private void Generate()
    {
        curves = new Curve[curveCount];
        for (int i = 0; i < curves.Length; i++)
        {
            Curve curve = curves[i] = Instantiate<Curve>(curvePrefab);
            curve.Create(generatePipes);
            curve.transform.SetParent(transform);
            if (i > 0)
            {
                curve.AlignWith(curves[i - 1]);
            }
            if (i != FIRST_CURVE && i != SECOND_CURVE)
            {
                curve.GenerateObstacles();
            }

        }
        finishedGenerating = true;

        //Position the curves at 0,0
        //The setup is done with the second curve to avoid seeing the pipes disappear
        transform.localPosition = new Vector3(0, -curves[SECOND_CURVE].GetTorusRadius(), 0);

        //Align with controller axis
        transform.rotation = Quaternion.Euler(0, -90, 0);

        AlignCurveWithOrigin();
    }


    void Update()
    {
        if (finishedGenerating)
        {
            //If we checked all curves generated obstacles behind the player from other curves would fade
            CheckForFadingObstacles(curves[FIRST_CURVE]);
            CheckForFadingObstacles(curves[SECOND_CURVE]);
        }
    }

    private void CheckForFadingObstacles(Curve curve)
    {
        for (int i = 0; i < curve.obstacleList.Count; i++)
            if (curve.obstacleList[i].obstacleObj.transform.position.z <= dissapearObstaclesZ
                && !curve.obstacleList[i].hasFaded)
                curve.obstacleList[i].FadeOutObstacle();
    }

    public void SetupAtrezzo(GameObject spaceAtrezzo)
    {
        spaceAtrezzo.transform.parent = curves[curves.Length - 1].transform;
        this.spaceAtrezzo = spaceAtrezzo;
    }

    public Curve PrepareNextCurve()
    {
        AddObstaclesToFirstCurves(false);

        curves[FIRST_CURVE].SetObstacles(scoreManager.currentLevel);

        MoveCurveOrder();
        AlignCurveWithOrigin();

        //Move curveSystem to make the curve appear at origin
        curves[curves.Length - 1].AlignWith(curves[curves.Length - 2]);
        transform.localPosition = new Vector3(0f, -curves[SECOND_CURVE].GetTorusRadius());

        MoveSpaceAtrezzo();
        FadeInLastCurve();

        return curves[SECOND_CURVE];
    }


    /// <summary>
    /// Places the just finished curve at the end and shifts all the curves forward in the order
    /// </summary>
    private void MoveCurveOrder()
    {
        Curve finishedCurve = curves[FIRST_CURVE];
        for (int i = 1; i < curves.Length; i++)
        {
            curves[i - 1] = curves[i];
        }
        curves[curves.Length - 1] = finishedCurve;
    }

    private void AlignCurveWithOrigin()
    {
        Transform currentCurveInOrigin = curves[SECOND_CURVE].transform;

        //If we set the curves as childs of this curve everything will rotate and move with it
        //Same strategy used with the curvePoints
        for (int i = 0; i < curves.Length; i++)
        {
            if (i != SECOND_CURVE)
                curves[i].transform.SetParent(currentCurveInOrigin);
        }

        currentCurveInOrigin.localPosition = Vector3.zero;
        currentCurveInOrigin.localRotation = Quaternion.identity;

        for (int i = 0; i < curves.Length; i++)
        {
            if (i != SECOND_CURVE)
                curves[i].transform.SetParent(transform);
        }
    }

    private void AddObstaclesToFirstCurves(bool retryGame)
    {
        if (firstCurveWithoutObstacles)
        {
            FillFirstCurvesAndMakeInvisible(retryGame);
            firstCurveWithoutObstacles = false;

        }
        else if (secondCurveWithoutObstacles)
        {
            FillFirstCurvesAndMakeInvisible(retryGame);
            secondCurveWithoutObstacles = false;
        }
    }

    void FillFirstCurvesAndMakeInvisible(bool retryGame)
    {
        if (!retryGame)
            curves[FIRST_CURVE].GenerateObstacles();

        foreach (Obstacle obst in curves[FIRST_CURVE].obstacleList)
        {
            if (obst.type == Obstacle.obstType.Mesh)
            {
                Material matColor = obst.obstacleObj.GetComponentInChildren<MeshRenderer>().material;
                matColor.color = new Color(matColor.color.r, matColor.color.g, matColor.color.b, 0);
            }
            else
            {
                Material matColor = obst.obstacleObj.GetComponentInChildren<SpriteRenderer>().material;
                matColor.color = new Color(matColor.color.r, matColor.color.g, matColor.color.b, 0);
            }
        }
    }

    private void FadeInLastCurve()
    {
        List<Obstacle> obstacles = curves[curves.Length - 1].obstacleList;
        for (int i = 0; i < obstacles.Count; i++)
        {
            obstacles[i].FadeInObstacle();
        }
    }

    private void MoveSpaceAtrezzo()
    {
        spaceAtrezzo.transform.SetParent(curves[SECOND_CURVE].transform, true);
    }

    public void ResetObstacles()
    {
        for (int i = 0; i < curves.Length; i++)
        {
            curves[i].SetObstacles(Level.SuperEasy);
            if (i == FIRST_CURVE || i == SECOND_CURVE || i == THIRD_CURVE)
            {
                List<Obstacle> obstacles = curves[i].obstacleList;
                for (int j = 0; j < obstacles.Count; j++)
                {
                    obstacles[j].obstacleObj.SetActive(false);
                }
            }
        }
        firstCurveWithoutObstacles = true;
        secondCurveWithoutObstacles = true;
        AddObstaclesToFirstCurves(true);
    }

    public Curve[] GetCurves()
    {
        return curves;
    }

    public uint getCurrentPlayerCurve()
    {
        return currentPlayerCurve;
    }

}
