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

        Debug.Log("Before point t: " + firstPoints[0].rotation.eulerAngles + " player pos " + playerShip.transform.rotation.eulerAngles);

        playerShip.transform.position = firstPoints[0].position;

        Vector3 forward = firstPoints[0].forward;

        playerShip.transform.forward = forward;


        Vector2 transformedRotationAxis = new Vector2(-playerShip.transform.rotation.eulerAngles.z, -playerShip.transform.rotation.eulerAngles.x);

        playerShip.GetComponent<StandaloneController>().res_rotation = transformedRotationAxis;

        Debug.Log("point t: " + firstPoints[0].rotation.eulerAngles + " player pos " + playerShip.transform.rotation.eulerAngles);

        // playerShip.transform.forward = forward;
    }

}
