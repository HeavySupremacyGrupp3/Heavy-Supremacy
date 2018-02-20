using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class TutorialMenuScript : MonoBehaviour {

	public SceneTransitionScript SceneTransition;

	public void LoadHUB()
    {
		SceneTransition.StartTransition("HUBScene");
		//SceneManager.LoadScene("HUBScene");
    }
	
	public void LoadWorkTutorial()
	{
		SceneTransition.StartTransition("WorkTutorialScene");
		Debug.Log("Loading work - tutorial");
	}
	
	public void LoadPracticeTutorial()
	{
	}
	
	public void LoadWork()
    {
		SceneTransition.StartTransition("WorkScene");
    }
}
