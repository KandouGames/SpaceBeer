using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [HideInInspector]
    public GameManager gameManager;

    public Canvas gamePlayUI;
    public Image screenFader;

    public GameObject gameOverUI;
    public GameObject gameInfoUI;
    public GameObject pauseUI;
    

    public List<RectTransform> tutorialSlides;
    public int tutorialSlideID;
    private float speedSlides = 0.25f;

    public Text beerCoinsText;
    public Text distanceText;
    public List<GameObject> arrayBarrels;
    public Text gameOverRecordText;
    public RectTransform musicAdvisor;
    public Text musicText;

    public List<Image> powerUpsIcons;

    public string mainMenuScene;

    void Awake()
    {
        gamePlayUI.GetComponent<GraphicRaycaster>().enabled = false;
        Color color = screenFader.color;
        screenFader.color = new Color(color.r, color.g, color.b, 1);
        screenFader.DOColor(new Color(color.r, color.g, color.b, 0), 1.0f).OnComplete(() =>
        {
            gamePlayUI.GetComponent<GraphicRaycaster>().enabled = true;
        }
        );
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            gameManager.PauseGame();
            pauseUI.SetActive(true);
        }

    }

    public void SetBeerCoins(int beerCoins)
    {
        beerCoinsText.text = beerCoins.ToString();
    }

    public void SetDistance(ulong distance)
    {
        distanceText.text = distance.ToString();
    }

    public void SetBarrels(int barrels)
    {
        for (int i = 0; i < arrayBarrels.Count; ++i)
        {
            if (i < barrels)
                arrayBarrels[i].SetActive(true);
            else
                arrayBarrels[i].SetActive(false);
        }
    }

    public void ShowGameOver(int beerCoins, ulong distance)
    {
        gameManager.PauseGame();
        gameOverUI.SetActive(true);
        gameOverRecordText.text = "You won " + beerCoins.ToString() + " beercoins \n and have traveled " + distance.ToString() + " km";
    }

    public void ShowSong(string songName)
    {
        musicText.text = songName;
        StartCoroutine(wait(musicAdvisor));
    }

    public void ShowGameInfo()
    {
        gameInfoUI.SetActive(true);
    }


    public void ShowPowerUpIcon(Image image)
    {
        image.transform.DOShakePosition(2.0f, 4, 100);
        image.transform.DOScale(new Vector3(2, 2, 2), 1);
        image.DOColor(new Color(image.color.r, image.color.g, image.color.b, 1), 1.0f).OnComplete(() =>
        {
            image.DOColor(new Color(image.color.r, image.color.g, image.color.b, 0), 0.5f);
        }
        );
    }
    


    public void NextSlide()
    {
        if (tutorialSlideID != tutorialSlides.Count - 1)
        {
            RectTransform actualSlide = tutorialSlides[tutorialSlideID];
            tutorialSlideID++;
            RectTransform nextSlide = tutorialSlides[tutorialSlideID];


            actualSlide.DOAnchorPos(new Vector2(-1140, 0), speedSlides);
            actualSlide.gameObject.SetActive(false);
            nextSlide.gameObject.SetActive(true);
            nextSlide.DOAnchorPos(Vector2.zero, speedSlides);
        }
        else
        {
            //Start game
            RectTransform actualSlide = tutorialSlides[tutorialSlideID];
            actualSlide.DOAnchorPos(new Vector2(-1140, 0), speedSlides);
            actualSlide.gameObject.SetActive(false);
            actualSlide.transform.parent.gameObject.SetActive(false);

            ShowGameInfo();
            gameManager.ResumeGame();
        }
    }

    public void PreviousSlide()
    {
        RectTransform actualSlide = tutorialSlides[tutorialSlideID];
        tutorialSlideID--;
        RectTransform previousSlide = tutorialSlides[tutorialSlideID];


        actualSlide.DOAnchorPos(new Vector2(1140, 0), speedSlides);
        actualSlide.gameObject.SetActive(false);
        previousSlide.gameObject.SetActive(true);
        previousSlide.DOAnchorPos(Vector2.zero, speedSlides);
    }




    IEnumerator wait(RectTransform advisor)
    {
        Vector2 anchored = advisor.anchoredPosition;
        advisor.DOAnchorPos(Vector2.zero, 1f);
        yield return new WaitForSeconds(4);
        advisor.DOAnchorPos(anchored, 1f);
    }

    public void LoadMainMenu()
    {
        //Desactivamos las interacciones durante la animación
        gamePlayUI.GetComponent<GraphicRaycaster>().enabled = false;

        Color color = screenFader.color;
        screenFader.DOColor(new Color(color.r, color.g, color.b, 1), 1.0f).OnComplete(() =>
        {
            //Hide Load Time in screenfade
            SceneManager.LoadScene(mainMenuScene);

        }
        );
    }
}
