using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneyStatScript : Stats
{


    public float difference;

    public ParticleSystem MoneyRecievedParticles;
    public ParticleSystem MoneySpentParticles;
    public static ParticleSystem _MoneyRecieved;
    public static ParticleSystem _MoneySpent;

    public override void initialize()
    {
        thisStat = 4;

        _MoneyRecieved = MoneyRecievedParticles;
        _MoneySpent = MoneySpentParticles;
    }


    void OnEnable() //events
    {
        produktScript.earnMoney += addMoney;
    }

    void OnDisable() //events
    {
        produktScript.earnMoney -= addMoney;
    }

    public void addMoney()
    {
        addOrRemoveAmount(difference);
        //amount += 50f;
    }

    public override void addOrRemoveAmount(float a)
    {
        base.addOrRemoveAmount(a);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == GameManager.HUBScene && a != 0)
        {
            if (a < 0)
            {
                a *= -1;
                _MoneySpent.emissionRate = a / 8;
                _MoneySpent.Play();
            }
            else
            {
                _MoneyRecieved.emissionRate = a / 8;
                _MoneyRecieved.Play();
            }
        }
    }
}
