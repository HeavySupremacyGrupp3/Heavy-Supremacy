
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour {

	float amount;
	
	public Slider progressSlider;

	void Start ()
	{
		amount=0.0f;
	}
		
	void Update ()
	{
		amount+=0.01f;
		amount=amount%1.0f;
		showProgressBar(amount);
	}		

	public void setAmount(float a)
	{
		amount=a;
	}

	public float getAmount()
	{
		return amount;
	}

	void showProgressBar(float f)
	{
		progressSlider.value=amount;
	}
}

