using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineParticleTrigger : MonoBehaviour
{
    [SerializeField]
    private new ParticleSystem[] particleSystem;

    public void PlayParticles(float time = 0f)
    {
        foreach (ParticleSystem part in particleSystem)
        {
            part.Play();
        }
        if (time != 0f)
            Invoke("StopParticles", time);
    }

    public void StopParticles()
    {

        foreach (ParticleSystem part in particleSystem)
        {
            part.Stop();
        }
    }
}
