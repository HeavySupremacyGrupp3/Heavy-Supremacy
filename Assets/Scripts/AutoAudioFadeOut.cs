using UnityEngine;

public class AutoAudioFadeOut : MonoBehaviour
{
    [Range(0f, 1f)]
    public float TimeInSongToStartFade = 0.9f;
    private float startVolume = 1f;
    private float volumeDecreasePerSecond = 0f;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        startVolume = source.volume;
        volumeDecreasePerSecond = startVolume / (source.clip.length - (TimeInSongToStartFade * source.clip.length));
    }

    private void Update()
    {
        if ((source.time >= TimeInSongToStartFade * source.clip.length) && source.volume > 0)
        {
            source.volume -= volumeDecreasePerSecond * Time.deltaTime;
        }

    }
}