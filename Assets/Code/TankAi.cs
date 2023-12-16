using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TankAi : MonoBehaviour
{
    public static event UnityAction<TankAi> TankExploded;
    public static event UnityAction tankAimed;

    [SerializeField]
    private Transform player,
        playerBase;

    [SerializeField]
    private GameObject explosion;

    private int idleTime = 20;
    private int searchingTime = 20;
    private Attacker attacker;
    private RaycastHit2D hit;
    private int ignoreLayer = ~(1 << 7);
    private bool aiming;
    private float shootTime;
    private float aimTime = 0.5f;
    public float maxVertZone,
        maxHorZone;

    private enum BehaviourState
    {
        Idle = 0,
        SearchingPlayer = 1,
        SearchingBase = 2
    }

    private BehaviourState currentState;

    private Dictionary<BehaviourState, TankController> controllers =
        new Dictionary<BehaviourState, TankController>();
    private TankMover tankMover;
    public float yDistance,
        xDistance;
    private bool initiated;

    public void Init(int idleTime, int searchingTime, float aimTime)
    {
        this.idleTime = idleTime;
        this.searchingTime = searchingTime;
        this.aimTime = aimTime;
        Vector2 camsize = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        maxHorZone = camsize.x;
        maxVertZone = camsize.y;

        player = GameObject.FindWithTag("Player").transform;
        playerBase = FindAnyObjectByType<PlayerBase>().transform;
        attacker = GetComponent<Attacker>();
        Transform target = new GameObject(gameObject.name + "_Target").transform;
        target.position = transform.position;
        tankMover = GetComponent<TankMover>();
        tankMover.Init(target);

        TankIdler tankIdler = GetComponent<TankIdler>();
        tankIdler.Init(player, playerBase, target);
        controllers.Add(BehaviourState.Idle, tankIdler);
        tankIdler.Play();
        StartCoroutine(IdleTimer());

        PlayerSearcher playerSearcher = GetComponent<PlayerSearcher>();
        playerSearcher.Init(player, playerBase, target);
        controllers.Add(BehaviourState.SearchingPlayer, playerSearcher);

        BaseSearcher baseSearhcer = GetComponent<BaseSearcher>();
        baseSearhcer.Init(player, player, target);
        controllers.Add(BehaviourState.SearchingBase, baseSearhcer);
        initiated = true;
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

    IEnumerator PlayerSearcherTimer()
    {
        WaitForSecondsRealtime second = new WaitForSecondsRealtime(1);

        for (int timeLeft = searchingTime; timeLeft > 0; timeLeft--)
        {
            yield return second;
        }
        NextState();
    }

    public void StartSearchTimer()
    {
        StartCoroutine(PlayerSearcherTimer());
    }

    public void NextState()
    {
        controllers[currentState].Stop();
        if (currentState != BehaviourState.SearchingBase)
        {
            currentState++;
        }
        controllers[currentState].Play();
    }

    private void Update()
    {
        if (initiated && player && playerBase)
        {
            xDistance = Mathf.Abs(player.position.x - transform.position.x);
            yDistance = Mathf.Abs(player.position.y - transform.position.y);
            if (yDistance < maxVertZone && xDistance < maxHorZone)
            {
                hit = Physics2D.Raycast(
                    transform.position,
                    player.position - transform.position,
                    Mathf.Infinity,
                    ignoreLayer
                );

                if (hit.collider != null)
                {
                    CheckPlayer(hit);
                    return;
                }
            }
            else
            {
                CheckBase();
                if (!controllers[currentState].enabled)
                {
                    controllers[currentState].Play();
                }
            }
        }
    }

    private void CheckBase()
    {
        if (Vector2.Distance(transform.position, playerBase.transform.position) < 6)
        {
            hit = hit = Physics2D.Raycast(
                transform.position,
                playerBase.position - transform.position,
                Mathf.Infinity,
                ignoreLayer
            );

            if (hit.collider != null && hit.collider.CompareTag("Base"))
            {
                controllers[currentState].Stop();
                Vector2 directionView = (
                    playerBase.transform.position - transform.position
                ).normalized;
                transform.right = Vector2.MoveTowards(transform.right, directionView, 0.01f);
                if ((Vector2)transform.right == directionView && attacker.canAttack)
                {
                    if (!aiming)
                    {
                        shootTime = Time.time + aimTime / 2;
                        aiming = true;
                    }
                    if (aiming && shootTime <= Time.time)
                    {
                        aiming = false;
                        attacker.Fire(transform.right);
                    }
                }
                else
                {
                    aiming = false;
                }
            }
        }
        else if (!controllers[currentState].enabled)
        {
            aiming = false;
            controllers[currentState].Play();
        }
        else
        {
            aiming = false;
        }
    }

    private void CheckPlayer(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("Player"))
        {
            controllers[currentState].Stop();
            Vector2 directionView = (player.transform.position - transform.position).normalized;
            transform.right = Vector2.MoveTowards(transform.right, directionView, 0.01f);
            if ((Vector2)transform.right == directionView && attacker.canAttack)
            {
                if (!aiming)
                {
                    shootTime = Time.time + aimTime;
                    aiming = true;
                    tankAimed?.Invoke();
                }
                if (aiming && shootTime <= Time.time)
                {
                    aiming = false;
                    attacker.Fire(transform.right);
                }
            }
            else
            {
                aiming = false;
            }
        }
        else
        {
            CheckBase();
        }
    }

    [SerializeField]
    private AudioClip boom;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("projectile"))
        {
            TankExploded?.Invoke(this);
            Instantiate(explosion, transform.position, new Quaternion());
            Handheld.Vibrate();
            if (PlayerPrefs.GetInt("Sound", 1) == 1)
            {
                AudioSource.PlayClipAtPoint(boom, Vector3.zero);
            }
            Destroy(gameObject);
        }
    }
}
