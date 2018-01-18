using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    public AudioClip AudioClip;
    public AudioSource AudioSource;
    public float UpdateInterval = 0.1f;
    public int sampleDataLength = 1024;

    private float clipTime = 0f;
    private float clipVolume;
    private float[] clipSampleData;

    void Awake()
    {
        Initialize();
    }

    void Update()
    {

        clipTime += Time.deltaTime;
        if (clipTime >= UpdateInterval)
        {
            clipTime = 0f;
            AudioSource.clip.GetData(clipSampleData, AudioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipVolume = 0f;
            foreach (float sample in clipSampleData)
            {
                clipVolume += Mathf.Abs(sample);
            }
            clipVolume /= sampleDataLength;
            Debug.Log(clipVolume);
        }
    }

    void Initialize()
    {
        clipSampleData = new float[sampleDataLength];
        
    }
}
