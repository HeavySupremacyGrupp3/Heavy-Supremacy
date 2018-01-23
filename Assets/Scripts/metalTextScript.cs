using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class metalTextScript : MonoBehaviour
{

    public float Metal;
    Stats StatReference;
    private Text t;

    void Start()
    {
        StatReference = FindObjectOfType<metalStatScript>();
        Metal = StatReference.getAmount();
        t = GetComponent<Text>();
    }

    void Update()
    {
        Metal = StatReference.getAmount();
        t.text = "Metal: " + Metal;
    }
}