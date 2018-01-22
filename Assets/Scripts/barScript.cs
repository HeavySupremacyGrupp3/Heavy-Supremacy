using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barScript : MonoBehaviour {

	public Slider progressSlider;
	
	float amount;

	void Update ()
	{
		progressSlider.value=amount;
	}
}
