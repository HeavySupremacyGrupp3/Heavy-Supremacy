using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;

    private void Update()
    {
        transform.Translate(new Vector3(0f, Speed * Time.deltaTime, 0f), Space.Self);
        if (transform.localPosition.y > 2000 || transform.localPosition.y < -2000)
            Destroy(gameObject);
    }
}
