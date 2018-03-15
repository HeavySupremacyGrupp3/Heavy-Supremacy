using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundTrigger : MonoBehaviour {

    public string SoundName;

    private AudioManager audioMgr;

    public void OnEnable()
    {
        GetAudioManager();
        if (!string.IsNullOrEmpty(SoundName))
            audioMgr.Play(SoundName);
    }

    public void PlaySound(string soundName)
    {
        audioMgr.Play(soundName);
    }

    private void GetAudioManager()
    {
        if (audioMgr == null)
            audioMgr = FindObjectOfType<AudioManager>();
    }
}
