using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveSystem : MonoBehaviour
{
    public Curve curvePrefab;

    public int curveCount;
    public bool generatePipes = false;

    private Curve[] curves;

    private void Awake()
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
    }

}
