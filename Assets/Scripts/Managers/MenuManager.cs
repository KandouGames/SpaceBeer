using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image screenFader;
    public Canvas mainMenuUI;
    public float transitionTime = 1.0f;

    public Text beerCoinsText;

    public MenuPanels menuPanels;
    public List<GameObject> tutorialSlides;
    public GameObject slidePC;
    public GameObject slideAndroid;
    private int slideID;
    public MenuCameraPositions cameraPositions;

    private Camera camera;
    private Trans currentCameraView;
    public float timeTakenDuringLerp = 5.0f;
    //Esta variable guarda el tiempo en el que comenzo la interpolacion
    private float timeStartedLerp;

    public string gameplayScene;

    AsyncOperation asyncLoadScene;

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

        cameraPositions.hangarPanel.position = new Vector3(-0.64f, 22.96f, -46.26f);
        cameraPositions.hangarPanel.rotation = new Vector3(11.342f, -62.838f, 1.547f);

        currentCameraView.position = cameraPositions.mainPanel.position;
        currentCameraView.rotation = cameraPositions.mainPanel.rotation;

        mainMenuUI.GetComponent<GraphicRaycaster>().enabled = false;

        Color color = screenFader.color;
        screenFader.color = new Color(color.r, color.g, color.b, 1);
        screenFader.DOColor(new Color(color.r, color.g, color.b, 0), transitionTime).OnComplete(() =>
        {
            mainMenuUI.GetComponent<GraphicRaycaster>().enabled = true;
        }
        );

        #if UNITY_ANDROID
                slidePC.SetActive(false);
                slideAndroid.SetActive(true);
        #else
                slidePC.SetActive(true);
                slideAndroid.SetActive(false);
        #endif

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

    public void CargaEscena()
    {
        //Pasamos nave y balas a la escena gameplay
        PlayerData playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        GameObject spaceShip = playerData.spaceShipsLP[playerData.spaceShipID];

        //Desparentamos la nave y la bala
        spaceShip.transform.parent = null;
        spaceShip.SetActive(true);
        DontDestroyOnLoad(spaceShip);

        //Desactivamos las interacciones durante la animación
        mainMenuUI.GetComponent<GraphicRaycaster>().enabled = false;
        Image[] slide = screenFader.GetComponentsInChildren<Image>();

        foreach (Image img in slide)
        {
            img.DOColor(new Color(img.color.r, img.color.g, img.color.b, 1), transitionTime / 3);
        }

        Color color = screenFader.color;

        screenFader.DOColor(new Color(color.r, color.g, color.b, 1), transitionTime).OnComplete(() =>
            {
                StartCoroutine("LoadScene");
            }
        );


    }

    //This is technically not necessary anymore but I want to see the repercussions of the audio changes.
    IEnumerator LoadScene()
    {
        yield return null;

        asyncLoadScene = SceneManager.LoadSceneAsync(gameplayScene);
        float timeSinceLoadingStarted = 0f;
        Slider slide = screenFader.GetComponentInChildren<Slider>();


        while (!asyncLoadScene.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoadScene.progress / 0.9f);
            // Debug.Log("Loading progress: " + (progress * 100) + "%");
            slide.value = progress;

            //Fake timer advancing
            progress += timeSinceLoadingStarted;

            yield return null;
        }
    }

    public void ShowMainMenu()
    {
        menuPanels.mainPanel.SetActive(true);
        menuPanels.settingsPanel.SetActive(false);
        menuPanels.creditsPanel.SetActive(false);
        menuPanels.hangarPanel.SetActive(false);
        menuPanels.hangarPanel.transform.Find("ButtonsPanel/ShipsButton").GetComponent<Button>().interactable = false;
        menuPanels.hangarPanel.transform.Find("ButtonsPanel/WeaponsButton").GetComponent<Button>().interactable = true;
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

        beerCoinsText.text = GameObject.Find("PlayerData").GetComponent<PlayerData>().beerCoins.ToString();

        currentCameraView.position = cameraPositions.hangarPanel.position;
        currentCameraView.rotation = cameraPositions.hangarPanel.rotation;
        timeStartedLerp = Time.time;
    }

    public void ShowStore()
    {
        PlayerData playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        playerData.spaceShips[playerData.spaceShipID].SetActive(false);
        playerData.weapons[playerData.weaponID].SetActive(false);
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

    public void ShowTutorial(bool show)
    {
        menuPanels.volumePanel.SetActive(!show);
        menuPanels.tutorialPanel.SetActive(show);
    }

    public void NextSlide()
    {
        if (slideID != tutorialSlides.Count - 1)
        {
            tutorialSlides[slideID].SetActive(false);
            slideID++;
            tutorialSlides[slideID].SetActive(true);
        }
        else
        {
            slideID = 0;
            tutorialSlides[slideID].SetActive(false);
            ShowTutorial(false);
            menuPanels.volumePanel.SetActive(false);
        } 
    }

    public void PreviousSlide()
    {
        if (slideID != tutorialSlides.Count - 1)
        {
            tutorialSlides[slideID].SetActive(false);
            slideID--;
            tutorialSlides[slideID].SetActive(true);
        }

    }

    //Este metodo actualiza el indice y se encarga de enseñar la nave en el menu
    public void Next()
    {
        GameObject botonNave = GameObject.Find("Canvas").transform.Find("HangarPanel/ButtonsPanel/ShipsButton").gameObject;
        GameObject botonArmas = GameObject.Find("Canvas").transform.Find("HangarPanel/ButtonsPanel/WeaponsButton").gameObject;

        PlayerData playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();

        if (!botonNave.GetComponent<Button>().IsInteractable())
        {

            Quaternion rotacionNave;
            //Desactivamos nave actual
            playerData.spaceShips[playerData.spaceShipID].SetActive(false);
            rotacionNave = playerData.spaceShips[playerData.spaceShipID].transform.rotation;

            //Actualizamos id de la nave
            playerData.spaceShipID = (playerData.spaceShipID + 1) % playerData.spaceShips.Count;

            //Activamos nave siguiente
            playerData.spaceShips[playerData.spaceShipID].transform.rotation = rotacionNave;
            playerData.spaceShips[playerData.spaceShipID].SetActive(true);
        }

        else if (!botonArmas.GetComponent<Button>().IsInteractable())
        {
            Quaternion rotacionArma;

            //Desactivamos arma actual
            playerData.weapons[playerData.weaponID].SetActive(false);
            rotacionArma = playerData.weapons[playerData.weaponID].transform.rotation;

            //Actualizamos id de la nave
            playerData.weaponID = (playerData.weaponID + 1) % playerData.weapons.Count;

            //Activamos nave siguiente
            playerData.weapons[playerData.weaponID].transform.rotation = rotacionArma;
            playerData.weapons[playerData.weaponID].SetActive(true);
        }
    }

    public void Previous()
    {
        GameObject botonNave = GameObject.Find("Canvas").transform.Find("HangarPanel/ButtonsPanel/ShipsButton").gameObject;
        GameObject botonArmas = GameObject.Find("Canvas").transform.Find("HangarPanel/ButtonsPanel/WeaponsButton").gameObject;

        PlayerData playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();

        if (!botonNave.GetComponent<Button>().IsInteractable())
        {
            Quaternion rotacionNave;

            //Desactivamos nave actual
            playerData.spaceShips[playerData.spaceShipID].SetActive(false);
            rotacionNave = playerData.spaceShips[playerData.spaceShipID].transform.rotation;

            //Actualizamos id de la nave
            if (playerData.spaceShipID == 0) playerData.spaceShipID = playerData.spaceShips.Count - 1;
            else playerData.spaceShipID--;

            //Activamos nave siguiente
            playerData.spaceShips[playerData.spaceShipID].transform.rotation = rotacionNave;
            playerData.spaceShips[playerData.spaceShipID].SetActive(true);
        }
        else if (!botonArmas.GetComponent<Button>().IsInteractable())
        {
            Quaternion rotacionArma;

            //Desactivamos arma actual
            playerData.weapons[playerData.weaponID].SetActive(false);
            rotacionArma = playerData.weapons[playerData.weaponID].transform.rotation;

            //Actualizamos id de la nave
            if (playerData.weaponID == 0) playerData.weaponID = playerData.weapons.Count - 1;
            else playerData.weaponID--;

            //Activamos nave siguiente
            playerData.weapons[playerData.weaponID].transform.rotation = rotacionArma;
            playerData.weapons[playerData.weaponID].SetActive(true);
        }
    }

    public void ShowSpaceShips()
    {
        PlayerData playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        playerData.spaceShips[playerData.spaceShipID].SetActive(true);

        playerData.weapons[playerData.weaponID].SetActive(false);
    }

    public void ShowWeapons()
    {
        PlayerData playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        playerData.spaceShips[playerData.spaceShipID].SetActive(false);

        playerData.weapons[playerData.weaponID].SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
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
    public GameObject tutorialPanel;
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
