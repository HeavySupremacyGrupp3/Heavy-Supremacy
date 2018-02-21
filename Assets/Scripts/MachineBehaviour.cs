using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineBehaviour : MonoBehaviour {

    [SerializeField]
    private GameObject[] machinesToSpawn;

    /*[SerializeField]
    private AudioClip armSFX1;
    [SerializeField]
    private AudioClip armSFX2;
    [SerializeField]
    private AudioClip armSFX3;*/

    private AudioSource[] audioSources;

    private GameObject machine1;
    private GameObject machine2;
    private GameObject machine3;

    [Range(0, 10)]
    public int spacing;


    public Transform checkpoint;

    void Start()
    {
        //Instantiate three machines at their positions
        machine1 = Instantiate(machinesToSpawn[0], new Vector3(-3.5f - spacing, 5.8f), Quaternion.identity);
        machine2 = Instantiate(machinesToSpawn[1], new Vector3(0.1f, 5.8f), Quaternion.identity);
        machine3 = Instantiate(machinesToSpawn[2], new Vector3(3.5f + spacing, 6.8f), Quaternion.identity);

        machine1.GetComponent<MachineProperties>().type = 0;
        machine2.GetComponent<MachineProperties>().type = 1;
        machine3.GetComponent<MachineProperties>().type = 2;

        audioSources = GetComponents<AudioSource>();
    }

    //  Älskar dig <3
}
