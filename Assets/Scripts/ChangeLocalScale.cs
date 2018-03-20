using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLocalScale : MonoBehaviour {

    public float largeWidth;
    public float largeHeight;
    public float smallWidth;
    public float smallHeight;
    public Button thisButton;

    public void LargeSize()
    {
        thisButton.GetComponent<RectTransform>().sizeDelta = new Vector2(largeWidth, largeHeight);

    }

    public void SmallSize()
    {
        thisButton.GetComponent<RectTransform>().sizeDelta= new Vector2(smallWidth, smallHeight);

    }

}
