using UnityEngine;
using UnityEngine.UI;

public class ColorLerp : MonoBehaviour
{
    public float Speed = 1;
    public Color StartColor = Color.white;
    public Color EndColor = Color.white;
    public bool Lerp = true;
    public bool Reverse;

    private SpriteRenderer spriteRenderer;
    private TextMesh textMesh;
    private Text text;
    private float timer = 1;
    private Color currentColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponent<TextMesh>();
        text = GetComponent<Text>();

        if (spriteRenderer != null)
            spriteRenderer.color = StartColor;
        else if (textMesh != null)
            textMesh.color = StartColor;
        else if (text != null)
            text.color = StartColor;
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

        currentColor = Color.Lerp(EndColor, StartColor, timer);

        if (spriteRenderer != null)
            spriteRenderer.color = currentColor;
        else if (textMesh != null)
            textMesh.color = currentColor;
        else if (text != null)
            text.color = currentColor;
    }

    public void ReverseLerp()
    {
        Reverse = !Reverse;
    }
}