using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barScript : MonoBehaviour {

	private Slider progressSlider;
	public int StatIndex;
	Stats StatReference;
	
	float amount;

	void Start()
	{

		
		progressSlider=GetComponent<Slider>();
	}
	
	void Update ()
	{		
		//Stats[] arr = FindObjectsOfType<Stats>();
		Stats[] arr = FindObjectsOfType<Stats>();
		for(int i = 0; i < arr.Length; i++)
		{
			if(StatIndex == arr[i].getStat())
				StatReference = arr[i];
		}

			//if(arr[i].getStat()==InitialReference.getStat())
				//StatReference=InitialReference;
				amount=StatReference.getAmount();
				progressSlider.value=amount;
				
				Debug.Log(amount);
				//Debug.Log("amount: "+arr[i].getAmount()+" stat: "+arr[i].getStat());
				//Debug.Log();
				//Debug.Log(arr[i].getStat());
				//Debug.Log(progressSlider.value); // nollställs hela tiden av okänd anlednin	
				//Debug.Log("getAmount "+InitialReference.getAmount());						
			
		
	}
}
