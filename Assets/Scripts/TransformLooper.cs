using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLooper : MonoBehaviour
{

    public float Speed = 1;
    public Vector2 TargetPosition;

    private float posTimer = 0;
    private Vector2 startPos;

    [SerializeField]
    private GameObject travelator;

    private void Start()
    {
        startPos = transform.localPosition;
        TargetPosition = new Vector2(transform.localPosition.x + TargetPosition.x, transform.localPosition.y + TargetPosition.y);
    }

    void Update()
    {
        //NewTravelator();
        posTimer += Time.deltaTime * Speed;
        if (posTimer >= 1)
            posTimer = 0;

        transform.localPosition = Vector2.Lerp(startPos, TargetPosition, posTimer);
    }

    private void NewTravelator()
    {
        if (transform.position.x == 0)
        {
            travelator = Instantiate(travelator, new Vector3(startPos.x -19.43f, startPos.y), Quaternion.identity);
            //-19.43
        }
    }
}
