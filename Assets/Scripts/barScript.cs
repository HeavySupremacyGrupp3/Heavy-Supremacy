using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barScript : MonoBehaviour {

	public Slider progressSlider;

	void Update ()
	{
		progressSlider.value=amount;
	}
}
