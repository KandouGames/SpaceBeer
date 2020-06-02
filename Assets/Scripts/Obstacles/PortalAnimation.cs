using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PortalAnimation : MonoBehaviour
{
    Light pointLight;
    private float angle;
    private float speed = 5.0f;
    private bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        pointLight = this.gameObject.GetComponentInChildren<Light>();
        angle = Random.Range(0.0f, 2 * Mathf.PI);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            angle = (angle < 0.0f) ? 2 * Mathf.PI : angle - speed * Time.deltaTime;
            pointLight.intensity = 0.3f + Mathf.Sin(angle) * 0.7f;
        }
    }

    public void Activate(float fadeTime)
    {
        isActive = true;
        pointLight.DOIntensity(1, fadeTime);
    }

    public void Deactivate(float fadeTime)
    {
        isActive = false;
        pointLight.DOIntensity(0.25f, fadeTime);
    }
}
