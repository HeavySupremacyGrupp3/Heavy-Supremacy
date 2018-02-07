using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Options : MonoBehaviour {

    public AudioMixer am;

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
