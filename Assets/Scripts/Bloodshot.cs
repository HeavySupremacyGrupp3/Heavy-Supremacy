using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodshot : MonoBehaviour
{
    [Range(0f, 1f)]
    public float Progress = 0f;
    public Color StartColor, EndColor;
    public int StartAmountOfParticles, EndAmountOfParticles;
    private ParticleSystem Particles;
    private ParticleSystem.MainModule ParticlesMainModule;
    private ParticleSystem.EmissionModule ParticlesEmissionModule;

    void Start()
    {
        Particles = GetComponent<ParticleSystem>();
        ParticlesMainModule = Particles.main;
        ParticlesEmissionModule = Particles.emission;
    }


    void Update ()
    {
        ParticlesMainModule.startColor = Color.Lerp(StartColor, EndColor, Progress);
        ParticlesEmissionModule.rateOverTime = StartAmountOfParticles + Progress * (EndAmountOfParticles - StartAmountOfParticles);
	}
}
