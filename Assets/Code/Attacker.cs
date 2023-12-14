using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField]
    private Projectile projectile;

    private float lastShot;

    private float recharge = 1.5f;
    public bool CanAttack
    {
        get { return lastShot + recharge < Time.time; }
    }

    public void Fire()
    {
        if (CanAttack)
        {
            Vector2 direction = transform.up * 100;
            Instantiate(projectile, transform.position + transform.up * 0.5f, new Quaternion())
                .Shoot(direction);
            lastShot = Time.time;
        }
    }

    public void Fire(Vector2 direction)
    {
        if (CanAttack)
        {
            Instantiate(projectile, transform.position + transform.up * 0.5f, new Quaternion())
                .Shoot(direction);
            lastShot = Time.time;
        }
    }
}
