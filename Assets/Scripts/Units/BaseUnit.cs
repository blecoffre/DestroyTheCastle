using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct TargetInfos
{
    public Transform targetTransform;
    public IDamageable targetDamagble;
}

public class BaseUnit : Entity, IDamageable, IMovable
{
    [SerializeField]
    private UnitUI m_unitUI;

    #region Stats

    /* Attack */
    [SerializeField]
    private float m_baseAttack;
    [HideInInspector]
    public float BaseAttack
    {
        get
        {
            return m_baseAttack;
        }
    }
    private float m_currentAttack;
    public float CurrentAttack
    {
        get
        {
            return m_currentAttack;
        }
    }

    /* Attack Range */
    [SerializeField]
    private float m_baseAttackRange;
    [HideInInspector]
    public float BaseAttackRange
    {
        get
        {
            return m_baseAttackRange;
        }
    }
    private float m_currentAttackRange;
    public float CurrentAttackRange
    {
        get
        {
            return m_currentAttackRange;
        }
    }

    /* Attack Cooldown */
    [SerializeField]
    private float m_baseAttackCooldown;
    [HideInInspector]
    public float BaseAttackCooldown
    {
        get
        {
            return m_baseAttackCooldown;
        }
    }
    private float m_currentAttackCooldown;
    public float CurrentAttackCooldown
    {
        get
        {
            return m_currentAttackCooldown;
        }
    }


    /* Move Speed */
    [SerializeField]
    private float m_baseMoveSpeed;
    [HideInInspector]
    public float BaseMoveSpeed
    {
        get
        {
            return m_baseMoveSpeed;
        }
    }

    private float m_currentMoveSpeed;
    public float CurrentMoveSpeed
    {
        get
        {
            return m_currentMoveSpeed;
        }
    }

    #endregion Stats

    private TargetInfos m_focusedTarget = new TargetInfos();
    private Transform m_inFrontFriend = default;

    private float m_attackCooldownTimer;

    private Action<float> m_takeDamageCallback = null;

    private float m_maxRayLength = 2000.0f;

    private float m_distBetweenUnits = 1.0f;

    private void Awake()
    {
        m_attackCooldownTimer = 0.0f;

        m_takeDamageCallback += ApplyDamage;
        SetDieAction();

        GetInFrontFriend();

        SetStats();
    }

    private void Start()
    {
        m_unitUI.Initialize();
        HealthBar = m_unitUI.GetHealthBar();
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    private void SetStats()
    {
        m_currentAttack = m_baseAttack;
        m_currentAttackRange = m_baseAttackRange;
        m_currentAttackCooldown = m_baseAttackCooldown;
        m_currentMoveSpeed = m_baseMoveSpeed;
    }
    
    private void SetDieAction()
    {
        ClearDieAction();
        DieAction += EntityDie;
    }

    private void Update()
    {
        if (IsTargetWithinAttackRange())
            Attack();
        else
            Move();

        if (!HasTarget())
        {
            GetTarget();
        }
    }

    private bool IsTargetWithinAttackRange()
    {
        if (HasTarget())
            Debug.Log("Pos truc : " + transform.GetComponent<Collider>().ClosestPointOnBounds(m_focusedTarget.targetTransform.position));
        if(HasTarget())
            Debug.Log("Distance from castle : " + Vector3.Distance(transform.GetComponent<Collider>().ClosestPointOnBounds(m_focusedTarget.targetTransform.position), m_focusedTarget.targetTransform.GetComponent<Collider>().ClosestPointOnBounds(transform.position)));
        if (HasTarget())
            return (Vector3.Distance(transform.GetComponent<Collider>().ClosestPointOnBounds(m_focusedTarget.targetTransform.position), m_focusedTarget.targetTransform.GetComponent<Collider>().ClosestPointOnBounds(transform.position)) < CurrentAttackRange) ? true : false;
        else
            return false;
    }

    private void GetTarget()
    {
        if (!HasTarget())
        {
            RaycastHit hit;
            LayerMask layerMask = LayerMask.GetMask("Entity");

            if (Physics.Raycast(transform.position, transform.forward, out hit, m_maxRayLength, layerMask))
            {
                if (hit.transform && hit.transform.GetComponent<ITargetable>() != null && hit.transform.GetComponent<ITargetable>().IsEnemy(EntityTeam, EntityType) && hit.transform?.GetComponent<IDamageable>() != null)
                {
                    m_focusedTarget.targetTransform = hit.transform;
                    m_focusedTarget.targetDamagble = hit.transform.GetComponent<IDamageable>();
                }
            }
        }
    }

    private bool HasTarget()
    {
        return (m_focusedTarget.targetTransform != null && !m_focusedTarget.targetTransform.Equals(null) && m_focusedTarget.targetDamagble != null && !m_focusedTarget.targetDamagble.Equals(null)) ? true : false;
    }

    private void GetInFrontFriend()
    {
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Entity");

        if (Physics.Raycast(transform.position, transform.forward, out hit, m_maxRayLength, layerMask))
        {
            if (hit.transform && hit.transform.GetComponent<ITargetable>() != null && !hit.transform.GetComponent<ITargetable>().IsEnemy(EntityTeam, EntityType))
            {
                m_inFrontFriend = hit.transform;
            }
        }
    }
    
    #region Attack

    public void TakeDamage(float damage)
    {
        m_takeDamageCallback.InvokeIfNotNull(damage);
    }

    private void Attack()
    {
        if (HasTarget())
        {
            if (AttackCooldownDone())
            {
                AttackTarget();
            }
        }
        else
        {
            GetTarget();
        }
    }

    private bool AttackCooldownDone()
    {
        if (m_attackCooldownTimer <= 0.0f)
        {
            ResetCooldown();
            return true;
        }

        DecreaseCooldown(Time.deltaTime);
        return false;
    }

    private void DecreaseCooldown(float decreasingAmount)
    {
        m_attackCooldownTimer -= decreasingAmount;
    }

    private void ResetCooldown()
    {
        m_attackCooldownTimer = CurrentAttackCooldown;
    }

    private void AttackTarget()
    {
        m_focusedTarget.targetDamagble.TakeDamage(CurrentAttack);
    }

    #endregion Attack

    public void Move()
    {
        if ((m_inFrontFriend && Vector3.Distance(transform.GetComponent<Collider>().ClosestPointOnBounds(m_inFrontFriend.position), m_inFrontFriend.GetComponent<Collider>().ClosestPointOnBounds(transform.position)) > m_distBetweenUnits) || !m_inFrontFriend)
            transform.Translate(transform.forward * CurrentMoveSpeed * Time.deltaTime, Space.World);
    }

    private void DestroyUI()
    {
        m_unitUI.DestroyUI();
    }

    public void EntityDie()
    {
        DestroyEntity();
        DestroyUI();
    }
}
