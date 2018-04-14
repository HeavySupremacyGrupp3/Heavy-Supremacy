using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    public RectTransform ParentPanel;
    public GameObject Bullet;
    public float Cooldown;

    private bool IsOnCooldown = false;

    private void Update()
    {
        PointAtMouse();
        RegisterClick();
    }

    private void OnEnable()
    {
        IsOnCooldown = false;
    }

    private void RegisterClick()
    {
        if (IsOnCooldown)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            StartCoroutine(ResetCooldown());
            Instantiate(Bullet, transform.position, transform.rotation, ParentPanel);
            AudioManager.instance.Play("LoveInvasion_Shoot");
        }
    }

    private void PointAtMouse()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;
        transform.up = direction;
    }

    private IEnumerator ResetCooldown()
    {
        IsOnCooldown = true;
        yield return new WaitForSeconds(Cooldown);
        IsOnCooldown = false;
    }
}
