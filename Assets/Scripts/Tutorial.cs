using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tutorial : MonoBehaviour
{
    public GameObject[] TextSections;
    public GameObject[] HighlightedObjects;
    public string[] AnimationTriggers;
    public KeyCode[] Buttons;
    public GameObject[] ObjectsToDisableOnStart;
    public int LastStepIndex = 0;
    public int DefaultLayer = 1, PopOutLayer = 200;

    private uint currentIndex = 0;
    private Animator anim;
    private bool isAllowedClick = false;
    private KeyCode nextButton;
    private Dictionary<SpriteRenderer, int> DefaultLayers = new Dictionary<SpriteRenderer, int>();

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Run()
    {
        gameObject.SetActive(true);
        currentIndex = 0;
        foreach (GameObject objects in ObjectsToDisableOnStart)
            objects.SetActive(false);
        ShowText();
    }

    void highlightCurrentObject()
    {
        for (int i = 0; i < HighlightedObjects.Length; i++)
        {
            if (HighlightedObjects[i] == null)
                continue;

            changeOrderInLayerForObject(HighlightedObjects[i], DefaultLayer, true);

            if (i == currentIndex)
                changeOrderInLayerForObject(HighlightedObjects[i], PopOutLayer);
        }  
    }

    public void HideHighlightedObjects()
    {
        for (int i = 0; i < HighlightedObjects.Length; i++)
        {
            if (HighlightedObjects[i] == null)
                continue;

            changeOrderInLayerForObject(HighlightedObjects[i], DefaultLayer, true);
        }
    }

    //g = object to have it's order in layer changed, o = the new order in layer for said object, set = set the layer to a number, don't add it
    void changeOrderInLayerForObject(GameObject g, int o, bool reset = false)
    {
        foreach (SpriteRenderer spriteRenderer in g.GetComponentsInChildren<SpriteRenderer>())
        {
            if (!DefaultLayers.ContainsKey(spriteRenderer))
            {
                DefaultLayers.Add(spriteRenderer, spriteRenderer.sortingOrder);
            }

            if (!reset)
                spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + o; // Add offset to current layer number
            else
                spriteRenderer.sortingOrder = DefaultLayers[spriteRenderer];
        }
    }

    public void ClickedScreen()
    {
        if (!isAllowedClick || !Input.GetKeyUp(nextButton))
            return;

        if (LastStepIndex == currentIndex)
        {
            gameObject.SetActive(false);
            foreach (GameObject objects in ObjectsToDisableOnStart)
                objects.SetActive(true);
				
			HideText();
            return;
        }

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