using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorkTutorial : MonoBehaviour
{
    public GameObject[] TextSections;
	public GameObject[] HighlightedObjects;
    public string[] AnimationTriggers;
    public KeyCode[] Buttons;
	public uint HighlightInterval;

    private uint currentIndex = 0;
    private uint sections = 0;
    private Animator anim;
    private bool isAllowedClick = false;
    private KeyCode nextButton;
	private uint A, B;
	
	void Start ()
    {
        anim = GetComponent<Animator>();
		sections = (uint)TextSections.Length;
        TextSections[currentIndex].SetActive(true);
    }
	
	void highlightCurrentObject()
	{
		A=currentIndex/HighlightInterval;
		B=(currentIndex+HighlightInterval)/HighlightInterval;
		
		Component[] spritedChildren;
		if(currentIndex>0)		
		{
			HighlightedObjects[A].GetComponent<SpriteRenderer>().sortingOrder=1;
			spritedChildren = HighlightedObjects[A].GetComponentsInChildren<SpriteRenderer>();		
			
			foreach (SpriteRenderer spriteRenderer in spritedChildren)
				spriteRenderer.sortingOrder=1;	
		}	
		
		HighlightedObjects[B].GetComponent<SpriteRenderer>().sortingOrder=200;
		spritedChildren = HighlightedObjects[B].GetComponentsInChildren<SpriteRenderer>();			
			
		foreach (SpriteRenderer spriteRenderer in spritedChildren)
			spriteRenderer.sortingOrder=200;	
		
	}

    public void ClickedScreen()
    {
        if (!isAllowedClick || !Input.GetKeyUp(nextButton))
            return;

        HideText();
        NextIndex();
        TriggerAnimation();
        isAllowedClick = false;
		
		Debug.Log("Rock me like a hurricane "+currentIndex);
		
		if(currentIndex%HighlightInterval==0 && (B)<HighlightedObjects.Length)
			highlightCurrentObject();
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