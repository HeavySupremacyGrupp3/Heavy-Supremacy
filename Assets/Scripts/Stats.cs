
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Stats : MonoBehaviour {

	[SerializeField]
	protected float amount;	
	private float startAmount;

	[SerializeField]
	protected float max;
	static float min=0;
	
	protected int thisStat;

    [HideInInspector]
    public float LastWeeksStatGain = 0;

	//public GameObject SliderObject;
	//public Slider progressSlider;

	void Awake ()
	{
		 if (FindObjectsOfType<Stats>().Length > 5)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        startAmount = amount;

        //DontDestroyOnLoad(gameObject);
        initialize();
		//SliderObject=GameObject.Find("energySlider");
		//progressSlider=SliderObject.GetComponent<Slider>();
		//amount=25.0f;
	}
	
	public virtual void initialize()
	{
		
	}
		
	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.Q))
            addOrRemoveAmount(10);
		//amount+=0.01f;
		//amount=amount%1.0f;
	}	

	public void setAmount(float a)
	{
		amount=a;

        if (a > max)
            amount = max;
	}
	
	//amount can't be made to exceed max or min
	public void addOrRemoveAmount(float a)
	{
		if(amount+a<max && amount+a>min)
		{
			amount+=a;
		}	
		
		else if(amount+a>=max)
		{
			AudioManager.instance.Play("statmax");
			amount=max;
		}
		
		else if(amount+a<=min)
		{
			amount=min;
		}
	}

    public void ResetAmount()
    {
        amount = startAmount;
    }

	public float getAmount()
	{
		return amount;
	}
	
	public float getMin()
	{
		return min;
	}
	
	public float getMax()
	{
		return max;
	}
	
	public int getStat()
	{
		return thisStat;
	}

    public void UpdateWeeklyStatGains()
    {
        LastWeeksStatGain = LastWeeksStatGain - amount;
        Debug.Log("Last Week I Earned: " + LastWeeksStatGain);
    }
}

