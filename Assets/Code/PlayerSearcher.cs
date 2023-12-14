using System.Collections;
using UnityEngine;

public class PlayerSearcher : TankController
{
    public override void Play()
    {
        base.Play();
        Debug.Log($"{gameObject.name} start searching player");
        GetComponent<TankAi>().StartSearchTimer();
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) < 1)
            {
                SetNewTarget();
            }
        }
    }

    private void SetNewTarget()
    {
        Vector3 destination = new Vector2(
            player.position.x + Random.Range(-3, 4),
            player.position.y + Random.Range(-3, 4)
        );

        if (
            destination.x < 12.4f
            && destination.x > -12.4f
            && destination.y < 7.4f
            && destination.y > -7.4f
            && tankMover.IsAchievable(destination)
        )
        {
            target.position = destination;
            tankMover.Move();
        }
        else
        {
            SetNewTarget();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tank"))
        {
            SetNewTarget();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Tank"))
        {
            SetNewTarget();
        }
    }
}
