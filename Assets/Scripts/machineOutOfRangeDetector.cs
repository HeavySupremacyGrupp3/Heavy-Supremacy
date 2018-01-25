using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineOutOfRangeDetector : MonoBehaviour {

	public delegate void targetDetector(GameObject t);
	public event targetDetector productToSpoilDetected;
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		productToSpoilDetected(collision.gameObject);
	}
}
