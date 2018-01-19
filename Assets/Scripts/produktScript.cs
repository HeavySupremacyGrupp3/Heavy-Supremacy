using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class produktScript : MonoBehaviour {

	public delegate void mittEvent();
	public static event mittEvent earnMoney;
	
	void Update ()
	{
		transform.Translate(Vector3.right * 1f*Time.deltaTime);
	}
	
	void OnTriggerEnter2D(Collider2D other) //kollisioner
	{
		earnMoney();
		Debug.Log("hej jag krockade");
	}
}
