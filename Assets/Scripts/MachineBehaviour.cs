using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineBehaviour : MonoBehaviour {

    [SerializeField]
    private GameObject[] machinesToSpawn;
    //[SerializeField]
    //private List<GameObject> machines = new List<GameObject>();

    [SerializeField]
    private float speed;

    private GameObject machine1;
    private GameObject machine2;
    private GameObject machine3;

    private bool lerpMachine1 = false;
    private float lerpTimer1 = 0f;
    private float lerpTime = 2f;

    void Start()
    {
        //Instantiate three machines at their positions
        machine1 = Instantiate(machinesToSpawn[0], new Vector3(-0.2f, 0f), Quaternion.identity);
        machine2 = Instantiate(machinesToSpawn[1], new Vector3(0f, 0f), Quaternion.identity);
        machine3 = Instantiate(machinesToSpawn[2], new Vector3(0.2f, 0f), Quaternion.identity);
    }

    void Update()
    {
        MachineMovement();
        /*
        if(lerpMachine1)
        {
            Vector2 startPosition1 = machine1.transform.position;
            Vector2 endPosition1 = machine1.transform.position -= new Vector3(0f, 2f);

            if (lerpTimer1 <= 1f)
            {
                lerpTimer1 += Time.deltaTime / lerpTime;

                machine1.transform.position = Vector3.Lerp(startPosition1, endPosition1, lerpTime);
            }
            else
            {
                if(startPosition1 = machin
            }
        }
        */
    }

    private void MachineMovement()
    {
        /* if (Input.GetKeyDown("left") && machine1.transform.position.y = 0f)
        {
            Vector2 movement = new Vector2(0f, -0.2f * Time.deltaTime);
            machine1.transform.Translate(movement * speed);
        }

        if (Input.GetKeyDown("down") && machine2.transform.position.y = 0f)
        {
            Vector2 movement = new Vector2(0f, -0.2f * Time.deltaTime);
            machine2.transform.Translate(movement * speed);
        }

        if (Input.GetKeyDown("right") && machine3.transform.position.y = 0f)
        {
            Vector2 movement = new Vector2(0f, -0.2f * Time.deltaTime);
            machine3.transform.Translate(movement * speed);
        }

        if (machine1.transform.position.y = -1f)
        {
            Vector2 movement = new Vector2(0f, 0.2f * Time.deltaTime);
            machine1.transform.Translate(movement * speed);
        }*/

        if (Input.GetKeyDown("left") && machine1.transform.position.y == 0f)
        {
            if (!lerpMachine1)
                lerpMachine1 = true;
        }

       /* if (Input.GetKeyDown("down") && machine2.transform.position.y == 0f)
        {
            Vector3 startPosition2 = new Vector3(machine2.transform.position.x, machine2.transform.position.y);
            Vector3 endPosition2 = new Vector3(machine2.transform.position.x, -2F);

            machine2.transform.position = Vector3.Lerp(startPosition2, endPosition2, i);
        }

        if (Input.GetKeyDown("right") && machine3.transform.position.y == 0f)
        {
            Vector3 startPosition3 = new Vector3(machine3.transform.position.x, machine3.transform.position.y);
            Vector3 endPosition3 = new Vector3(machine3.transform.position.x, -2F);

            machine3.transform.position = Vector3.Lerp(startPosition3, endPosition3, i);
        }*/
           

        //journeyLength = Vector3.Distance(tartPosition1, endPosition1);

           
        
    }
}
