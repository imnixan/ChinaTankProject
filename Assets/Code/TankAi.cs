using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAi : MonoBehaviour
{
    [SerializeField]
    private Transform player,
        playerBase;

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
        Transform target = new GameObject(gameObject.name + "_Target").transform;
        target.position = transform.position;
        tankMover = gameObject.AddComponent<TankMover>();
        tankMover.Init(target);

        TankIdler tankIdler = gameObject.AddComponent<TankIdler>();
        tankIdler.Init(player, playerBase, target);
        controllers.Add(BehaviourState.Idle, tankIdler);
        tankIdler.Activate();
    }
}
