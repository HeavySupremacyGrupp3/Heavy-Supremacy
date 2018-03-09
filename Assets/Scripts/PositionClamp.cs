using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionClamp : MonoBehaviour {

    public Transform myTransform;

    public Vector2 Constraints;
    private Vector2 startPos;

    private void Start()
    {
        startPos = myTransform.position;
    }

    private void Update ()
    {
        myTransform.position = new Vector2(Mathf.Clamp(myTransform.position.x, startPos.x - Constraints.x, startPos.x + Constraints.x),
            Mathf.Clamp(myTransform.position.y, startPos.y - Constraints.y, startPos.y + Constraints.y));
	}
}
