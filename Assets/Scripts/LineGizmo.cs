using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGizmo : MonoBehaviour {

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(transform.position.x, 10000f), new Vector3(transform.position.x, -10000f));
    }
}
