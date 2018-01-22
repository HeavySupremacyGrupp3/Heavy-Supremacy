using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasScriptDontDestroyOnload : MonoBehaviour {

	void Start ()
	{
		if (FindObjectsOfType<canvasScriptDontDestroyOnload>().Length > 1)
            Destroy(FindObjectsOfType<canvasScriptDontDestroyOnload>()[0]);
        else
            DontDestroyOnLoad(gameObject);
	}
	

}
