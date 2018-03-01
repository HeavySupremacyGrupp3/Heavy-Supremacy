using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TimingSystem : MonoBehaviour
{
    public float GracePeriod = 0.05f;
    public KeyCode ActivasionKey1;
    public KeyCode ActivasionKey2;
    public KeyCode ActivasionKey3;
    public float AllowedDistance;

    public bool CanComboKey;
    public bool CanExitCollider;

    private bool MechanicActive = false;
    private float TimeSinceGraceStarted = 0f;
    private bool IsInGracePeriod = false;
    private bool ComingOutOfGracePeriod = false;
    private bool HasNextNote = false;
    private bool NoteHasSecondChance = false;

    private NoteMeta NextNote = null;
    public static int ActivatedMechanicAndMissedNotesCounter = 0;

    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> hitTargets = new List<GameObject>(); //Shitty solution to simultanious "good and miss" appearances.    

    void Update()
    {
        if ((Input.GetKeyDown(ActivasionKey1) || Input.GetKeyDown(ActivasionKey2) || Input.GetKeyDown(ActivasionKey3)) && HasNextNote)
            StartGracePeriod();

        bool isNoteCleared = false;
        if (IsInGracePeriod)
        {
            if (Input.GetKey(ActivasionKey3))
                isNoteCleared = NextNote.IsNoteCleared(ActivasionKey3);
            if (Input.GetKey(ActivasionKey2))
                isNoteCleared = NextNote.IsNoteCleared(ActivasionKey2);
            if (Input.GetKey(ActivasionKey1))
                isNoteCleared = NextNote.IsNoteCleared(ActivasionKey1);

            if (isNoteCleared && NextNote.CorrectAmountOfKeystrokes())
            {
                SuccessfulKeyPress();
            }

            CheckGracePeriod();
        }
        else if (!isNoteCleared && ComingOutOfGracePeriod)
        {
            if (NoteHasSecondChance)
            {
                NoteHasSecondChance = false;
                ComingOutOfGracePeriod = false;
                return;
            }
            //FAIL!
            FailedKeyPress();
        }

        if (!HasNextNote && NoteGenerator.NoteQueue.Count > 0)
            GetNextNoteSet();

    }

    private void SuccessfulKeyPress()
    {
        if (NoteIsWithinAllowedAboveRange())
        {
            //Success!
            Debug.Log("SUCCESS!");
            SucceedTiming();
            GetNextNoteSet();
        }
        else
        {
            //Fail! Out of range
            Debug.Log("Out of range.");
            foreach (GameObject note in NextNote.GetNotes())
            {
                if (note.transform.position.y < transform.position.y)
                {
                    FailedKeyPress();
                    break;
                }
            }
            NoteHasSecondChance = true;
        }

        IsInGracePeriod = false;
    }

    private void FailedKeyPress()
    {
        Debug.Log("Massive fail!");
        FailTiming();
        GetNextNoteSet();
        ComingOutOfGracePeriod = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector2(100, AllowedDistance));
    }

    private void StartGracePeriod()
    {
        if (IsInGracePeriod)
            return;

        IsInGracePeriod = true;
        TimeSinceGraceStarted = 0f;
    }

    private void CheckGracePeriod()
    {
        TimeSinceGraceStarted += Time.deltaTime;
        if (TimeSinceGraceStarted >= GracePeriod)
        {
            IsInGracePeriod = false;
            ComingOutOfGracePeriod = true;
        }
    }
    private bool GetNextNoteSet()
    {
        if (NoteGenerator.NoteQueue.Count == 0)
        {
            HasNextNote = false;
            NextNote = null;
            return false;
        }

        HasNextNote = true;
        NextNote = NoteGenerator.NoteQueue.Dequeue();
        return true;
    }

    private bool NoteIsWithinAllowedAboveRange()
    {
        foreach (GameObject note in NextNote.GetNotes())
        {
            if (YDistanceToTarget(note) <= AllowedDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private float YDistanceToTarget(GameObject note)
    {
        Vector2 pos = transform.position;
        Vector2 notePos = note.transform.position;

        return Mathf.Abs(pos.y - notePos.y);
    }

    public virtual void FailTiming()
    {

    }

    public virtual void SucceedTiming()
    {
        DestroyNotes();
    }

    private void DestroyNotes()
    {
        foreach (GameObject note in NextNote.GetNotes())
        {
            Destroy(note);
        }
    }

}
