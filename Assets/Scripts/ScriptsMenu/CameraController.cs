using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform[] views;
    Transform currentView;
    Button backHangar;

    float timeTakenDuringLerp = 5f;

    //Esta variable guarda el tiempo en el que comenzo la interpolacion
    float timeStartedLerp;

    // Start is called before the first frame update
    void Start()
    {
        //Default
        currentView = views[0];
    }

    void Update()
    {
        //Cogo la diferencia de tiempo entre el actual y desde que comenzo la interpolacion para calcular el porcentaje
        float timeSinceStarted = Time.time - timeStartedLerp;
        float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

        transform.position = Vector3.Lerp(transform.position, currentView.position, percentageComplete);
        transform.rotation = Quaternion.Lerp(transform.rotation, currentView.rotation, percentageComplete);

        //En caso de haber llegado a final la posicion actual sera la misma que la final
        if (percentageComplete >= 1.0f)
        {
            transform.position = currentView.position;
            transform.rotation = currentView.rotation;
        }
    }


    void SetCameraStartMenu()
    {
        currentView = views[0];
        timeStartedLerp = Time.time;
    }

    void SetCameraHangarMenu()
    {
        currentView = views[1];
        timeStartedLerp = Time.time;
    }
    void SetCameraSettingsMenu()
    {
        currentView = views[2];
        timeStartedLerp = Time.time;
    }
    void SetCameraCreditsMenu()
    {
        currentView = views[3];
        timeStartedLerp = Time.time;
    }

}
