using System.Collections;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField]
    private GameObject explodeEffect;
    private SpriteRenderer sr;
    private int hp = 3;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("projectile"))
        {
            hp--;
            sr.color = new Color(1, 0.25f * hp, 0.25f * hp);
            if (hp == 0)
            {
                Instantiate(explodeEffect, transform.position, new Quaternion());
                Destroy(gameObject);
            }
        }
    }
}
