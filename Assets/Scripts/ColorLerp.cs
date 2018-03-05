using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorLerp : MonoBehaviour
{
    public float Speed = 1;
    public Color StartColor = Color.white;
    public Color EndColor = Color.white;
    public bool Lerp = true;
    public bool Reverse;
    public float StartDelay = 0;

    private SpriteRenderer spriteRenderer;
    private TextMesh textMesh;
    private Text text;
    private Image image;
    private float timer = 1;
    private Color currentColor;

    void Start()
    {
        if (!Lerp && StartDelay > 0)
            StartCoroutine(StartLerp());

        spriteRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponent<TextMesh>();
        text = GetComponent<Text>();
        image = GetComponent<Image>();

        if (spriteRenderer != null)
            spriteRenderer.color = StartColor;
        else if (textMesh != null)
            textMesh.color = StartColor;
        else if (text != null)
            text.color = StartColor;
        else if (image != null)
            image.color = StartColor;
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

        currentColor = Color.Lerp(EndColor, StartColor, timer);

        if (spriteRenderer != null)
            spriteRenderer.color = currentColor;
        else if (textMesh != null)
            textMesh.color = currentColor;
        else if (text != null)
            text.color = currentColor;
        else if (image != null)
            image.color = currentColor;
    }

    public void ReverseLerp()
    {
        Reverse = !Reverse;
    }
}