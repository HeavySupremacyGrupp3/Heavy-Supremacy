using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimingSystem : MonoBehaviour
{
    public float GracePeriod = 0.2f;
    public KeyCode ActivasionKey1;
    public KeyCode ActivasionKey2;
    public KeyCode ActivasionKey3;

    public bool CanComboKey;
    public bool CanExitCollider;

    private bool MechanicActive = false;
    private float TimeSinceGraceStarted = 0f;

    private NoteMeta NextNote = null;

    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> hitTargets = new List<GameObject>(); //Shitty solution to simultanious "good and miss" appearances.

    public static float ActivatedMechanicAndMissedNotesCounter = 0;

    private void Start()
    {
        ActivatedMechanicAndMissedNotesCounter = 0;
    }

    void Update()
    {
        if (NextNote == null)
        {
            if (NoteGenerator.NoteQueue.Count <= 0)
                return;

            NextNote = NoteGenerator.NoteQueue.Dequeue();
        }

        if (Input.GetKeyDown(ActivasionKey1) || Input.GetKeyDown(ActivasionKey2) || Input.GetKeyDown(ActivasionKey3) || MechanicActive)
            ActivateMechanic();
    }

    void ActivateMechanic()
    {
        if (!MechanicActive) // New key-pressing sequence has started
        {
            MechanicActive = true;
            TimeSinceGraceStarted = 0f;
        }
        else if (TimeSinceGraceStarted >= GracePeriod) // Time has passed for a new key to be accepted, player failed.
        {
            FailTiming();
            MechanicActive = false;
        }
        else
        {
            TimeSinceGraceStarted += Time.deltaTime;
        }

        if (targets.Count == 0)
        {
            FailTiming();
            return;
        }

        bool noteIsCleared = false;
        if (Input.GetKey(ActivasionKey1))
            noteIsCleared = NextNote.IsNoteCleared(ActivasionKey1);
        if (Input.GetKey(ActivasionKey2))
            noteIsCleared = NextNote.IsNoteCleared(ActivasionKey2);
        if (Input.GetKey(ActivasionKey3))
            noteIsCleared = NextNote.IsNoteCleared(ActivasionKey3);

        if (noteIsCleared)
        {
            SucceedTiming();
            NextNote.DestroyNotes();
            if (NoteGenerator.NoteQueue.Count <= 0)
            {
                NextNote = null;
                return;
            }

            NextNote = NoteGenerator.NoteQueue.Dequeue();
        }
    }

    public virtual void FailTiming()
    {
        ActivatedMechanicAndMissedNotesCounter++;

        if (targets.Count > 0)
        {
            targets.RemoveAt(0);
            if (NoteGenerator.NoteQueue.Count <= 0)
            {
                NextNote = null;
                return;
            }

            NextNote = NoteGenerator.NoteQueue.Dequeue();
        }
    }

    public virtual void SucceedTiming()
    {
        ActivatedMechanicAndMissedNotesCounter++;

        hitTargets.Add(targets[0]);
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.tag == "TimingObject" && !targets.Contains(collision.gameObject))
    //    {
    //        targets.Add(collision.gameObject);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D coll) // Ändrade från OnTriggerStay2D
    {
        if (coll.tag == "TimingObject" && !targets.Contains(coll.gameObject))
        {
            targets.Add(coll.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "TimingObject" && targets.Contains(coll.gameObject))
        {
            targets.Remove(coll.gameObject);
        }
    }
}
