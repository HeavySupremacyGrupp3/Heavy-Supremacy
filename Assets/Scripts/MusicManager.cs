using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    [SerializeField]
    private AudioClip music1;
    [SerializeField]
    private AudioClip music2;
    [SerializeField]
    private AudioClip music3;

    private AudioSource audioSource;
    private float volume;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip music1, bool loop)
    {
        GetComponent<AudioSource>().PlayOneShot(music1, volume);
    }

    public void PlayMusic(AudioClip music2, bool loop)
    {
        GetComponent<AudioSource>().PlayOneShot(music2, volume);
    }

    public void PlayMusic(AudioClip music3, bool loop)
    {
        GetComponent<AudioSource>().PlayOneShot(music3, volume);
    }


    public void PlayMusic(AudioClip audioClip, bool loop)
    {
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void StopMusic(AudioClip music1)
    {
        audioSource.Stop();
    }

    public void StopMusic(AudioClip music2)
    {
        audioSource.Stop();
    }

    public void StopMusic(AudioClip music1)
    {
        audioSource.Stop();
    }

    public void ToggleLoop()
    {
        audioSource.loop = !audioSource.loop;
    }

    public void SetLoop(bool loop)
    {
        audioSource.loop = loop;
    }

    //possible solution
   /* public void PlayMusic(AudioClip audioClip, bool loop)
    {
        audioSource.clip = audioClip;
        GetComponent<AudioSource>().PlayOneShot(audioClip, volume);
    }*/

}
