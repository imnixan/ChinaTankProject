using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private Attacker attacker;

    private void Start()
    {
        attacker = GetComponent<Attacker>();
    }

    public void Attack()
    {
        attacker.Fire();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }
}
