using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayTracker : MonoBehaviour {

    public int day = 1;
    public int week = 1;
    private TextMesh t;

    public void Start()
    {
        t = GetComponent<TextMesh>();
        t.text = "Approximately week: ";
    }

    public void Update()
    {

        t.text = "Approximately week: " + week;
    }

    public void IncreaseDay()
    {
        day++;
        IncreaseWeek();
    }

    public void IncreaseWeek()
    {
        if (day%7 == 0)
        {
            week++;
        }
    }
}
