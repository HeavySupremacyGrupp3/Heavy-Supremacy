using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    public float TransitionTime = 1;
    public Color StartColor = Color.white;
    public Color EndColor = Color.white;

    private SpriteRenderer spriteRenderer;
    private float timer;

	void Start ()
    {
        timer = TransitionTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = StartColor;
	}
	
	void Update ()
    {
        UpdateTransition();
	}

    void UpdateTransition()
    {
        timer -= Time.deltaTime;
        spriteRenderer.color = Color.Lerp(EndColor, StartColor, timer / TransitionTime);
    }
}
