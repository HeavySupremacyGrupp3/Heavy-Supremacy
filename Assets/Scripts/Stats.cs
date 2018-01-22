
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Stats : MonoBehaviour {

	protected float amount;
	
	//public GameObject SliderObject;
	//public Slider progressSlider;

	void Start ()
	{
		DontDestroyOnLoad(gameObject);
		//SliderObject=GameObject.Find("energySlider");
		//progressSlider=SliderObject.GetComponent<Slider>();
		//amount=25.0f;
	}
		
	void Update ()
	{
		//amount+=0.01f;
		//amount=amount%1.0f;
	}	

	public void setAmount(float a)
	{
		amount=a;
	}
	
	public void addOrRemoveAmount(float a)
	{
		amount+=a;
	}

	public float getAmount()
	{
		return amount;
	}
}

