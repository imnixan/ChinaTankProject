using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAi : MonoBehaviour
{
    [SerializeField]
    private Transform player,
        playerBase;

    private int idleTime,
        searchingRadius;
    private Attacker attacker;
    private RaycastHit2D hit;
    private int playerLayer = ~1 << 7;

    private enum BehaviourState
    {
        Idle,
        SearchingPlayer,
        SearchingBase
    }

    private BehaviourState currentState;

    private Dictionary<BehaviourState, TankController> controllers =
        new Dictionary<BehaviourState, TankController>();
    private TankMover tankMover;

    private void Start()
    {
        attacker = GetComponent<Attacker>();
        Transform target = new GameObject(gameObject.name + "_Target").transform;
        target.position = transform.position;
        tankMover = gameObject.AddComponent<TankMover>();
        tankMover.Init(target);

        TankIdler tankIdler = gameObject.AddComponent<TankIdler>();
        tankIdler.Init(player, playerBase, target);
        controllers.Add(BehaviourState.Idle, tankIdler);
        tankIdler.Play();
        //StartCoroutine(IdleTimer());
    }

    IEnumerator IdleTimer()
    {
        WaitForSecondsRealtime second = new WaitForSecondsRealtime(1);

        for (int timeLeft = idleTime; timeLeft > 0; timeLeft--)
        {
            yield return second;
        }
        NextState();
    }

    public void NextState()
    {
        controllers[currentState].Stop();
        currentState++;
        //controllers[currentState].Play();
    }

    private void Update()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, playerLayer);
        if (hit.collider != null)
        {
            CheckPlayer(hit);
        }
        hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, playerLayer);
        if (hit.collider != null)
        {
            CheckPlayer(hit);
        }
        hit = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, playerLayer);
        if (hit.collider != null)
        {
            CheckPlayer(hit);
        }
        hit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, playerLayer);
        if (hit.collider != null)
        {
            CheckPlayer(hit);
        }
    }

    private void CheckPlayer(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("Player"))
        {
            controllers[currentState].Stop();
            transform.right = hit.collider.transform.position - transform.position;
            if (attacker.CanAttack)
            {
                attacker.Fire(transform.right * 100);
            }
            else
            {
                controllers[currentState].Play();
                //NextState();
            }
        }
    }
}
