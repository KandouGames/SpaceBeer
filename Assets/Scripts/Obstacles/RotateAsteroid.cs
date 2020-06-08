using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAsteroid : MonoBehaviour
{
    private Vector3 randomAxis;

    void Start()
    {
        this.transform.Rotate(new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)));

        randomAxis = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        randomAxis = randomAxis.normalized * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(randomAxis);
    }
}
