using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static bool paused = false;
    public GameObject pauseMenu;
	

	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == false)
                Pause();
        }
	}

    public void Pause()
    {
        if (FindObjectOfType<NoteGenerator>())
            FindObjectOfType<NoteGenerator>().ToggleMusic(false);

        pauseMenu.SetActive(true);
        paused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        paused = false;
        Time.timeScale = 1f;
    }
}

