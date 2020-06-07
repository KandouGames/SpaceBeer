using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class PostProcessManager : MonoBehaviour
{
    public PostProcessVolume volume;
    private ColorGrading colorGrading;

    void Awake()
    {
        volume.profile.TryGetSettings(out colorGrading);

        colorGrading.active = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PPSlowMotion(bool active)
    {
        if (active)
            colorGrading.active = true;
        else
            colorGrading.active = false;

    }
}
