using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider volumeSlider;

    [Range(0.0f, 1.0f)]
    public float mainVolume = 1.0f;

    public Sound mainMenuSong;
    public Sound buttonClickSound;

    private void Awake()
    {
        volumeSlider.value = mainVolume;
        volumeSlider.onValueChanged.AddListener(UpdateVolume);


        InitializeSound(buttonClickSound);
        InitializeSound(mainMenuSong);
    }

    private void Start()
    {
        mainMenuSong.source.Play();
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
        mainMenuSong.source.volume = mainMenuSong.volume * mainVolume;
        buttonClickSound.source.volume = buttonClickSound.volume * mainVolume;

    }

    public void PlayButtonPressed()
    {
        buttonClickSound.source.Play();
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
