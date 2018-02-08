using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutProduktScript : MonoBehaviour {

	public bool moving=true;
	
	 void Update ()
	{
		if(moving)
			transform.Translate(Vector3.right * 3f*Time.deltaTime);
    }
	
	void OnTriggerEnter2D(Collider2D other) //kollisioner
	{
		if(other.gameObject.tag=="Box")
		{		
				Destroy(gameObject);
		}
    }
}

