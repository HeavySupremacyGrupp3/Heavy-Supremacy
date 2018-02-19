using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayTrackerText : MonoBehaviour {

    public GameManager gm;
    private Text t;

    public void Start()
    {
        t = GetComponent<Text>();
        gm = GetComponent<GameManager>();
        t.text = "Week: ";
        
    }

    public void Update()
    {
        t.text = "Week: " + GameManager.week;
    }

}
