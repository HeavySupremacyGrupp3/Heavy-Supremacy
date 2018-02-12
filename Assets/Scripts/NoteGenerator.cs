using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject TutorialPanel;
    public Text FameText;
    public Text MoneyText;
    public Text AngstText;
    public Text MetalText;

    public static int NoteMultiplier = 1;
    public static int NumberOfUniqueNotes = 3;
    public static float NotesTotal = 0;
    public static bool ShowTutorial = true;

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
        if (ShowTutorial)
        {
            TutorialPanel.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!ShowTutorial)
        {
            Initialize();
            TutorialPanel.SetActive(false);
            Time.timeScale = 1;
        }

        NotesTotal = 0;
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
        if (!ShowTutorial)
        {
            if (NoteGenerationAudioSource.isPlaying && CheckForNote() && noteSpawnTimer >= NoteSpawnMinInterval && !EndGamePanel.activeSelf)
                SendNote();
            else if (!MusicAudioSource.isPlaying && Application.isFocused && !EndGamePanel.activeSelf) //End game if song is over and the game hasn't already ended.
                EndGame(true);
        }
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
            NotesTotal++;
        }
        noteSpawnTimer = 0;
    }

    public void EndGame(bool victory = true)
    {
        EndGamePanel.SetActive(true);
        MusicAudioSource.Stop();
        NoteGenerationAudioSource.Stop();

        angstStatScript angst = FindObjectOfType<angstStatScript>();
        metalStatScript metal = FindObjectOfType<metalStatScript>();
        fameStatScript fame = FindObjectOfType<fameStatScript>();
        moneyStatScript money = FindObjectOfType<moneyStatScript>();
        float metalGained = 0;
        float fameGained = 0;
        float moneyGained = 0;
        float angstGained = 0;

        FindObjectOfType<TimingString>().enabled = false;

        //Practice always goes to victory.
        if (victory)
        {
            MusicAudioSource.PlayOneShot(VictorySound);

            //Calculate rewards then apply them.
            metalGained = Mathf.CeilToInt(25 * (1 / (1 + (angst.getAmount() / 15))) * (TimingString.NotesHit / TimingSystem.ActivatedMechanicAndMissedNotesCounter));
            fameGained = Mathf.CeilToInt(50 * (2 / (10 - (metal.getAmount() / 15))));
            moneyGained = Mathf.CeilToInt(3000 * (6 / (100 - fame.getAmount())));
            angstGained = Mathf.CeilToInt(-25 * (TimingString.NotesHit / TimingSystem.ActivatedMechanicAndMissedNotesCounter));

            Debug.Log(NotesTotal + " TOTAL, " + TimingString.NotesHit + " HIT");
            MetalText.text = "Metal Gained: " + metalGained.ToString();
            AngstText.text = "Angst Loss: " + angstGained.ToString();

            metal.addOrRemoveAmount(metalGained);


            if (GigBackgroundManager.GigSession)
            {
                FameText.text = "Fame Gained: " + fameGained.ToString();
                MoneyText.text = "Money Gained: " + moneyGained.ToString();

                fame.addOrRemoveAmount(fameGained);
                money.addOrRemoveAmount(moneyGained);
                angst.addOrRemoveAmount(angstGained);

                if (fame.getAmount() >= fame.getMax())
                {
                    GameManager.ToEndGame = true;
                    GameManager.EndGameTitleText = "You're famous and won the game!";
                }
            }
        }
        else
        {
            MusicAudioSource.PlayOneShot(DefeatSound);
        }
    }

    public void SetTutorial(bool active)
    {
        ShowTutorial = active;

        if (active)
            Time.timeScale = 0;
        else if (!active)
        {
            Time.timeScale = 1;
            Initialize();
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
        ShowTutorial = true;
    }
}
