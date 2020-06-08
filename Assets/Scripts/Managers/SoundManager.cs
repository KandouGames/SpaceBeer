using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public GameManager gameManager;
    public ScoreManager scoreManager;
    public UIManager uiManager;

    public Slider volumeSlider;

    [Range(0.0f, 1.0f)]
    public float mainVolume = 1.0f;

    //Lists
    public List<AudioClip> mainClips;
    public List<AudioClip> shootClips;

    //Sounds
    public Sound mainTheme;
    public Sound buttonClickSound;
    public Sound shootSound;
    public Sound planetPortalSound;
    public Sound beerPortalSound;
    public Sound crashSound;
    public Sound finalCrashSound;
    public Sound gameOverSound;

    //IDs
    public int mainSoundID;
    public int shootSoundID;

    //Auxiliar
    private Level prevLevel;

    private void Awake()
    {

        volumeSlider.value = mainVolume;
        volumeSlider.onValueChanged.AddListener(UpdateVolume);

        InitializeSound(buttonClickSound);
        InitializeSound(mainTheme);
        InitializeSound(shootSound);
        InitializeSound(planetPortalSound);
        InitializeSound(beerPortalSound);
        InitializeSound(crashSound);
        InitializeSound(finalCrashSound);
        InitializeSound(gameOverSound);

        if (SceneManager.GetActiveScene().name == "MainMenu")
            mainTheme.source.Play();

        prevLevel = scoreManager.currentLevel;
    }

    private void Start()
    {
        //Shoot sound comes from weapon selected in MainMenu
        if (shootClips.Count != 0 && shootSoundID < shootClips.Count && shootClips[shootSoundID] != null) shootSound.source.clip = shootClips[shootSoundID];
    }


    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (!gameManager.paused)
            {
                ulong distance = scoreManager.getDistance();
                if (distance == 0)
                {
                    if (mainTheme.source.clip is null || mainTheme.clip is null)
                    {
                        mainSoundID = (Random.Range(0, mainClips.Count) + 1) % mainClips.Count;
                        mainTheme.source.clip = mainClips[mainSoundID];
                        mainTheme.clip = mainClips[mainSoundID];
                    }
                    mainTheme.loop = true;
                    mainTheme.source.Play();
                    uiManager.ShowSong(mainTheme.clip.name);
                }
                else if (scoreManager.currentLevel != prevLevel)
                {
                    changeSong();
                    uiManager.ShowSong(mainTheme.clip.name);
                }

                prevLevel = scoreManager.currentLevel;
            }

        }
    }

    private void InitializeSound(Sound s)
    {
        s.source = this.gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume * mainVolume;
        s.source.loop = s.loop;
    }

    public void UpdateVolume(float value)
    {
        mainVolume = value;
        mainTheme.source.volume = mainTheme.volume * mainVolume;
        buttonClickSound.source.volume = buttonClickSound.volume * mainVolume;
        shootSound.source.volume = shootSound.volume * mainVolume;
        planetPortalSound.source.volume = planetPortalSound.volume * mainVolume;
        beerPortalSound.source.volume = beerPortalSound.volume * mainVolume;
        crashSound.source.volume = crashSound.volume * mainVolume;
        finalCrashSound.source.volume = finalCrashSound.volume * mainVolume;
        gameOverSound.source.volume = gameOverSound.volume * mainVolume;
    }

    public void PlayButtonPressed()
    {
        buttonClickSound.source.Play();
    }

    public void PlayShoot()
    {
        shootSound.source.Play();
    }

    public void PlayCrashSound()
    {
        crashSound.source.Play();
    }

    public void PlayFinalCrashSound()
    {
        finalCrashSound.source.Play();
    }

    public void PlayPlanetPortal()
    {
        planetPortalSound.source.Play();
    }

    public void PlayBeerPortal()
    {
        beerPortalSound.source.Play();
    }

    public void PlayGameOver()
    {
        gameOverSound.source.Play();
    }

    public void changeSong()
    {
        mainSoundID = (mainSoundID + 1) % mainClips.Count;

        mainTheme.source.Stop();
        mainTheme.source.clip = mainClips[mainSoundID];
        mainTheme.clip = mainClips[mainSoundID];
        mainTheme.source.Play();
    }
}

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;

    public bool loop = false;

    [HideInInspector]
    public AudioSource source;

}
