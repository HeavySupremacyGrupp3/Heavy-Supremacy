using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimingSystem : MonoBehaviour
{

    public KeyCode ActivasionKey1;
    public KeyCode ActivasionKey2;
    public KeyCode ActivasionKey3;

    public bool CanComboKey;
    public bool CanExitCollider;

    protected bool TargetInRange = false;

    public List<GameObject> targets = new List<GameObject>();
    //public List<GameObject> hitTargets = new List<GameObject>(); //Shitty solution to simultanious "good and miss" appearances.

    public static float ActivatedMechanicAndMissedNotesCounter = 0;

    private void Start()
    {
        ActivatedMechanicAndMissedNotesCounter = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(ActivasionKey1) || Input.GetKeyDown(ActivasionKey2) || Input.GetKeyDown(ActivasionKey3))
            ActivateMechanic();
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
            List<string> keysPressed = new List<string>();

            //Need multiple if-statements to handle simultanious notes.
            if (TargetInRange && Input.GetKey(ActivasionKey1))
            {
                keysPressed.Add("_1");
            }
            if (TargetInRange && Input.GetKey(ActivasionKey2))
            {
                keysPressed.Add("_2");
            }
            if (TargetInRange && Input.GetKey(ActivasionKey3))
            {
                keysPressed.Add("_3");
            }

            if (NoteGenerator.NoteSets[0].CheckNotes(keysPressed))
            {
                foreach (GameObject note in NoteGenerator.NoteSets[0].Notes)
                {
                    SucceedTiming(note);
                }
                NoteGenerator.NoteSets.RemoveAt(0);

            }
            return;
        }
    }

    public virtual void FailTiming()
    {
        Debug.Log("FAILED TIMING!");
        ActivatedMechanicAndMissedNotesCounter++;

        TargetInRange = false;
    }

    public virtual void SucceedTiming(GameObject note)
    {
        Debug.Log("SUCCEEDED TIMING!");
        ActivatedMechanicAndMissedNotesCounter++;


        //hitTargets.Add(targets[0]);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TargetInRange = true;

        //if (collision.tag == "TimingObject" && !targets.Contains(collision.gameObject))
        //{
        //    targets.Add(collision.gameObject);
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}
