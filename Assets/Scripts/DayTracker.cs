using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTracker : MonoBehaviour {

    public int day = 1;
    public int week = 1;


    public void IncreaseDay()
    {
        day++;
    }

    public void IncreaseWeek()
    {
        if (day%7 == 0)
        {
            week++;
        }
    }
}
