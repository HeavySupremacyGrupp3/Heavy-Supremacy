using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvasionEnemy : MonoBehaviour
{
    public GameObject NextEnemy;
    public ParticleSystem ParticleTrail;
    public int Hitpoints;
    public float Speed;
    public static float BlinkTime = 0.1f;
    public static float BlinkMaxAmount = 0.75f;
    public AudioClip[] Hits;

    private void Start()
    {
        LoveInvasion.SpawnedEnemies.Add(gameObject);
        if (ParticleTrail != null)
            Invoke("StartParticleSystem", 0.7f);
    }

    private void StartParticleSystem()
    {
        ParticleTrail.Play();
    }

    private void OnDestroy()
    {
        LoveInvasion.SpawnedEnemies.Remove(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Bullet"))
        {
            Hit();
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("Ground"))
        {
            Destroy(gameObject);
            LoveInvasion.LostGame = true;
        }
    }

    private void Update()
    {
        Vector2 pos = transform.localPosition;
        pos.y -= Speed * Time.deltaTime;
        transform.localPosition = pos;
    }

    private void Hit()
    {
        Hitpoints--;

        if (Hitpoints == 0)
        {
            if (NextEnemy != null)
                Instantiate(NextEnemy, transform.position, Quaternion.identity, transform.parent);

            Destroy(gameObject);
            AudioManager.instance.Play("LoveInvasion_Destroy");
        }
        else
        {
            StartCoroutine(Blink());
            AudioClip clip = Hits[Random.Range(0, Hits.Length)];
            AudioManager.instance.GetSource("LoveInvasion_Hit").PlayOneShot(clip);
        }
    }

    private IEnumerator Blink()
    {
        Image img = GetComponent<Image>();
        for (int i = 0; i < 2; i++)
        {
            float timeElapsed = 0;
            while (timeElapsed < BlinkTime / 2f)
            {
                img.material.SetFloat("_Blink", (timeElapsed / (BlinkTime / 2)) * BlinkMaxAmount);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            while (timeElapsed < BlinkTime)
            {
                img.material.SetFloat("_Blink", BlinkMaxAmount - ((timeElapsed - (BlinkTime / 2)) * BlinkMaxAmount));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        img.material.SetFloat("_Blink", 0f);
    }
}
