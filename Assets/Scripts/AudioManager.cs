
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.playOnAwake = false;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mixer;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        s.source.Play();
        Debug.Log("Playing: " + s.source.clip.name + " From: " + s.name);
        if (s == null)
        {
            Debug.LogWarning(name + "wasn't found");
            return;
        }
    }
}

/* Old solution 2

using System;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public Sound[] sounds;
	//public Sound testSound;
    public static MusicManager instance;
	int updateCounter=0;
	AudioSource[] audiosrc;

	void OnEnable()
	{
		TimingMachine.productDetected+=disableMusic;
		MachineProperties.moveSound+=disableMusic;
	}
	
	void OnDisable()
	{
		TimingMachine.productDetected-=disableMusic;
		MachineProperties.moveSound-=disableMusic;
	}
	
	
    void Start()
    {
        if (instance == null)
            instance = this;
        else 
		{ 
            Destroy(gameObject);
             return;
        }

            DontDestroyOnLoad(gameObject);

        for(int i=0;i<sounds.Length;i++) //foreach sound in Sounds
        {
			//Debug.Log(sounds[i].name);
            //gameObject.AddComponent<AudioSource>();
			//s.source.clip = s.clip;
			//s.source.volume = s.volume;
            //s.source.loop = s.loop;
			gameObject.AddComponent<AudioSource>().clip=sounds[i].clip;

        }
		//gameObject.AddComponent<AudioSource>().clip=testSound.clip;
		audiosrc=gameObject.GetComponents<AudioSource>();
		//audiosrc[1].Play(); //funkar fint
		//testSound;
		
		//disableMusic("tre");
    }
	
	void Update()
	{
		//Play("gregert");
		if(updateCounter%20==9)
			//disableMusic("tre");
		updateCounter++;
	}
	
	public void disableMusic(string name)
	{
		Debug.Log("music to my ears");
		for(int i=0;i<sounds.Length;i++)
			if(sounds[i].name==name)
				audiosrc[i].Play();
			else
				Debug.LogWarning(name + "wasn't found");
	}

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        //s.source.Play();
        if (s == null)
        {
            Debug.LogWarning(name + "wasn't found");
            return;
        }

    }

/*
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

/*
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

}
*/
