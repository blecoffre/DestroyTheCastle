using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, ITargetable
{
    [SerializeField]
    public HealthBar HealthBar;

    public Action DieAction = null;

    public EntityTypeEnum EntityType;
    public TeamsEnum EntityTeam = TeamsEnum.WHITE;

    [SerializeField]
    private float m_baseHealth;
    public float BaseHealth
    {
        get
        {
            return m_baseHealth;
        }
    }
    public float CurrentHealth;

    

    private void Awake()
    {
        DieAction += DestroyEntity;
    }

    public virtual void Initialize()
    {
        
    }

    public void ClearDieAction()
    {
        DieAction = null;
    }

    public virtual void DestroyEntity()
    {
        if (gameObject)
            Destroy(gameObject);
    }

    public virtual void ApplyDamage(float damage)
    {
        CurrentHealth -= damage;

        HealthBar?.UpdateHealthBar(CurrentHealth / BaseHealth);

        if (IsEntityHealthBelowZero())
            DieAction.InvokeIfNotNull();
    }

    public virtual bool IsEntityHealthBelowZero()
    {
        return CurrentHealth <= 0;
    }

    public bool IsEnemy(TeamsEnum myTeam, EntityTypeEnum myType)
    {
        //bool validTarget = false;
        //if (myType == EntityTypeEnum.Tower && (EntityType == EntityTypeEnum.Flying || EntityType == EntityTypeEnum.FlyingAW || EntityType == EntityTypeEnum.Walking || EntityType == EntityTypeEnum.WalkingAR))
        //{
        //    validTarget = true;
        //}
        //else if (myType == EntityTypeEnum.Walking && (EntityType == EntityTypeEnum.Walking) || EntityType == EntityTypeEnum.WalkingAR || EntityType == EntityTypeEnum.Building)
        //{
        //    validTarget = true;
        //}
        //else if(myType == EntityTypeEnum.WalkingAR && (EntityType == EntityTypeEnum.Flying || EntityType == EntityTypeEnum.FlyingAW || EntityType == EntityTypeEnum.Walking || EntityType == EntityTypeEnum.WalkingAR || EntityType == EntityTypeEnum.Building))
        //{
        //    validTarget = true;
        //}
        //else if (myType == EntityTypeEnum.Flying && (EntityType == EntityTypeEnum.Flying || EntityType == EntityTypeEnum.FlyingAW || EntityType == EntityTypeEnum.Building))
        //{
        //    validTarget = true;
        //}
        //else if (myType == EntityTypeEnum.FlyingAW && (EntityType == EntityTypeEnum.Flying || EntityType == EntityTypeEnum.FlyingAW || EntityType == EntityTypeEnum.Walking || EntityType == EntityTypeEnum.WalkingAR || EntityType == EntityTypeEnum.Building))
        //{
        //    validTarget = true;
        //}

        return ((myTeam != EntityTeam) /*&& validTarget*/);
    }
}
