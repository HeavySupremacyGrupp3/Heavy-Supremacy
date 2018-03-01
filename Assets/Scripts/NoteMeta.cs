using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteMeta
{
    private Dictionary<KeyCode, GameObject> notes;
    private List<GameObject> noteObjects;
    private int activations = 0;

    /// <summary>
    /// Create a note meta object
    /// </summary>
    /// <param name="_activationKeys">An ordered array with keys that corresponds to the given notes</param>
    /// <param name="_notes">An ordered array of notes</param>
    public NoteMeta(KeyCode[] _activationKeys, GameObject[] _notes)
    {
        if (_activationKeys.Length != _notes.Length)
        {
            throw new ArgumentException("Arrays must be of equal length to create a dictionary.");
        }

        notes = new Dictionary<KeyCode, GameObject>();
        noteObjects = new List<GameObject>();
        for (int i = 0; i < _activationKeys.Length; i++)
        {
            notes.Add(_activationKeys[i], _notes[i]);
            noteObjects.Add(_notes[i]);
        }
    }

    public bool IsNoteCleared(KeyCode key)
    {
        activations++;

        if (notes.ContainsKey(key))
            notes.Remove(key);

        if (notes.Count == 0)
            return true;

        return false;
    }

    public bool CorrectAmountOfKeystrokes()
    {
        if (activations == noteObjects.Count)
            return true;

        return false;
    }

    public IEnumerable<GameObject> GetNotes()
    {
        return noteObjects;
    }
}
