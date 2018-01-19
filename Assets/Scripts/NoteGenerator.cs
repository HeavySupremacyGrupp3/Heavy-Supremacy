using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    public AudioClip Music;
    public AudioClip NoteGenerationAudio;
    public AudioSource MusicAudioSource;
    public AudioSource NoteGenerationAudioSource;
    public float UpdateInterval = 0.1f; //For optimizing performance.
    private int SampleDataLength = 1024;  //1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
    public float MusicStartDelay = 1;
    private float volumeTreshold = 1;
    public GameObject NotePrefab;

    private float clipTime = 0f;
    private float clipVolume;
    private float lastClipVolume;
    private float[] clipSampleData;
    private bool canSendNextNote = true;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        clipSampleData = new float[SampleDataLength];

        MusicAudioSource.clip = Music;
        MusicAudioSource.PlayDelayed(MusicStartDelay);

        NoteGenerationAudioSource.clip = NoteGenerationAudio;
        NoteGenerationAudioSource.Play();
    }

    void Update()
    {

        if (NoteGenerationAudioSource.isPlaying && CheckForNote())
            SendNote();
    }

    bool CheckForNote()
    {
        clipTime += Time.deltaTime;
        if (clipTime >= UpdateInterval)
        {
            clipTime = 0f;
            NoteGenerationAudioSource.clip.GetData(clipSampleData, NoteGenerationAudioSource.timeSamples);
            foreach (float sample in clipSampleData)
            {
                clipVolume += Mathf.Abs(sample);
            }
            //clipVolume /= SampleDataLength; //Used for what?
            //Debug.Log(clipVolume);

            //Set volumetreshold to the volume of the first note.
            if (volumeTreshold <= 1 && clipVolume > 1)
                volumeTreshold = clipVolume;

            //If the tone is long, create only one note.
            if (clipVolume > lastClipVolume + volumeTreshold)
                canSendNextNote = true;
            lastClipVolume = clipVolume;

            if (clipVolume >= volumeTreshold && canSendNextNote)
            {
                clipVolume = 0f;
                canSendNextNote = false;
                return true;
            }
        }
        clipVolume = 0f;
        return false;
    }

    void SendNote()
    {
        Instantiate(NotePrefab, transform.position, Quaternion.identity);
    }
}
