using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomButtonHitboxScript : MonoBehaviour {
	
	public float DistanceFromOrigo;
	
	float x;
	float y;
	
	void checkIfInsideBoundaries()
	{
		/*if(x^2 + y^2 == DistanceFromOrigo^2)
			Debug.Log("On the edge of the circle.");
		
		if(x^2 + y^2 < DistanceFromOrigo^2)
			Debug.Log("Inside the circle.");
		
		if(x^2 + y^2 < DistanceFromOrigo^2)
			Debug.Log("Outside the circle.");
		*/
	}
}
