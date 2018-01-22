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

    [SerializeField]
    private AudioSource[] audioSources;
    private float volume;


    void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    //Old solution
    /*
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


     public void ToggleLoop()
     {
         audioSource.loop = !audioSource.loop;
     }

     public void SetLoop(bool loop)
     {
         audioSources[index].loop = loop;
     }*/

    public void StopMusic(AudioClip audioClip, int index)
{
    audioSources[index].clip = audioClip;
    audioSources[index].Stop();
}

    public void PlayMusic(AudioClip audioClip, int index, bool loop)
    {
        // Index 0: Music, 1: Sound Effects, 2: Something else
        audioSources[index].clip = audioClip;
        audioSources[index].volume = volume;
        audioSources[index].PlayOneShot(audioClip, volume);
        audioSources[index].loop = !audioSources[index].loop;
        audioSources[index].loop = loop;

    }

}
