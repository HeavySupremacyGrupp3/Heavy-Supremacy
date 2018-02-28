using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodshot : MonoBehaviour
{
    [Range(0f, 1f)]
    public float Progress = 0f;
    public Color StartColor, EndColor;
    private ParticleSystem.MainModule Particles;
    
	void Start ()
    {
        Particles = GetComponent<ParticleSystem>().main;
	}
	
	void Update ()
    {
        Particles.startColor = Color.Lerp(StartColor, EndColor, Progress);
	}
}
