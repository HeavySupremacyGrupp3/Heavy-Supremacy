using UnityEngine;

public class CutoffLerp : MonoBehaviour
{
    public float TransitionTime = 1;

    private MeshRenderer meshRenderer;
    private float timer = 0;
    private string MaterialValueString = "_Cutoff";
    private float LerpStartValue = 1;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        timer = TransitionTime;
        meshRenderer.material.SetFloat(MaterialValueString, LerpStartValue);
    }

    void Update()
    {
        UpdateTransition();
    }

    void UpdateTransition()
    {
        timer -= Time.deltaTime;
        meshRenderer.material.SetFloat(MaterialValueString, Mathf.Lerp(0, 1, timer / TransitionTime));
    }
}
