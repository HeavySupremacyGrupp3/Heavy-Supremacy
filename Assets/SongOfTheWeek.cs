using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongOfTheWeek : MonoBehaviour
{
    public Text PlayPauseText;
    public Sprite[] PlayPauseImages;
    public Image PlayPauseIcon;

    private const string PLAY_TEXT = "PLAY";
    private const string STOP_TEXT = "STOP";

    private Text SongNameText;
    private bool isPlayingSong = false;
    private AudioSource source;
    private AudioClip originalAudioClip;

	void Start ()
    {
        SongNameText = GetComponent<Text>();
        source = AudioManager.instance.GetSource("HUBMusic");
        originalAudioClip = source.clip;
    }
	
	// Update is called once per frame
	void Update ()
    {
        SongNameText.text = GameManager.GigSongs[GameManager.SongIndex].SongName;
	}

    public void TogglePlaySong()
    {
        if (isPlayingSong)
        {
            source.clip = originalAudioClip;
            source.Play();
            PlayPauseText.text = PLAY_TEXT;
            PlayPauseIcon.sprite = PlayPauseImages[0];
        }
        else
        {
            source.clip = GameManager.GigSongs[GameManager.SongIndex].MusicWithLead;
            source.time = 0f;
            source.Play();
            PlayPauseText.text = STOP_TEXT;
            PlayPauseIcon.sprite = PlayPauseImages[1];
        }
        isPlayingSong = !isPlayingSong;
    }
}
