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
        AlignPlayerShip();
    }

    private void AlignPlayerShip()
    {
        Transform[] firstPoints = curves[0].getCurveBasePoints();

        playerShip.transform.position = firstPoints[0].position;
        playerShip.transform.forward = firstPoints[0].forward;

        playerShip.GetComponent<StandaloneController>().rest_rotation = playerShip.transform.rotation.eulerAngles;

    }

}
