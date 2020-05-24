using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Image screenFader;
    public float transitionTime = 1.0f;

    public MenuPanels menuPanels;
    public MenuCameraPositions cameraPositions;

    private Camera camera;
    private Trans currentCameraView;
    public float timeTakenDuringLerp = 5.0f;
    //Esta variable guarda el tiempo en el que comenzo la interpolacion
    private float timeStartedLerp;

    public void Awake()
    {
        camera = Camera.main;

        currentCameraView = new Trans();

        cameraPositions.mainPanel.position = new Vector3(0.0f, 8.0f, -70.0f);
        cameraPositions.mainPanel.rotation = new Vector3(0f, 0f, 0f);

        cameraPositions.settingsPanel.position = new Vector3(-23.0f, -15.0f, -20.0f);
        cameraPositions.settingsPanel.rotation = new Vector3(0f, 0f, 0f);

        cameraPositions.creditsPanel.position = new Vector3(27.0f, 25.0f, -8.0f);
        cameraPositions.creditsPanel.rotation = new Vector3(0f, 70f, 0f);

        cameraPositions.hangarPanel.position = new Vector3(0.0f, 23.0f, -43.0f);
        cameraPositions.hangarPanel.rotation = new Vector3(11.342f, -70.729f, 0f);

        currentCameraView.position = cameraPositions.mainPanel.position;
        currentCameraView.rotation = cameraPositions.mainPanel.rotation;        
    }

    void Update()
    {

        //Cojo la diferencia de tiempo entre el actual y desde que comenzo la interpolacion para calcular el porcentaje
        float timeSinceStarted = Time.time - timeStartedLerp;
        float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

        camera.transform.position = Vector3.Lerp(camera.transform.position, currentCameraView.position, percentageComplete);
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, Quaternion.Euler(currentCameraView.rotation), percentageComplete);

        //En caso de haber llegado a final la posicion actual sera la misma que la final
        if (percentageComplete >= 1.0f)
        {
            camera.transform.position = currentCameraView.position;
            camera.transform.rotation = Quaternion.Euler(currentCameraView.rotation);
        }
    }

    public void CargaEscena(string pNombreScene)
    {
        Color color = screenFader.color;
        screenFader.DOColor(new Color(color.r, color.g, color.b, 1), transitionTime).OnComplete(() =>
            {
                SceneManager.LoadScene(pNombreScene);
            }
        );
    }

    public void ShowMainMenu()
    {
        menuPanels.mainPanel.SetActive(true);
        menuPanels.settingsPanel.SetActive(false);
        menuPanels.creditsPanel.SetActive(false);
        menuPanels.hangarPanel.SetActive(false);
        menuPanels.exitPanel.SetActive(false);
        menuPanels.volumePanel.SetActive(false);

        currentCameraView.position = cameraPositions.mainPanel.position;
        currentCameraView.rotation = cameraPositions.mainPanel.rotation;
        timeStartedLerp = Time.time;
    }

    public void ShowSettingsMenu()
    {
        menuPanels.mainPanel.SetActive(false);
        menuPanels.settingsPanel.SetActive(true);
        menuPanels.creditsPanel.SetActive(false);
        menuPanels.hangarPanel.SetActive(false);
        menuPanels.exitPanel.SetActive(false);
        menuPanels.volumePanel.SetActive(false);

        currentCameraView.position = cameraPositions.settingsPanel.position;
        currentCameraView.rotation = cameraPositions.settingsPanel.rotation;
        timeStartedLerp = Time.time;
    }

    public void ShowCreditsMenu()
    {
        menuPanels.mainPanel.SetActive(false);
        menuPanels.settingsPanel.SetActive(false);
        menuPanels.creditsPanel.SetActive(true);
        menuPanels.hangarPanel.SetActive(false);
        menuPanels.exitPanel.SetActive(false);
        menuPanels.volumePanel.SetActive(false);

        currentCameraView.position = cameraPositions.creditsPanel.position;
        currentCameraView.rotation = cameraPositions.creditsPanel.rotation;
        timeStartedLerp = Time.time;
    }

    public void ShowHangarMenu()
    {
        menuPanels.mainPanel.SetActive(false);
        menuPanels.settingsPanel.SetActive(false);
        menuPanels.creditsPanel.SetActive(false);
        menuPanels.hangarPanel.SetActive(true);
        menuPanels.exitPanel.SetActive(false);
        menuPanels.volumePanel.SetActive(false);

        currentCameraView.position = cameraPositions.hangarPanel.position;
        currentCameraView.rotation = cameraPositions.hangarPanel.rotation;
        timeStartedLerp = Time.time;
    }

    public void ShowExitMenu()
    {
        menuPanels.mainPanel.SetActive(false);
        menuPanels.settingsPanel.SetActive(false);
        menuPanels.creditsPanel.SetActive(false);
        menuPanels.hangarPanel.SetActive(false);
        menuPanels.exitPanel.SetActive(true);
        menuPanels.volumePanel.SetActive(false);
    }

    public void ShowVolumePanel()
    {
        menuPanels.mainPanel.SetActive(false);
        menuPanels.settingsPanel.SetActive(true);
        menuPanels.creditsPanel.SetActive(false);
        menuPanels.hangarPanel.SetActive(false);
        menuPanels.exitPanel.SetActive(false);
        menuPanels.volumePanel.SetActive(true);
    }
}

[System.Serializable]
public class MenuPanels
{
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public GameObject hangarPanel;
    public GameObject exitPanel;
    public GameObject volumePanel;
}

[System.Serializable]
public class Trans
{
    public Vector3 position;
    public Vector3 rotation;
}

[System.Serializable]
public class MenuCameraPositions
{
    public Trans mainPanel;
    public Trans settingsPanel;
    public Trans creditsPanel;
    public Trans hangarPanel;
}
