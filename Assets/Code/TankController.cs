using System.Collections;
using UnityEngine;

public abstract class TankController : MonoBehaviour
{
    protected TankMover tankMover;
    protected Transform player;
    protected Transform playerBase;

    [SerializeField]
    protected Transform target;
    protected bool inAction;

    public void Init(Transform player, Transform playerBase, Transform target)
    {
        this.tankMover = GetComponent<TankMover>();
        this.player = player;
        this.playerBase = playerBase;
        this.target = target;
    }

    public void Stop()
    {
        tankMover.StopMoving();
        this.enabled = false;
    }

    public void Play()
    {
        tankMover.Move();
        this.enabled = true;
    }
}
