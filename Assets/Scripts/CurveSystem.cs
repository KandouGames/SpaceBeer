using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveSystem : MonoBehaviour
{
    public Curve curvePrefab;

    [HideInInspector]
    public Transform playerShip;

    public int curveCount;
    public bool generatePipes = false;

    private Curve[] curves;

    private uint currentPlayerCurve = 0;

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
        }

        //Position the curves at 0,0
        transform.localPosition = new Vector3(0, -curves[0].GetTorusRadius(), 0);

        //Align with controller axis
        transform.rotation = Quaternion.Euler(0, -90, 0);

        AlignPlayerShip();
    }

    private void AlignPlayerShip()
    {
        Transform[] firstPoints = curves[0].getCurveBasePoints();

        playerShip.transform.position = firstPoints[0].position;
        playerShip.transform.forward = firstPoints[0].forward;

        // playerShip.GetComponent<StandaloneController>().rest_rotation = playerShip.transform.rotation.eulerAngles;

        currentPlayerCurve = 0;
    }

    public Curve PrepareNextCurve()
    {
        MoveCurveOrder();
        AlignCurveWithOrigin();
        transform.localPosition = new Vector3(0f, -curves[0].GetTorusRadius());
        return curves[0];
    }

    /// <summary>
    /// Places the just finished curve at the end and shifts all the curves forward in the order
    /// </summary>
    private void MoveCurveOrder()
    {
        Curve finishedCurve = curves[0];
        for (int i = 0; i < curves.Length - 1; i++)
        {
            curves[i] = curves[i + 1];
        }
        curves[curves.Length - 1] = finishedCurve;
    }

    private void AlignCurveWithOrigin()
    {
        Transform currentCurveInOrigin = curves[0].transform;

        //If we set the curves as childs of this curve everything will rotate and move with it
        //Same strategy used with the curvePoints
        for (int i = 1; i < curves.Length; i++)
        {
            curves[i].transform.SetParent(currentCurveInOrigin);
        }

        currentCurveInOrigin.localPosition = Vector3.zero;
        currentCurveInOrigin.localRotation = Quaternion.identity;

        for (int i = 1; i < curves.Length; i++)
        {
            curves[i].transform.SetParent(transform);
        }
    }

    public Curve[] getCurves()
    {
        return curves;
    }

    public uint getCurrentPlayerCurve()
    {
        return currentPlayerCurve;
    }

}
