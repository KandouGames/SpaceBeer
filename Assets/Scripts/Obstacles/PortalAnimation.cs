using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalAnimation : MonoBehaviour
{
    public Light pointLight;
    private float angle;
    private float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        pointLight = this.gameObject.GetComponentInChildren<Light>();
        angle = Random.Range(0.0f, 2 * Mathf.PI);
    }

    // Update is called once per frame
    void Update()
    {
        angle = (angle < 0.0f) ? 2 * Mathf.PI : angle - speed * Time.deltaTime;
        pointLight.intensity = 0.3f + Mathf.Sin(angle) * 0.7f;
    }
}
