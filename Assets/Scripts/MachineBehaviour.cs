using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineBehaviour : MonoBehaviour {

    [SerializeField]
    private GameObject[] machinesToSpawn;
    //[SerializeField]
    //private List<GameObject> machines = new List<GameObject>();

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
    public GameObject leftButton;
    public GameObject downButton;
    public GameObject rightButton;

    public Sprite leftSprite;
    public Sprite downSprite;
    public Sprite rightSprite;
    private SpriteRenderer sr;

    private Vector2 startPosition1;
    private Vector2 endPosition1;

    private Vector2 startPosition2;
    private Vector2 endPosition2;

    private Vector2 startPosition3;
    private Vector2 endPosition3;

    private bool reverseLerp1 = false;
    private bool lerpMachine1 = false;
    private float lerpTimer1 = 0f;

    private bool reverseLerp2 = false;
    private bool lerpMachine2 = false;
    private float lerpTimer2 = 0f;

    private bool reverseLerp3 = false;
    private bool lerpMachine3 = false;
    private float lerpTimer3 = 0f;

    [Range(0, 10)]
    public int spacing;

    public float buttonTime;

    [SerializeField]
    private float lerpTime = 2f;

    //2.35

    void Start()
    {
        //Instantiate three machines at their positions
        machine1 = Instantiate(machinesToSpawn[0], new Vector3(-3.5f - spacing, 5.8f), Quaternion.identity);
        machine2 = Instantiate(machinesToSpawn[1], new Vector3(0.1f, 5.8f), Quaternion.identity);
        machine3 = Instantiate(machinesToSpawn[2], new Vector3(3.5f + spacing, 5.8f), Quaternion.identity);

        machine1.GetComponent<MachineProperties>().machineType = 0;
        machine2.GetComponent<MachineProperties>().machineType = 1;
        machine3.GetComponent<MachineProperties>().machineType = 2;


        //Set start and end position that will lerp
        startPosition1 = machine1.transform.position;
        endPosition1 = new Vector3(machine1.transform.position.x, 2.5f);

        startPosition2 = machine2.transform.position;
        endPosition2 = new Vector3(machine2.transform.position.x, 2.5f);

        startPosition3 = machine3.transform.position;
        endPosition3 = new Vector3(machine3.transform.position.x, 2.5f);

        audioSources = GetComponents<AudioSource>();
    }

    void Update()
    {
        MachineMovement();
        
        //When called in MachineMovement, start the lerps
        if(lerpMachine1)
        {

            if (lerpTimer1 <= 1f && !reverseLerp1)
            {
                lerpTimer1 += Time.deltaTime / lerpTime;

                machine1.transform.position = Vector3.Lerp(startPosition1, endPosition1, lerpTimer1);

                if (lerpTimer1 >= 1)
                    reverseLerp1 = true;
            }
            else
            { 
                Debug.Log("Up");
                lerpTimer1 -= Time.deltaTime / lerpTime;

                if (lerpTimer1 <= 0)
                    lerpMachine1 = false;

                machine1.transform.position = Vector3.Lerp(startPosition1, endPosition1, lerpTimer1);

                if (lerpTimer1 <= 0)
                    reverseLerp1 = false;
            }
        }

        if (lerpMachine2)
        {

            if (lerpTimer2 <= 1f && !reverseLerp2)
            {
                lerpTimer2 += Time.deltaTime / lerpTime;

                machine2.transform.position = Vector3.Lerp(startPosition2, endPosition2, lerpTimer2);

                if (lerpTimer2 >= 1)
                    reverseLerp2 = true;
            }
            else
            {
                Debug.Log("Up");
                lerpTimer2 -= Time.deltaTime / lerpTime;

                if (lerpTimer2 <= 0)
                    lerpMachine2 = false;

                machine2.transform.position = Vector3.Lerp(startPosition2, endPosition2, lerpTimer2);

                if (lerpTimer2 <= 0)
                    reverseLerp2 = false;
            }
        }

        if (lerpMachine3)
        {

            if (lerpTimer3 <= 1f && !reverseLerp3)
            {
                lerpTimer3 += Time.deltaTime / lerpTime;

                machine3.transform.position = Vector3.Lerp(startPosition3, endPosition3, lerpTimer3);

                if (lerpTimer3 >= 1)
                    reverseLerp3 = true;
            }
            else
            {
                Debug.Log("Up");
                lerpTimer3 -= Time.deltaTime / lerpTime;

                if (lerpTimer3 <= 0)
                    lerpMachine3 = false;

                machine3.transform.position = Vector3.Lerp(startPosition3, endPosition3, lerpTimer3);

                if (lerpTimer3 <= 0)
                    reverseLerp3 = false;
            }
        }
    }

    private void MachineMovement()
    {
 
        //  Älskar dig <3

        if (Input.GetKeyDown(KeyCode.LeftArrow) && lerpTimer1 <= 0)
            lerpMachine1 = OnMachineMove(leftButton, leftSprite, audioSources[0], lerpMachine1, 0);

        if (Input.GetKeyDown("down") && lerpTimer2 <= 0)
            lerpMachine2 = OnMachineMove(downButton, downSprite, audioSources[2], lerpMachine2, 1);

        if (Input.GetKeyDown("right") && lerpTimer3 <= 0)
            lerpMachine3 = OnMachineMove(rightButton, rightSprite, audioSources[0], lerpMachine3, 2);
    }

    private bool OnMachineMove(GameObject button, Sprite sprite, AudioSource audiosource, bool lerpMachine, int index)
    {
        SpriteRenderer sr = button.GetComponent<SpriteRenderer>();
        Sprite oldSprite = sr.sprite;
        sr.sprite = sprite;

        audiosource = audioSources[index];
        audioSources[index].Play();
        StartCoroutine(ButtonTimer(sr, oldSprite));

        if (!lerpMachine)
            return true;
        else
            return false;
    }

    private IEnumerator ButtonTimer(SpriteRenderer sr, Sprite buttonSprite)
    {
        yield return new WaitForSeconds(buttonTime);
        sr.sprite = buttonSprite;
    }

    public void PlayAudio()
    {
        
    }
}
