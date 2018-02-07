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

    public GameObject[] NotePrefabs;
    public float[] NoteSpawnXOffset;

    public float NoteSpawnMinInterval = 0.1f;
    public GameObject EndGamePanel;

    public static int NoteMultiplier = 1;
    public static int NumberOfUniqueNotes = 3;

    public AudioClip VictorySound;
    public AudioClip DefeatSound;

    private float clipTime = 0;
    private float clipVolume;
    private float lastClipVolume;
    private float volumeTreshold = 0.1f;
    private float[] clipSampleData;
    private bool canSendNextNote = true;
    private float noteSpawnTimer = 0;
    private int noteIndex = 0;

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
        if (NoteGenerationAudioSource.isPlaying && CheckForNote() && noteSpawnTimer >= NoteSpawnMinInterval && !EndGamePanel.activeSelf)
            SendNote();
        else if (!MusicAudioSource.isPlaying && Application.isFocused && !EndGamePanel.activeSelf) //End game if song is over and the game hasn't already ended.
            EndGame(true);
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
            //Debug.Log(clipVolume);

            //Set volumetreshold to the volume of the first note.
            if (volumeTreshold <= 1 && clipVolume > 1 || clipVolume < volumeTreshold && clipVolume > 1)
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
        int tempIndex = 0;
        for (int i = 0; i < NoteMultiplier; i++)
        {
            do
            {
                tempIndex = Random.Range(0, NumberOfUniqueNotes);
            }
            while (noteIndex == tempIndex);
            noteIndex = tempIndex;

            Instantiate(NotePrefabs[noteIndex], new Vector2(transform.position.x + NoteSpawnXOffset[noteIndex], transform.position.y), Quaternion.identity);
        }
        noteSpawnTimer = 0;
    }

    public void EndGame(bool victory)
    {
        EndGamePanel.SetActive(true);
        MusicAudioSource.Stop();
        NoteGenerationAudioSource.Stop();

        if (victory)
        {
            MusicAudioSource.PlayOneShot(VictorySound);

            if (FindObjectOfType<fameStatScript>().getAmount() >= FindObjectOfType<fameStatScript>().getMax() && GigBackgroundManager.GigSession)
            {
                GameManager.ToEndGame = true;
                GameManager.EndGameTitleText = "You're famous and won the game!";
            }
        }
        else
        {
            MusicAudioSource.PlayOneShot(DefeatSound);
        }
    }

    public void LoadHub()
    {
        if (FindObjectOfType<GameManager>() != null)
            FindObjectOfType<GameManager>().LoadHUB();

        //If you start the game in the Practice-scene.
        else
            SceneManager.LoadScene(0);
    }

    public static void Reset()
    {
        NoteMultiplier = 1;
    }
}
