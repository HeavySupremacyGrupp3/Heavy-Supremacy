using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayTracker : MonoBehaviour {

    public int day = 1;
    public int week = 1;
    private TextMesh t;


    public void IncreaseDay()
    {
        day++;
        IncreaseWeek();
        t.text = "Approximately week: " + week;
    }

    public void IncreaseWeek()
    {
        if (day%7 == 0)
        {
            week++;
        }
    }
}
