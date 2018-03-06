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
			changeOrderInLayerForObject(HighlightedObjects[A], 1);
		}	
		
		if(B<HighlightedObjects.Length)
		changeOrderInLayerForObject(HighlightedObjects[B], 200);	
	}
	
	//g = object to have it's order in layer changed, o = the new order in layer for said object
	void changeOrderInLayerForObject(GameObject g, int o)
	{
		g.GetComponent<SpriteRenderer>().sortingOrder=o;		
		Component[] spritedChildren;
		spritedChildren = g.GetComponentsInChildren<SpriteRenderer>();			
			
		foreach (SpriteRenderer spriteRenderer in spritedChildren)
			spriteRenderer.sortingOrder=o;
	}

    public void ClickedScreen()
    {
        if (!isAllowedClick || !Input.GetKeyUp(nextButton))
            return;

        HideText();
        NextIndex();
        TriggerAnimation();
        isAllowedClick = false;
		
		if(currentIndex%HighlightInterval==0 && B<HighlightedObjects.Length)
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