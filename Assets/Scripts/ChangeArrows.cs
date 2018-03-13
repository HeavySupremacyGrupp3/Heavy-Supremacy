using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Arrows
{
    ShowBoth = 0,
    ShowOne = 1,
    ShowTwo = 2,
    ShowNone = 3,
};

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
	
	public void ResetArrows()
	{
		index = 0;
        SetPosAndRot();
	}

    public void NextArrowIndex()
    {
        index++;
        SetPosAndRot();
    }

    private void SetPosAndRot()
    {
        if (Arrow1Positions.Length > index)
            Arrow1.transform.position = Arrow1Positions[index];
        if (Arrow2Positions.Length > index)
            Arrow2.transform.position = Arrow2Positions[index];

        if (Arrow1Rotations.Length > index)
            Arrow1.transform.rotation = Quaternion.Euler(Arrow1Rotations[index]);
        if (Arrow2Rotations.Length > index)
            Arrow2.transform.rotation = Quaternion.Euler(Arrow2Rotations[index]);
    }

    public void ShowArrow(Arrows id = 0)
    {
        switch (id)
        {
            case Arrows.ShowBoth:
                Arrow1.gameObject.SetActive(true);
                Arrow2.gameObject.SetActive(true);
                break;
            case Arrows.ShowOne:
                Arrow1.gameObject.SetActive(true);
                Arrow2.gameObject.SetActive(false);
                break;
            case Arrows.ShowTwo:
                Arrow1.gameObject.SetActive(false);
                Arrow2.gameObject.SetActive(true);
                break;
            case Arrows.ShowNone:
                Arrow1.gameObject.SetActive(false);
                Arrow2.gameObject.SetActive(false);
                break;
            default:
                goto case Arrows.ShowNone;
        }
    }

}
