using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    public AudioMixer am;
    public Slider volumeSlider;
    public Slider SFXSlider;
    public Slider musicSlider;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    void Update()
    {
        PlayerPrefs.SetFloat("MasterVolume", volumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void SetVolume(float volume)
    {

        am.SetFloat("MasterVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {

        am.SetFloat("SFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {

        am.SetFloat("MusicVolume", volume);
    }

}
