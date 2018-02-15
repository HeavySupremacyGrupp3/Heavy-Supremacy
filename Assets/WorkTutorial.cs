using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorkTutorial : MonoBehaviour
{
    public GameObject[] TextSections;
    public string[] AnimationTriggers;
    private uint currentIndex = 0;
    private uint sections = 0;
    private Animator anim;
    private bool isAllowedClick = false;
	void Start ()
    {
        anim = GetComponent<Animator>();
		sections = (uint)TextSections.Length;
        TextSections[currentIndex].SetActive(true);
    }

    public void ClickedScreen()
    {
        if (!isAllowedClick)
            return;

        HideText();
        NextIndex();
        TriggerAnimation();
        isAllowedClick = false;
    }

    public void AllowClick()
    {
        isAllowedClick = true;
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
