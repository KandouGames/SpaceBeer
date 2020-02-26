using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combustible : MonoBehaviour
{
    float FULL_FUEL = -90.0f;
    float ZERO_FUEL = 90.0f;
    private Transform needleTransform;

    private float fuelMax;
    private float fuel;

    private void Awake()
    {
        needleTransform = transform.Find("needle");

        fuel = 200f;
        fuelMax = 200f;
    }

    private void Update()
    {
        fuel -= 10f * Time.deltaTime;

        if (fuel < 0) fuel = 0;

        needleTransform.localEulerAngles = new Vector3(0, 0, GetFuelRotation());
    }

    private float GetFuelRotation()
    {
        float totalAngle = Mathf.Abs(FULL_FUEL - ZERO_FUEL);

        //Valores del combustible entre 0 y 1
        float fuelNormalized = (fuelMax - fuel) / fuelMax;

        return FULL_FUEL + fuelNormalized * totalAngle;
    }

}
