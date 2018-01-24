using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barScript : MonoBehaviour {

	private Slider progressSlider;
	
	[SerializeField]
	private Stats InitialReference;
	Stats StatReference;
	
	float amount;

	void Start()
	{
		progressSlider=GetComponent<Slider>();
	}
	
	void Update ()
	{		
		Stats[] arr = FindObjectsOfType<Stats>();
		
		for(int i=0;i<arr.Length;i++)
		{
			if(arr[i].getStat()==InitialReference.getStat())
			{
				StatReference=arr[i];
				amount=StatReference.getAmount();
				progressSlider.value=amount;
				Debug.Log(arr[i].getStat());
				//Debug.Log(progressSlider.value); // nollställs hela tiden av okänd anlednin							
			}
		}
	}
}
