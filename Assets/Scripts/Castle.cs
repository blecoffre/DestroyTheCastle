using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Entity, IAttackable, IDamageable, ITargetable
{

    private Action<float> m_takeDamage = null;

    private void Awake()
    {
        m_takeDamage += ApplyDamage;
        SetDieAction();
    }

    private void SetDieAction()
    {
        ClearDieAction();
        DieAction += CastleExplose;
    }

    public bool IsTargetable()
    {
        return CurrentHealth > 0;
    }

    public void TakeDamage(float damage)
    {
        m_takeDamage.InvokeIfNotNull(damage);
    }

    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);
    }

    public void CastleExplose()
    {
        DestroyEntity();
    }
}
