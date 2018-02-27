using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class NoteGenerator : MonoBehaviour
{
    public int SongIndex = 0;

    public AudioClip[] PracticeWithLeadSongs;
    public AudioClip[] PracticeWithoutLeadSongs;
    public AudioClip[] PracticeMIDISongs;

    public AudioClip[] GigWithLeadSongs;
    public AudioClip[] GigWithoutLeadSongs;
    public AudioClip[] GigMIDISongs;

    private AudioClip MusicWithLead;
    private AudioClip MusicWithoutLead;
    private AudioClip NoteGenerationAudio;
    public AudioSource MusicWithLeadAudioSource;
    public AudioSource MusicWithoutLeadAudioSource;
    public AudioSource NoteGenerationAudioSource;

    public AudioMixerGroup PracticeWithLeadMixer;
    public AudioMixerGroup PracticeWithoutLeadMixer;
    public AudioMixerGroup GigWithLeadMixer;
    public AudioMixerGroup GigWithoutLeadMixer;

    public float UpdateInterval = 0.1f; //For optimizing performance.
    private int SampleDataLength = 1024;  //1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
    public float MusicStartDelay = 1;
    public float NoteGenerationStartDelay = 1;

    public GameObject[] NotePrefabs;
    public float[] NoteSpawnXOffset;

    public float NoteSpawnMinInterval = 0.1f;
    public GameObject EndGamePanel;
    public GameObject PracticeTutorialPanel;
    public GameObject GigTutorialPanel;
    public Text FameText;
    public Text MoneyText;
    public Text AngstText;
    public Text MetalText;
    public Slider ProgressionSlider;

    public static int NoteMultiplier = 1;
    public static int NumberOfUniqueNotes = 2;
    public static float NotesTotal = 0;
    public static bool ShowPracticeTutorial = true;
    public static bool ShowGigTutorial = true;

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

    private bool useLeadAudioSource = true;
    private bool lerpAudio = false;
    private float lerpTimer = 0;
    [SerializeField]
    private float lerpSeconds = 0.25f;

    void Start()
    {
        if (ShowPracticeTutorial || ShowGigTutorial)
        {
            SetTutorial(true);
        }
        else if (!ShowPracticeTutorial || !ShowGigTutorial)
        {
            SetTutorial(false);
        }

    }

    void Initialize()
    {
        Debug.Log("INITIALIZE");

        NotesTotal = 0;
        clipSampleData = new float[SampleDataLength];

        //Assign audioclips.
        if (!GigBackgroundManager.GigSession)
        {
            MusicWithLead = PracticeWithLeadSongs[SongIndex];
            MusicWithoutLead = PracticeWithoutLeadSongs[SongIndex];
            NoteGenerationAudio = PracticeMIDISongs[SongIndex];

            MusicWithLeadAudioSource.outputAudioMixerGroup = PracticeWithLeadMixer;
            MusicWithoutLeadAudioSource.outputAudioMixerGroup = PracticeWithoutLeadMixer;
        }
        else if (GigBackgroundManager.GigSession)
        {
            MusicWithLead = GigWithLeadSongs[SongIndex];
            MusicWithoutLead = GigWithoutLeadSongs[SongIndex];
            NoteGenerationAudio = GigMIDISongs[SongIndex];

            MusicWithLeadAudioSource.outputAudioMixerGroup = GigWithLeadMixer;
            MusicWithoutLeadAudioSource.outputAudioMixerGroup = GigWithoutLeadMixer;
        }

        //Play the music.
        MusicWithLeadAudioSource.clip = MusicWithLead;
        MusicWithLeadAudioSource.PlayDelayed(MusicStartDelay);

        MusicWithoutLeadAudioSource.clip = MusicWithoutLead;
        MusicWithoutLeadAudioSource.PlayDelayed(MusicStartDelay);

        NoteGenerationAudioSource.clip = NoteGenerationAudio;
        NoteGenerationAudioSource.PlayDelayed(NoteGenerationStartDelay);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            MusicWithLeadAudioSource.time = MusicWithLeadAudioSource.clip.length - 2;
            MusicWithoutLeadAudioSource.time = MusicWithoutLeadAudioSource.clip.length - 2;
            NoteGenerationAudioSource.time = NoteGenerationAudioSource.clip.length - 2;
        }

        if ((!ShowPracticeTutorial && !GigBackgroundManager.GigSession) || !ShowGigTutorial)
        {
            if (NoteGenerationAudioSource.isPlaying && CheckForNote() && noteSpawnTimer >= NoteSpawnMinInterval && !EndGamePanel.activeSelf)
                SendNote();
            else if (!MusicWithLeadAudioSource.isPlaying && Application.isFocused && !EndGamePanel.activeSelf && !PauseMenu.paused) //End game if song is over and the game hasn't already ended.
                EndGame(true);

            if (lerpAudio)
            {
                LerpAudioSourceVolume();
            }
        }

        if (MusicWithLeadAudioSource.clip != null)
            ProgressionSlider.value = MusicWithLeadAudioSource.time / MusicWithLeadAudioSource.clip.length;
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
            //Commented lines are to ensure unique notes every send.
            //do
            //{
            tempIndex = Random.Range(0, NumberOfUniqueNotes);
            //}
            //while (noteIndex == tempIndex);
            noteIndex = tempIndex;

            Instantiate(NotePrefabs[noteIndex], new Vector2(transform.position.x + NoteSpawnXOffset[noteIndex], transform.position.y), Quaternion.identity);
            NotesTotal++;
        }
        noteSpawnTimer = 0;
    }

    public void EndGame(bool victory = true)
    {
        Debug.Log("ENDED GAME");

        EndGamePanel.SetActive(true);
        FindObjectOfType<TimingString>().enabled = false;

        MusicWithLeadAudioSource.Stop();
        MusicWithoutLeadAudioSource.Stop();
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
            Debug.Log("VICTORY");

            MusicWithLeadAudioSource.PlayOneShot(VictorySound);

            //Calculate rewards then apply them.
            metalGained = Mathf.CeilToInt(30 * (1 / (1 + (angst.getAmount() / 20))) * (TimingString.NotesHit / TimingSystem.ActivatedMechanicAndMissedNotesCounter));
            fameGained = Mathf.CeilToInt(50 * (2 / (10 - (metal.getAmount() / 15))));
            moneyGained = Mathf.CeilToInt(3000 * (6 / (100 - fame.getAmount())));
            angstGained = Mathf.CeilToInt(-25 * (TimingString.NotesHit / TimingSystem.ActivatedMechanicAndMissedNotesCounter));

            if (moneyGained > money.getMax() || moneyGained < 0)
                moneyGained = money.getMax();
            if (fameGained > fame.getMax() || fameGained < 0)
                fameGained = fame.getMax();
            if (metalGained > metal.getMax() || metalGained < 0)
                metalGained = metal.getMax();
            //if (angstGained > angst.getMax() || angstGained < 0)
            //    angstGained = angst.getMax();

            Debug.Log(NotesTotal + " TOTAL, " + TimingString.NotesHit + " HIT");
            MetalText.text = "" + metalGained.ToString();
            AngstText.text = "" + angstGained.ToString();

            metal.addOrRemoveAmount(metalGained);
            angst.addOrRemoveAmount(angstGained);

            if (GigBackgroundManager.GigSession)
            {
                FameText.text = "" + fameGained.ToString();
                MoneyText.text = "" + moneyGained.ToString();

                fame.addOrRemoveAmount(fameGained);
                money.addOrRemoveAmount(moneyGained);

                if (fame.getAmount() >= fame.getMax())
                {
                    GameManager.ToEndGame = true;
                    GameManager.EndGameTitleText = "You're famous and won the game!";
                }
            }
        }
        else
        {
            Debug.Log("DEFEAT");

            MusicWithLeadAudioSource.PlayOneShot(DefeatSound);

            if (GigBackgroundManager.GigSession)
            {
                fame.addOrRemoveAmount(-10);
                metal.addOrRemoveAmount(-20);
                angst.addOrRemoveAmount(20);
                money.addOrRemoveAmount(150);
            }
        }
    }

    public void SwitchMusicSource(bool withLead)
    {
        lerpTimer = 0;

        //Lerp audio if there has been a switch.
        if (withLead != useLeadAudioSource)
            lerpAudio = true;

        if (withLead)
        {
            useLeadAudioSource = true;
        }
        else
        {
            useLeadAudioSource = false;
        }

    }

    public void LerpAudioSourceVolume()
    {
        lerpTimer += Time.deltaTime / lerpSeconds;

        if (useLeadAudioSource)
        {
            MusicWithLeadAudioSource.volume = Mathf.Lerp(0, 1, lerpTimer);
            MusicWithoutLeadAudioSource.volume = Mathf.Lerp(1, 0, lerpTimer);
        }
        else if (!useLeadAudioSource)
        {
            MusicWithLeadAudioSource.volume = Mathf.Lerp(1, 0, lerpTimer);
            MusicWithoutLeadAudioSource.volume = Mathf.Lerp(0, 1, lerpTimer);
        }

        if (lerpTimer >= 1)
            lerpAudio = false;
    }

    public void SetTutorial(bool active)
    {
        if (!GigBackgroundManager.GigSession)
        {
            ShowPracticeTutorial = active;
            PracticeTutorialPanel.SetActive(active);
        }
        if (GigBackgroundManager.GigSession)
        {
            ShowGigTutorial = active;
            GigTutorialPanel.SetActive(active);
        }

        ToggleMusic(!active);
    }

    public void ToggleMusic(bool resume)
    {
        if (!resume)
        {
            NoteGenerationAudioSource.Pause();
            MusicWithLeadAudioSource.Pause();
            MusicWithoutLeadAudioSource.Pause();

            FindObjectOfType<TimingString>().enabled = false;
            Time.timeScale = 0;
        }
        else if (resume)
        {
            NoteGenerationAudioSource.UnPause();
            MusicWithLeadAudioSource.UnPause();
            MusicWithoutLeadAudioSource.UnPause();

            FindObjectOfType<TimingString>().enabled = true;
            Time.timeScale = 1;

            if (!NoteGenerationAudioSource.isPlaying)
                Initialize();
        }
    }

    public void LoadHub()
    {
        Time.timeScale = 1;

        if (FindObjectOfType<GameManager>() != null)
            FindObjectOfType<GameManager>().LoadHUB();

        //If you start the game in the Practice-scene.
        else
            SceneManager.LoadScene(0);
    }

    public static void Reset()
    {
        NoteMultiplier = 1;
        NumberOfUniqueNotes = 2;
        ShowPracticeTutorial = true;
        ShowGigTutorial = true;
    }
}
