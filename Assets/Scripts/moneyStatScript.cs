using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneyStatScript : Stats {
	
	
	public float difference;

    public ParticleSystem MoneyParticles;
    public static ParticleSystem _MoneyParticles;

	public override void initialize()
	{
		thisStat=4;

        _MoneyParticles = MoneyParticles;
	}


	void OnEnable() //events
	{
		produktScript.earnMoney +=addMoney;
	}
	
	void OnDisable() //events
	{
		produktScript.earnMoney -=addMoney;
	}	

	public void addMoney()
	{
		addOrRemoveAmount(difference);
		//amount += 50f;
	}

    public override void addOrRemoveAmount(float a)
    {
        base.addOrRemoveAmount(a);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "HUBScene")
        {
            if (a < 0)
                a *= -1;
            _MoneyParticles.emissionRate = a;
            _MoneyParticles.Play();
        }
    }
}
