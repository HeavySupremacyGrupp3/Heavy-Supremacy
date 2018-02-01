using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayTrackerText : MonoBehaviour {

    public GameManager gm;
    private TextMesh t;

    public void Start()
    {
        t = GetComponent<TextMesh>();
        gm = GetComponent<GameManager>();
        t.text = "Approximately week: ";
        
    }

    public void Update()
    {
        t.text = "Approximately week: " + GameManager.week;
    }

}
