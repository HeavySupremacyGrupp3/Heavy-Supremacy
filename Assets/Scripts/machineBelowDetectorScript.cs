using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineBelowDetectorScript : MonoBehaviour {
	
	public delegate void targetDetector(GameObject t);
	public event targetDetector productBelowDetected;
	
	public bool isActive;
	
	void Update()
	{
		//if(isActive)
			//Debug.Log("I am active");
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(isActive)
		{
			//Debug.Log("Product below");
			productBelowDetected(collision.gameObject);
		}
	}
	
	public void ToggleActive()
	{
		isActive=!isActive;
	}
}
