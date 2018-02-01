using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fameTextScript : MonoBehaviour
{
    public float Fame;
    Stats StatReference;
    private Text t;

    void Start()
    {
        StatReference = FindObjectOfType<fameStatScript>();
        Fame = StatReference.getAmount();
        t = GetComponent<Text>();
    }

    void Update()
    {
        Fame = StatReference.getAmount();
        t.text = "Fame: " + Fame;
    }
}
