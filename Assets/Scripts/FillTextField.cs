using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FillTextField : MonoBehaviour
{
    public string TextKey;
	void Start ()
    {
        if (!TextManager.IsFileRead)
            throw new System.Exception("TextManager has not been initialized! TextManagerInit Monobehavior needs to be attached to a GameObject in the scene.");

        GetComponent<Text>().text = TextManager.GetText(TextKey);
    }
}
