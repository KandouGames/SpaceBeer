using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public UIManager uiManager;

    public Slider volumeSlider;

    [Range(0.0f, 1.0f)]
    public float mainVolume = 1.0f;

    public List<AudioClip> clips;
    public Sound mainSong;
    public Sound buttonClickSound;


    public int songID;

    private void Awake()
    {
        volumeSlider.value = mainVolume;
        volumeSlider.onValueChanged.AddListener(UpdateVolume);


        InitializeSound(buttonClickSound);
        InitializeSound(mainSong);

        mainSong.loop = true;
        mainSong.source.Play();
        uiManager.ShowSong(mainSong.clip.name);
    }


    private void Update()
    {
        ulong distance = scoreManager.getDistance() + 1;
        if (distance % 100 == 0)
        {
            changeSong();
            uiManager.ShowSong(mainSong.clip.name);
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
        mainSong.source.volume = mainSong.volume * mainVolume;
        buttonClickSound.source.volume = buttonClickSound.volume * mainVolume;

    }

    public void PlayButtonPressed()
    {
        buttonClickSound.source.Play();
    }

    public void changeSong()
    {
        Debug.Log(songID);
        songID = (songID + 1) % clips.Count;

        mainSong.source.Stop();
        mainSong.source.clip = clips[songID];
        mainSong.clip = clips[songID];
        mainSong.source.Play();
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
