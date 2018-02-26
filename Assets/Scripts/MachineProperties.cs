using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineProperties : MonoBehaviour {

    public int type = 0;

    [SerializeField]
    private GameObject button;

    [SerializeField]
    private Sprite buttonPressedSprite;

    private Vector2 startPosition;
    private Vector2 endPosition;

    private bool reverseLerp = false;
    private bool lerpMachine = false;
    private bool impact = false;
    private float lerpTimer = 0f;

    [SerializeField]
    private Animator animator;

    public float buttonTime = 0.5f;

    [SerializeField]
    private KeyCode key;

    [SerializeField]
    private float lerpTime = 0.5f;
    [SerializeField]
    private float lerpDistance;
	
	public delegate void musicEvent(string s);
	public static event musicEvent moveSound;
	
	public string FirstSound;

    void Start()
    {
        animator = GetComponent<Animator>();

        //Set start and end position that will lerp
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, transform.position.y - lerpDistance);
    }

    void Update()
    {
        MachineMovement();
        if (type > 1)
            animator.SetBool("impact", impact);

        //When called in MachineMovement, start the lerps
        if (lerpMachine)
        {

            if (lerpTimer <= 1f && !reverseLerp)
            {
                lerpTimer += Time.deltaTime / lerpTime;

                impact = true;
                transform.position = Vector3.Lerp(startPosition, endPosition, lerpTimer);

                if (lerpTimer >= 1)
                    reverseLerp = true;

            }
            else
            {
                lerpTimer -= Time.deltaTime / lerpTime;

                if (lerpTimer <= 0)
                {
                    lerpMachine = false;
                    impact = false;
                }


                transform.position = Vector3.Lerp(startPosition, endPosition, lerpTimer);

                if (lerpTimer <= 0)
                    reverseLerp = false;
            }
        }
    }

    private void MachineMovement()
    {

        //  Älskar dig <3

        if (Input.GetKeyDown(key) && lerpTimer <= 0)
            lerpMachine = OnMachineMove(button, buttonPressedSprite, lerpMachine, type);
    }

    private bool OnMachineMove(GameObject button, Sprite sprite, bool lerpMachine, int index)
    {
        SpriteRenderer sr = button.GetComponent<SpriteRenderer>();
        Sprite oldSprite = sr.sprite;
        sr.sprite = sprite;
		//moveSound(FirstSound);
		AudioManager.instance.Play(FirstSound);

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
}
