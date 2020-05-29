using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttack : MonoBehaviour, IAttack
{
    private enum State
    {
        Moving,
        Attacking
    }

    private State m_state;

    private void Awake()
    {
        SetStateNormal();
    }

    private void SetStateAttacking()
    {
        m_state = State.Attacking;
        GetComponent<IMoveVelocity>().Disable();
    }

    private void SetStateNormal()
    {
        m_state = State.Moving;
        GetComponent<IMoveVelocity>().Enable();
    }

    public void Attack()
    {
        SetStateAttacking();

        Vector3 attackDir = transform.forward;
    }
}
