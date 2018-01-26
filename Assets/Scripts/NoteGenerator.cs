using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteGenerator : MonoBehaviour
{
    public AudioClip Music;
    public AudioClip NoteGenerationAudio;
    public AudioSource MusicAudioSource;
    public AudioSource NoteGenerationAudioSource;
    public float UpdateInterval = 0.1f; //For optimizing performance.
    private int SampleDataLength = 1024;  //1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
    public float MusicStartDelay = 1;
    public float NoteGenerationStartDelay = 1;
    public GameObject NotePrefab;
    public float NoteSpawnMinInterval = 0.1f;
    public GameObject EndGamePanel;

    public static int NoteMultiplier = 1;

    private float clipTime = 0;
    private float clipVolume;
    private float lastClipVolume;
    private float volumeTreshold = 0.1f;
    private float[] clipSampleData;
    private bool canSendNextNote = true;
    private float noteSpawnTimer = 0;

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
        NoteGenerationAudioSource.PlayDelayed(NoteGenerationStartDelay);
    }

    void Update()
    {
        if (NoteGenerationAudioSource.isPlaying && CheckForNote() && noteSpawnTimer >= NoteSpawnMinInterval)
            SendNote();
        else if (!MusicAudioSource.isPlaying && Application.isFocused) //End game if song is over.
            EndGamePanel.SetActive(true);
    }

    bool CheckForNote()
    {
        noteSpawnTimer += Time.deltaTime;
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
            if (clipVolume >= lastClipVolume)
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
        for (int i = 0; i < NoteMultiplier; i++)
        {
            Instantiate(NotePrefab, transform.position, Quaternion.identity);
        }
        noteSpawnTimer = 0;
    }

    public void LoadHub()
    {
        if (FindObjectOfType<GameManager>() != null)
            FindObjectOfType<GameManager>().LoadHUB();

        //If you start the game in the Practice-scene.
        else
            SceneManager.LoadScene(0);
    }
}
