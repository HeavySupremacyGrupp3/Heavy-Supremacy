using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "Song", order = 1)]
public class Song : ScriptableObject
{ 
    public AudioClip MusicWithLead, MusicWithoutLead;
    public AudioClip[] MIDIMusic;
    public float GeneratorUpdateInterval, MinimumVolumeTreshold, MusicDelay = 1.8f;
    public string SongName;
}
