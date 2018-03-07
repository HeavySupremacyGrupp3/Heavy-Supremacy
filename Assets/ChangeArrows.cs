using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeArrows : MonoBehaviour
{
    public Transform Arrow1, Arrow2;

    public Vector3[] Arrow1Positions;
    public Vector3[] Arrow1Rotations;
    public Vector3[] Arrow2Positions;
    public Vector3[] Arrow2Rotations;

    private int index = 0;

    private void Start()
    {
        SetPosAndRot();
    }

    public void NextArrowIndex()
    {
        index++;
        SetPosAndRot();
    }

    private void SetPosAndRot()
    {
        Arrow1.transform.position = Arrow1Positions[index];
        Arrow2.transform.position = Arrow2Positions[index];

        Arrow1.transform.rotation = Quaternion.Euler(Arrow1Rotations[index]);
        Arrow2.transform.rotation = Quaternion.Euler(Arrow2Rotations[index]);
    }

    public void ShowArrow(int id = 0)
    {
        switch (id)
        {
            case 0:
                Arrow1.gameObject.SetActive(true);
                Arrow2.gameObject.SetActive(true);
                break;
            case 1:
                Arrow1.gameObject.SetActive(true);
                Arrow2.gameObject.SetActive(false);
                break;
            case 2:
                Arrow1.gameObject.SetActive(false);
                Arrow2.gameObject.SetActive(true);
                break;
            case 3:
                Arrow1.gameObject.SetActive(false);
                Arrow2.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

}
