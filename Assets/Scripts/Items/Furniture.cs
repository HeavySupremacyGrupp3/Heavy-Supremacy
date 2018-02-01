using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddGameObject(GameObject go)
    {
        GameObject goTemp = Instantiate(go, go.transform.position, Quaternion.identity, GameObject.Find("buttonCanvas").transform) as GameObject;
        goTemp.name = go.name;
    }
}
