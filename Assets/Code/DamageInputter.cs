using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DamageInputter : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("projectile"))
        {
            Instantiate(explosion, transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }
}
