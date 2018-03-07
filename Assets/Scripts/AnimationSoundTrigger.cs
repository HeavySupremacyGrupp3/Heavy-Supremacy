using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundTrigger : MonoBehaviour {

    public string SoundName;

    public void OnEnable()
    {
        FindObjectOfType<AudioManager>().Play(SoundName);
    }
}
