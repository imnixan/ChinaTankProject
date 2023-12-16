using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion,
        flash,
        shieldEffect,
        warning;

    [SerializeField]
    private GameManager gm;

    private Vector2 spawnPoint = new Vector2(0, -2.5f);

    private ParticleSystem shield;

    [SerializeField]
    private int shieldTime = 5;

    private void Start()
    {
        shield = GetComponentInChildren<ParticleSystem>();
        TurnOnShield();
    }

    IEnumerator ShieldTime()
    {
        WaitForSeconds second = new WaitForSeconds(1);
        for (int i = 0; i < shieldTime; i++)
        {
            yield return second;
        }
        shield.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("projectile"))
        {
            if (shield.isPlaying)
            {
                Instantiate(flash, transform.position, new Quaternion());
                shield.Stop();
            }
            else
            {
                Instantiate(explosion, transform.position, new Quaternion());
                gm.PlayerDead();
            }
        }
    }

    public void TurnOnShield()
    {
        Instantiate(shieldEffect, transform.position, new Quaternion());
        shield.Play();
        StartCoroutine(ShieldTime());
    }

    public void Respawn()
    {
        transform.position = spawnPoint;
        TurnOnShield();
    }

    public void OnEnable()
    {
        TankAi.tankAimed += OnTankAimed;
    }

    public void OnDisable()
    {
        TankAi.tankAimed -= OnTankAimed;
    }

    private float lastWarning;
    private float warningPause = 1f;

    private void OnTankAimed()
    {
        if (lastWarning + warningPause <= Time.time)
        {
            lastWarning = Time.time;
            Instantiate(warning, (Vector2)transform.position + Vector2.up, new Quaternion());
        }
    }
}
