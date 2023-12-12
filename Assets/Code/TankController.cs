using System.Collections;
using UnityEngine;

public abstract class TankController : MonoBehaviour
{
    protected TankMover tankMover;
    protected Transform player;
    protected Transform playerBase;

    public void Init(TankMover tankMover, Transform player, Transform playerBase)
    {
        this.tankMover = tankMover;
        this.player = player;
        this.playerBase = playerBase;
    }

    public void Stop()
    {
        tankMover.StopMoving();
    }
}
