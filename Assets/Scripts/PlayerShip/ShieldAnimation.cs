using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnimation : MonoBehaviour
{
    Light pointLight;
    private float angle;
    private float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        pointLight = this.gameObject.GetComponent<Light>();
        pointLight.intensity = 2;

        angle = 2 * Mathf.PI;
    }

    // Update is called once per frame
    void Update()
    {
        angle = (angle < 0.0f) ? 2 * Mathf.PI : angle - speed * Time.deltaTime;
        pointLight.range = 3.7f + Mathf.Cos(angle) * 1.1f;
    }
}
