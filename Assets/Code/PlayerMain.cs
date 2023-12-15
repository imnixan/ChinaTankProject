using System.Collections;
using UnityEngine;

public class PlayerMain : MonoBehaviour
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
