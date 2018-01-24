using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barScript : MonoBehaviour {

	public Slider progressSlider;
	
	public Stats InitialReference;
	Stats StatReference;
	
	float amount;
	
	void Start ()
	{
		StatReference=InitialReference;
	}

	void Update ()
	{
		//StatReference=FindObjectOfType<happinessStatScript>();
		//StatReference=null;
		
		Stats[] arr = FindObjectsOfType<Stats>();
		
		for(int i=0;i<arr.Length;i++)
		{
			if(arr[i].getStat()==InitialReference.getStat())
			{
				StatReference=arr[i];
			}
		}
		amount=StatReference.getAmount();		
		progressSlider.value=amount;
	}
}
