using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingMachine : MonoBehaviour {
	
	public Sprite spoilSprite;
	private int myType;
	
	//public string SecondSound;
	
	
	void Start()
	{
		myType=GetComponent<MachineProperties>().type;
	}
	
	private void OnTriggerEnter2D(Collider2D collision) //OnTriggerEnter2D nummer 2
	{
		produktScript sc=collision.GetComponent<produktScript>();
		
		if(myType<sc.type) // spoils the product if it is NOT in the correct stage
		{
			sc.spoil();
			sc.GetComponent<SpriteRenderer>().sprite = spoilSprite;
			AudioManager.instance.Play("spoilLjud");
		}
		
		if(myType==sc.type && !sc.Spoiled) // transform the products if it's in the correct stage
		{
			AudioManager.instance.Play("Machine"+myType+"Hit");
			sc.type++;
			collision.GetComponent<SpriteRenderer>().sprite=sc.Sprites[sc.type];
		}
		
		//AudioManager.instance.Play(SecondSound); //plays if it hits something
	}
}
