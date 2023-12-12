using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAi : MonoBehaviour
{
    private bool inAction;

    private enum BehaviourState
    {
        Idle,
        SearchingPlayer,
        SearchingBase
    }

    private BehaviourState currentState;

    private Transform player;
}
