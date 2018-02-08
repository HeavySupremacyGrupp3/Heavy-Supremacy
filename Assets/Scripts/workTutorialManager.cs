using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class workTutorialManager : MonoBehaviour {


	public void showText()
	{
		
	}
	
	public void ToggleGameObject(GameObject target)
    {
		Debug.Log("AAAAAAAAARGHHH!");
        target.SetActive(!target.active);
    }
	
	void OnEnable()
	{
		tutProduktScript.unhideText+=ToggleGameObject;
	}
	
	void OnDisable()
	{
		tutProduktScript.unhideText+=ToggleGameObject;
	}
}
