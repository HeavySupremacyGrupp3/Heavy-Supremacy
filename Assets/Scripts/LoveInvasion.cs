using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Bias
{
    [Range(-1, 1)]
    public float OriginalBias = 0f;
    [Range(-1, 1)]
    public float BiasIncrease = 0f;
    [Range(-1, 1)]
    public float MinimumBias = 0f;
    [Range(-1, 1)]
    public float MaximumBias = 0f;
}

public class LoveInvasion : MonoBehaviour
{
    [Header("Spawn Biases")]
    public Bias EasyBias;
    public Bias NormalBias;
    public Bias HardBias;
    public Bias ExtremeBias;

    [Header("Enemy Prefabs")]
    public GameObject EasyEnemy;
    public GameObject NormalEnemy;
    public GameObject HardEnemy;
    public GameObject ExtremeEnemy;

    [Header("Spawn Area")]
    public float SpawnY;
    public float SpawnXMin, SpawnXMax;

    [Header("Level Info")]
    public float Waves = 4;
    public int MaxWaves = 12;
    public float WaveIncreasePerLevel = 1.3334f;
    [Range(0, 1)]
    public float TwoWavesAtOnceChance = 0f;
    public float TwoWavesAtOnceMaxChance = 0.3f;
    [Range(0, 1)]
    public float ThreeWavesAtOnceChance = 0f;
    public float ThreeWavesAtOnceMaxChance = 0.2f;
    public float TwoWaveIncreasePerLevel = 1.05f;
    public float ThreeWaveIncreasePerLevel = 1.025f;
    public static int Level = 1;
    public float MinTimeBetweenWaves = 1.3f, MaxTimeBetweenWaves = 3.4f;
    private int CurrentWave = 0;
    public float WaitTimeAfterLevel = 3.3f;
    public Text LevelText, WaveText;
    public GameObject LosePanel;
    public static bool LostGame = false;
    private bool AlreadyLost = false;
    public AudioClip DefeatMusic;
    private AudioClip InitialMusic;
    public string[] HeartWords;

    private float InitialWaves;
    private float InitialEasyBias, InitialNormalBias, InitialHardBias, InitialExtremeBias;

    private bool IsPaused = true;

    private Queue<GameObject> EnemyQeue = new Queue<GameObject>();
    public static List<GameObject> SpawnedEnemies = new List<GameObject>();

    public Animator LevelPanel;

    private void Awake()
    {
        InitialWaves = Waves;
        InitialEasyBias = EasyBias.OriginalBias;
        InitialNormalBias = NormalBias.OriginalBias;
        InitialHardBias = HardBias.OriginalBias;
        InitialExtremeBias = ExtremeBias.OriginalBias;
        InitialMusic = AudioManager.instance.GetSound("LoveInvasion_Music").clip;
    }

    private void OnEnable()
    {
        LosePanel.SetActive(false);
        LostGame = false;
        AlreadyLost = false;
        ResetGame();
        AudioManager.instance.Stop("HUBMusic");
        AudioManager.instance.Play("LoveInvasion_Music");
    }

    private void OnDisable()
    {
        Defeat();
        AudioManager.instance.Stop("LoveInvasion_Music");
        AudioManager.instance.Play("HUBMusic");
    }

    private void ResetGame()
    {
        Level = 1;
        CurrentWave = 0;
        TwoWavesAtOnceChance = 0;
        ThreeWavesAtOnceChance = 0;
        StopAllCoroutines();

        Waves = InitialWaves;
        EasyBias.OriginalBias = InitialEasyBias;
        NormalBias.OriginalBias = InitialNormalBias;
        HardBias.OriginalBias = InitialHardBias;
        ExtremeBias.OriginalBias = InitialExtremeBias;

        AudioManager.instance.GetSource("LoveInvasion_Music").clip = InitialMusic;

        UpdateLevelTexts();
        EnableLevelPanel();
        GenerateWaves();
    }

    private void EnableLevelPanel()
    {
        LevelPanel.SetTrigger("Animate");
        StartCoroutine(PauseForSeconds(WaitTimeAfterLevel));
    }

    private void Update()
    {
        if (LostGame)
        {
            Defeat();
        }

        if (IsPaused)
            return;

        if (CurrentWave < (int)Waves) // Send wave
        {
            SendWave();
            StartCoroutine(PauseForSeconds(Random.Range(MinTimeBetweenWaves, MaxTimeBetweenWaves)));
        }
        else // Level is over
        {
            if (SpawnedEnemies.Count > 0) // Still enemies on the field
                return;
            
            IncreaseLevel();
        }
    }

    private void Defeat()
    {
        IsPaused = true;
        if (AlreadyLost)
            return;
        LosePanel.SetActive(true);
        EnemyQeue.Clear();

        foreach (GameObject enemy in SpawnedEnemies)
        {
            Destroy(enemy);
        }
        SpawnedEnemies.Clear();

        AudioSource src = AudioManager.instance.GetSource("LoveInvasion_Music");
        src.clip = DefeatMusic;
        src.Play();
        AlreadyLost = true;
    }

    private void SendWave()
    {
        float rnd = Random.Range(0f, 1f);
        if (rnd < ThreeWavesAtOnceChance + TwoWavesAtOnceChance)
        {
            if (CurrentWave + 2 <= (int)Waves)
            {
                Instantiate(EnemyQeue.Dequeue(), new Vector2(Random.Range(SpawnXMin, SpawnXMax), SpawnY), Quaternion.identity, transform);
                CurrentWave++;
            }
            if (CurrentWave + 3 <= (int)Waves && rnd < ThreeWavesAtOnceChance)
            {
                Instantiate(EnemyQeue.Dequeue(), new Vector2(Random.Range(SpawnXMin, SpawnXMax), SpawnY), Quaternion.identity, transform);
                CurrentWave++;
            }
        }
        Instantiate(EnemyQeue.Dequeue(), new Vector2(Random.Range(SpawnXMin, SpawnXMax), SpawnY), Quaternion.identity, transform);
        CurrentWave++;
    }

    private void IncreaseLevel()
    {
        Level++;
        CurrentWave = 0;
        Waves = Waves + WaveIncreasePerLevel > MaxWaves ? MaxWaves : Waves + WaveIncreasePerLevel;
        TwoWavesAtOnceChance = TwoWavesAtOnceChance + TwoWaveIncreasePerLevel > TwoWavesAtOnceMaxChance ? TwoWavesAtOnceMaxChance : TwoWavesAtOnceChance + TwoWaveIncreasePerLevel;
        ThreeWavesAtOnceChance = ThreeWavesAtOnceChance + ThreeWaveIncreasePerLevel > ThreeWavesAtOnceMaxChance ? ThreeWavesAtOnceMaxChance : ThreeWavesAtOnceChance + ThreeWaveIncreasePerLevel;
        UpdateBiases();
        UpdateLevelTexts();
        EnableLevelPanel();
        GenerateWaves();
    }

    private void UpdateLevelTexts()
    {
        string heartWord = HeartWords[Random.Range(0, HeartWords.Length)];

        LevelText.text = "LEVEL " + Level;
        WaveText.text = heartWord + ": " + (int)Waves;
    }

    private void GenerateWaves()
    {
        float total = EasyBias.OriginalBias + NormalBias.OriginalBias + HardBias.OriginalBias + ExtremeBias.OriginalBias;
        float realEBias = EasyBias.OriginalBias / total;
        float realNBias = NormalBias.OriginalBias / total;
        float realHBias = HardBias.OriginalBias / total;

        for (int i = 0; i < (int)Waves; i++)
        {
            float rand = Random.Range(0, 1f);

            if (rand < realEBias) // Easy
            {
                EnemyQeue.Enqueue(EasyEnemy);
            }
            else if (rand < realEBias + realNBias) // Normal
            {
                EnemyQeue.Enqueue(NormalEnemy);
            }
            else if (rand < realEBias + realNBias + realHBias) // Hard
            {
                EnemyQeue.Enqueue(HardEnemy);
            }
            else // Extreme
            {
                EnemyQeue.Enqueue(ExtremeEnemy);
            }
        }
    }

    private void UpdateBiases()
    {
        // Easy Bias
        float eBias = EasyBias.OriginalBias + EasyBias.BiasIncrease;
        if (eBias > EasyBias.MaximumBias)
            EasyBias.OriginalBias = EasyBias.MaximumBias;
        else if (eBias < EasyBias.MinimumBias)
            EasyBias.OriginalBias = EasyBias.MinimumBias;
        else
            EasyBias.OriginalBias = eBias;

        // Normal
        float nBias = NormalBias.OriginalBias + NormalBias.BiasIncrease;
        if (nBias > NormalBias.MaximumBias)
            NormalBias.OriginalBias = NormalBias.MaximumBias;
        else if (nBias < NormalBias.MinimumBias)
            NormalBias.OriginalBias = NormalBias.MinimumBias;
        else
            NormalBias.OriginalBias = nBias;

        // Hard
        float hBias = HardBias.OriginalBias + HardBias.BiasIncrease;
        if (hBias > HardBias.MaximumBias)
            HardBias.OriginalBias = HardBias.MaximumBias;
        else if (hBias < HardBias.MinimumBias)
            HardBias.OriginalBias = HardBias.MinimumBias;
        else
            HardBias.OriginalBias = hBias;

        // Extreme
        float exBias = ExtremeBias.OriginalBias + ExtremeBias.BiasIncrease;
        if (exBias > ExtremeBias.MaximumBias)
            ExtremeBias.OriginalBias = ExtremeBias.MaximumBias;
        else if (exBias < ExtremeBias.MinimumBias)
            ExtremeBias.OriginalBias = ExtremeBias.MinimumBias;
        else
            ExtremeBias.OriginalBias = exBias;
    }

    private IEnumerator PauseForSeconds(float time)
    {
        IsPaused = true;
        yield return new WaitForSeconds(time);
        IsPaused = false;
    }
}
