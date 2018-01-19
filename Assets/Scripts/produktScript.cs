using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class produktScript : MonoBehaviour {

	public delegate void mittEvent();
	public static event mittEvent earnMoney;
	
	void Update ()
	{
		transform.Translate(Vector3.right * 2f*Time.deltaTime);
	}
	
	void OnTriggerEnter2D(Collider2D other) //kollisioner
	{
		if(other.gameObject.tag=="Maskin")
		{
			Debug.Log("hej jag uppgraderades");
		}
		
		if(other.gameObject.tag=="Box")
		{
			earnMoney();
			Destroy(gameObject);
		}
	}
}
