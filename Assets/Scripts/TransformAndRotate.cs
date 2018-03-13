using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAndRotate : MonoBehaviour {

    [Header("Units per second")]
    public float RotationZ;
    public float TransformX;
    public float TransformY;

    private bool rotate = true;

    void OnEnable()
    {
        MiniGameManager.stopEverything += StopRotation;
    }

    void OnDisable()
    {
        MiniGameManager.stopEverything -= StopRotation;
    }

    

    void Update ()
    {
        if (rotate)
        {
            transform.Translate(new Vector3(TransformX * Time.deltaTime, TransformY * Time.deltaTime, 0));
            transform.Rotate(new Vector3(0, 0, RotationZ * Time.deltaTime));
        }
	}

    private void StopRotation()
    {
        rotate = !rotate;
    }
}
