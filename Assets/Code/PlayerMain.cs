using Kino;
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

    [SerializeField]
    private AudioClip warningSound;

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

    [SerializeField]
    private AudioClip boom,
        shieldSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("projectile"))
        {
            Handheld.Vibrate();
            if (shield.isPlaying)
            {
                if (PlayerPrefs.GetInt("Sound", 1) == 1)
                {
                    AudioSource.PlayClipAtPoint(shieldSound, Vector3.zero);
                }
                Instantiate(flash, transform.position, new Quaternion());
                shield.Stop();
            }
            else
            {
                if (PlayerPrefs.GetInt("Sound", 1) == 1)
                {
                    AudioSource.PlayClipAtPoint(boom, Vector3.zero);
                }
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
            if (PlayerPrefs.GetInt("Sound", 1) == 1)
            {
                AudioSource.PlayClipAtPoint(warningSound, Vector3.zero);
            }
            Instantiate(warning, (Vector2)transform.position + Vector2.up, new Quaternion());
        }
    }
}
