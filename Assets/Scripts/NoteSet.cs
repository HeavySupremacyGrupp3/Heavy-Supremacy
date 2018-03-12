using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSet : MonoBehaviour {

    public List<GameObject> Notes = new List<GameObject>();
    public bool Succeeded = false;

    public bool CheckNotes(List<string> activasionKey)
    {
        if (activasionKey.Count != Notes.Count)
            return Succeeded;

        bool correctKey = false;
        for (int i = 0; i < Notes.Count; i++)
        {
            for (int j = 0; j < activasionKey.Count; j++)
            {
                if (Notes[i] != null && Notes[i].name.Contains(activasionKey[j]))
                {
                    correctKey = true;
                }
                else
                    correctKey = false;
            }
        }

        Succeeded = correctKey;

        return Succeeded;
    }
}
