using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimingSystem : MonoBehaviour
{

    public KeyCode ActivasionKey1;
    public KeyCode ActivasionKey2;
    public KeyCode ActivasionKey3;

    protected bool TargetInRange = false;

    public static float FailedTimingCounter = 0;

    private List<string> keysPressed = new List<string>();
    private float graceTimer = 0;
    private float graceDuration = 0.25f;

    private void Start()
    {
        FailedTimingCounter = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(ActivasionKey1) || Input.GetKeyDown(ActivasionKey2) || Input.GetKeyDown(ActivasionKey3))
            ActivateMechanic();

        if (graceTimer >= 0)
        {
            graceTimer -= Time.deltaTime;
        }

        CheckNoteClearence();
    }

    void ActivateMechanic()
    {
        if (!TargetInRange)
        {
            FailTiming();
            return;
        }
        else if (TargetInRange)
        {
            if (graceTimer < 0)
                graceTimer = graceDuration;

            //Need multiple if-statements to handle simultanious notes.
            if (TargetInRange && Input.GetKeyDown(ActivasionKey1))
            {
                keysPressed.Add("_1");
            }
            if (TargetInRange && Input.GetKeyDown(ActivasionKey2))
            {
                keysPressed.Add("_2");
            }
            if (TargetInRange && Input.GetKeyDown(ActivasionKey3))
            {
                keysPressed.Add("_3");
            }

            return;
        }
    }

    private void CheckNoteClearence()
    {
        if (NoteGenerator.NoteSets != null && NoteGenerator.NotesTotal > 0)
        {
            if (NoteGenerator.NoteSets[0].CheckNotes(keysPressed) && graceTimer > 0)
            {
                foreach (GameObject note in NoteGenerator.NoteSets[0].Notes)
                {
                    SucceedTiming(note);
                }
                NoteGenerator.NoteSets.RemoveAt(0);
            }
        }
    }

    public virtual void FailTiming()
    {
        Debug.Log("FAILED TIMING!");
        FailedTimingCounter++;

        TargetInRange = false;

        keysPressed.Clear();
    }

    public virtual void SucceedTiming(GameObject note)
    {
        Debug.Log("SUCCEEDED TIMING!");
        keysPressed.Clear();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TargetInRange = true;
    }
}
