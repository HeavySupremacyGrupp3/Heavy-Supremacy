using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    public AudioClip AudioClip;
    public AudioSource AudioSource;
    public float UpdateInterval = 0.1f; //For optimizing performance.
    private int SampleDataLength = 1024;  //1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
    public float StartDelay = 1;
    private float volumeTreshold = 1;
    public GameObject NotePrefab;

    private float clipTime = 0f;
    private float clipVolume;
    private float lastClipVolume;
    private float[] clipSampleData;
    private bool canSendNextNote = true;

    void Start()
    {
        StartCoroutine(InitializeDelayTimer());
    }

    IEnumerator InitializeDelayTimer()
    {
        yield return new WaitForSeconds(StartDelay);
        Initialize();
    }

    void Initialize()
    {
        clipSampleData = new float[SampleDataLength];
        AudioSource.clip = AudioClip;
        AudioSource.Play();
    }

    void Update()
    {

        if (AudioSource.isPlaying && CheckForNote())
            SendNote();
    }

    bool CheckForNote()
    {
        clipTime += Time.deltaTime;
        if (clipTime >= UpdateInterval)
        {
            clipTime = 0f;
            AudioSource.clip.GetData(clipSampleData, AudioSource.timeSamples);
            foreach (float sample in clipSampleData)
            {
                clipVolume += Mathf.Abs(sample);
            }
            //clipVolume /= SampleDataLength; //Used for what?
            Debug.Log(clipVolume);

            if (volumeTreshold <= 1 && clipVolume > 1)
                volumeTreshold = clipVolume;

            //If the tone is long, create one note.
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
        Instantiate(NotePrefab, Vector3.zero, Quaternion.identity);
    }
}
