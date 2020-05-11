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
        Vector3 currentAngle = new Vector3(Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
                                           Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
                                           Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;
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
