using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class produktScript : MonoBehaviour {

	public delegate void mittEvent();
	public static event mittEvent earnMoney;
	
	public Sprite[] Sprites;
	public Sprite spoilSprite;
	public int currentStage=0;
	public bool Spoiled=false;
	
	void Update ()
	{
		transform.Translate(Vector3.right * 3f*Time.deltaTime);
	}
	
	void OnTriggerEnter2D(Collider2D other) //kollisioner
	{
		if(other.gameObject.tag=="Box")
		{
			if(!Spoiled)
				earnMoney();
			
			Destroy(gameObject);
		}
	}
	
	public void spoil()
	{
		GetComponent<SpriteRenderer>().sprite=spoilSprite;
		Spoiled=true;
	}
}
