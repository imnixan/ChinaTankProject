using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject hitEffect;
    private Rigidbody2D rb;

    public void Shoot(Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        transform.up = direction;
        rb.AddForce(direction * 10);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(hitEffect, collision.contacts[0].point, new Quaternion());
        Destroy(gameObject);
    }
}
