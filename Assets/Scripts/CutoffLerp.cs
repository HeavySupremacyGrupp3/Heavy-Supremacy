using UnityEngine;
using System.Collections;

public class CutoffLerp : MonoBehaviour
{
    public float Speed = 1;
    public bool Lerp = true;
    public bool Reverse;
    public float StartDelay = 0;

    private SpriteRenderer spriteRenderer;
    private float timer = 1;
    private float currentTransition;
    private string materialValueName = "_Cutoff";

    void Start()
    {
        if (!Lerp && StartDelay > 0)
            StartCoroutine(StartLerp());

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.material.SetFloat(materialValueName, currentTransition);
    }

    IEnumerator StartLerp()
    {
        yield return new WaitForSeconds(StartDelay);
        Lerp = true;
    }

    void Update()
    {
        if (Lerp)
            UpdateTransition();
    }

    void UpdateTransition()
    {
        if (Reverse && timer <= 1)
            timer += Time.deltaTime * Speed;
        else if (timer >= 0 && !Reverse)
            timer -= Time.deltaTime * Speed;

        currentTransition = Mathf.Lerp(1, 0, timer);

        if (spriteRenderer != null)
            spriteRenderer.material.SetFloat(materialValueName, currentTransition);
    }

    public void ReverseLerp()
    {
        Reverse = !Reverse;
    }
}