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
    public Text dangerText;
    public List<GameObject> arrayBarrels;
    public Text gameOverRecordText;
    public RectTransform musicAdvisor;
    public Text musicText;
    private Vector2 musicAdvisorPos;

    public List<Image> powerUpsIcons;

    public string mainMenuScene;
    public RectTransform difficultyTextContainer;
    private Text difficultyText;
    private Text difficultyQuoteText;


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

        difficultyQuoteText = difficultyTextContainer?.transform.GetChild(0)?.GetComponent<Text>();
        difficultyText = difficultyTextContainer?.transform.GetChild(1)?.GetComponent<Text>();

        dangerText.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        musicAdvisorPos = musicAdvisor.anchoredPosition;
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
            if (i < barrels && arrayBarrels[i].activeSelf)
            {

                arrayBarrels[i].SetActive(true);
                arrayBarrels[i].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack);
            }
            else
            {
                if (DOTween.IsTweening(arrayBarrels[i]))
                {
                    arrayBarrels[i].transform.DOKill();
                }

                arrayBarrels[i].transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
                {

                    if (barrels == 0)
                    {
                        dangerText.gameObject.SetActive(true);
                        dangerText.DOPlay();
                    }

                    arrayBarrels[i].SetActive(false);
                });
            }
        }

        if (barrels != 0)
        {

            if (DOTween.IsTweening(dangerText))
            {
                dangerText.DOPause();
            }
            dangerText.gameObject.SetActive(false);
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
        image.DOColor(new Color(image.color.r, image.color.g, image.color.b, 0.4f), 1.0f).OnComplete(() =>
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
        advisor.DOAnchorPos(Vector2.zero, 1f);
        yield return new WaitForSeconds(4);
        advisor.DOAnchorPos(musicAdvisorPos, 1f);
    }

    public void LoadMainMenu()
    {
        //Desactivamos las interacciones durante la animación
        gamePlayUI.GetComponent<GraphicRaycaster>().enabled = false;
        Image[] slide = screenFader.GetComponentsInChildren<Image>();

        gameOverUI.SetActive(false);

        foreach (Image img in slide)
        {
            img.DOColor(new Color(img.color.r, img.color.g, img.color.b, 1), 1);
        }

        Color color = screenFader.color;
        screenFader.DOColor(new Color(color.r, color.g, color.b, 1), 1.0f).OnComplete(() =>
        {
            StartCoroutine("LoadScene");
        }
        );
    }


    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(mainMenuScene);
        float timeSinceLoadingStarted = 0f;
        Slider slide = screenFader.GetComponentInChildren<Slider>();


        while (!asyncLoadScene.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoadScene.progress / 0.9f);
            slide.value = progress;

            //Fake timer advancing
            progress += timeSinceLoadingStarted;

            yield return null;
        }
    }
    public void ShowCurrentDifficulty(string difficulty, string quote)
    {
        int quantityToMove = 130;
        float durationOfTween = 0.5f;
        float showDifficultyDuration = 3;

        difficultyText.text = difficulty;
        difficultyQuoteText.text = quote;

        if (DOTween.IsTweening(difficultyTextContainer))
        {
            DOTween.Kill(difficultyTextContainer);
            difficultyTextContainer.transform.position = new Vector3(difficultyTextContainer.transform.position.x,
             difficultyTextContainer.transform.position.y - quantityToMove, difficultyTextContainer.transform.position.z);
        }

        difficultyTextContainer.transform.DOMoveY(quantityToMove, durationOfTween).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            difficultyTextContainer.transform.DOMoveY(-quantityToMove, durationOfTween).SetEase(Ease.InOutBack).SetDelay(showDifficultyDuration);
        });
    }
}
