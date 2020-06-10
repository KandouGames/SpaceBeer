using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnimation : MonoBehaviour
{
    Light pointLight;
    private float angle;
    private float speed = 10.0f;
    public bool disappear = false;
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

        if (disappear)
            StartCoroutine(BlinkShield());

    }

    IEnumerator BlinkShield()
    {
        while (disappear)
        {
            this.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);


            this.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
