using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorkTutorial : MonoBehaviour
{
    public GameObject[] TextSections;
    public string[] AnimationTriggers;
    public KeyCode[] Buttons;

    private uint currentIndex = 0;
    private uint sections = 0;
    private Animator anim;
    private bool isAllowedClick = false;
    private KeyCode nextButton;
	void Start ()
    {
        anim = GetComponent<Animator>();
		sections = (uint)TextSections.Length;
        TextSections[currentIndex].SetActive(true);
    }

    public void ClickedScreen()
    {
        if (!isAllowedClick || !Input.GetKeyUp(nextButton))
            return;

        HideText();
        NextIndex();
        TriggerAnimation();
        isAllowedClick = false;
    }

    private void Update()
    {
        ClickedScreen();
    }

    public void AllowClick()
    {
        isAllowedClick = true;
        nextButton = Buttons[currentIndex];
    }

    public void TriggerAnimation()
    {
        anim.SetTrigger(AnimationTriggers[currentIndex]);
    }

    public void HideText()
    {
        TextSections[currentIndex].SetActive(false);
    }

    public void ShowText()
    {
        TextSections[currentIndex].SetActive(true);
    }

    public void NextIndex()
    {
        currentIndex++;
    }


}