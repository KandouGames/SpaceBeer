using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;
    Button backHangar;  

    // Start is called before the first frame update
    void Start()
    {
        //Default
        currentView = views[0];
    }

    void Update()
    {

    }

    //LateUpdate para interpolaciones lineales
    void LateUpdate()
    {
        
        //Lerp position
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);
        //Lerp rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, currentView.rotation, Time.deltaTime * transitionSpeed);
    }

    void SetCameraStartMenu()
    {
        currentView = views[0];
    }

    void SetCameraHangarMenu()
    {
        currentView = views[1];
    }
    void SetCameraSettingsMenu()
    {
        currentView = views[2];
    }
    void SetCameraCreditsMenu()
    {
        currentView = views[3];
    }

}
