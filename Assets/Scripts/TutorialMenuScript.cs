﻿using System.Collections;
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
		//NoteGenerator.showGigTutorial=true;
		NoteGenerator.ShowPracticeTutorial=true;
		SceneTransition.StartTransition("PracticeScene");
	}
	
	public void LoadGigTutorial()
	{
		//NoteGenerator.showPracticeTutorial=false;
		NoteGenerator.ShowGigTutorial=true;
		SceneTransition.StartTransition("PracticeScene");
	}
	
	public void LoadWork()
    {
		SceneTransition.StartTransition("WorkScene");
		Debug.Log("Loading work - regular");
    }
}