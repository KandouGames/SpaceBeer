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
    public Sound mainSound;
    public Sound buttonClickSound;
    public Sound shootSound;
    public Sound planetPortalSound;
    public Sound beerPortalSound;

    //IDs
    public int mainSoundID;
    public int shootSoundID;

    private void Awake()
    {
        volumeSlider.value = mainVolume;
        volumeSlider.onValueChanged.AddListener(UpdateVolume);

        InitializeSound(buttonClickSound);
        InitializeSound(mainSound);
        InitializeSound(shootSound);
        InitializeSound(planetPortalSound);
        InitializeSound(beerPortalSound);

        if (SceneManager.GetActiveScene().name == "MainMenu")
            mainSound.source.Play();

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
                mainSound.loop = true;
                mainSound.source.Play();
                uiManager.ShowSong(mainSound.clip.name);
            }

            else if (distance % 2000 == 0)
            {
                changeSong();
                uiManager.ShowSong(mainSound.clip.name);
            }
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
        mainSound.source.volume = mainSound.volume * mainVolume;
        buttonClickSound.source.volume = buttonClickSound.volume * mainVolume;

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

    }
    public void PlayPlanetPortal()
    {
        planetPortalSound.source.Play();
    }

    public void PlayBeerPortal()
    {
        beerPortalSound.source.Play();
    }

    public void changeSong()
    {
        mainSoundID = (mainSoundID + 1) % mainClips.Count;

        mainSound.source.Stop();
        mainSound.source.clip = mainClips[mainSoundID];
        mainSound.clip = mainClips[mainSoundID];
        mainSound.source.Play();
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
