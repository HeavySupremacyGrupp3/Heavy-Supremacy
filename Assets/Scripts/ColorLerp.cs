using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    public float TransitionTime = 1;
    public Color StartColor = Color.white;
    public Color EndColor = Color.white;
    public bool Lerp;
    public bool Reverse;


    private SpriteRenderer spriteRenderer;
    private TextMesh textMesh;
    private float timer;
    private Color currentColor;

    void Start()
    {
        timer = TransitionTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponent<TextMesh>();

        if (spriteRenderer != null)
            spriteRenderer.color = StartColor;
        else if (textMesh != null)
            textMesh.color = StartColor;
    }

    void Update()
    {
        if(Lerp)
            UpdateTransition();
    }

    void UpdateTransition()
    {
        if (Reverse && timer >= 0)
            timer -= Time.deltaTime;
        else if (timer <= 1 && !Reverse)
            timer += Time.deltaTime;

        currentColor = Color.Lerp(EndColor, StartColor, timer / TransitionTime);

        if (spriteRenderer != null)
            spriteRenderer.color = currentColor;
        if (textMesh != null)
            textMesh.color = currentColor;
    }

    public void ReverseLerp()
    {
        Reverse = !Reverse;
    }
}
