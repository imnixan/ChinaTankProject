using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class Attacker : MonoBehaviour
{
    [SerializeField]
    private Projectile projectile;

    [SerializeField]
    private GunReload gr;

    public bool canAttack;

    private void Start()
    {
        canAttack = true;
    }

    public void Fire()
    {
        if (canAttack)
        {
            Vector2 direction = transform.up * 100;
            Instantiate(projectile, transform.position + transform.up * 0.5f, new Quaternion())
                .Shoot(direction);
            StartCoroutine(Reload());
            if (gr != null)
            {
                gr.TurnOn();
            }
        }
    }

    IEnumerator Reload()
    {
        canAttack = false;
        for (float i = 0; i <= 1.5f; i += 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        canAttack = true;
    }

    public void Fire(Vector2 direction)
    {
        if (canAttack)
        {
            Instantiate(projectile, transform.position + transform.up * 0.5f, new Quaternion())
                .Shoot(direction * 100);
            StartCoroutine(Reload());
        }
    }
}
