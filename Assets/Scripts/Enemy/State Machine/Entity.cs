using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Entity : MonoBehaviour, IDamageable, IKnockBackable
{
    protected Movement Movement
    {
        get => movement ?? Core.GetCoreComponent(ref movement);
    }
    private Movement movement;

    public FiniteStateMachine stateMachine;

    public D_Entity entityData; 

    public Animator anim { get; private set; }
    public AnimationToStatemachine atsm { get; private set; }
    public int lastDamageDirection { get; set; }
    public Core Core { get; private set; }

    private bool canReceiveDamage = true;
    private Coroutine damageRecoveryCoroutine;

    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    private Transform groundCheck;

    private float currentHealth;
    private float currentHitResistance;
    private float lastDamageTime;

    public Vector2 velocityWorkspace;

    protected bool isHit;

    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();
        currentHealth = entityData.maxHealth;
        currentHitResistance = entityData.hitResistance; 
        anim = GetComponent<Animator>();
        atsm = GetComponent<AnimationToStatemachine>();
        stateMachine = new FiniteStateMachine();    
    }

    public void SetCanReceiveDamage(bool canReceive)
    {
        canReceiveDamage = canReceive;

        Stats stats = Core.GetCoreComponent<Stats>();
        if (stats != null)
        {
            stats.SetCanReceiveDamage(canReceive);
        }
    }

    public bool CanReceiveDamage() => canReceiveDamage;

    public void StartDamageRecovery(float duration)
    {
        if (damageRecoveryCoroutine != null)
        {
            StopCoroutine(damageRecoveryCoroutine);
        }
        damageRecoveryCoroutine = StartCoroutine(DamageRecoveryCoroutine(duration));
    }

    private IEnumerator DamageRecoveryCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        SetCanReceiveDamage(true);
    }

    public virtual void Damage(float amount)
    {
        if (!canReceiveDamage) return;

        currentHealth -= amount;
        Stats stats = Core.GetCoreComponent<Stats>();
        if (stats != null)
        {
            stats.DecreaseHealth(amount);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public virtual void Die()
    {
        // can override in derived entity classes
    }

    public virtual void KnockBack(Vector2 angle, float strenght, int direction)
    {
        Movement?.SetVelocity(strenght, angle, direction);
    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
        anim.SetFloat("yVelocity", Movement.RB.velocity.y);
        if (Time.time >= lastDamageTime + entityData.hitRecoveryTime)
        {
            ResetHitResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(Movement.RB.velocity.x, velocity);
        Movement.RB.velocity = velocityWorkspace;
    }

    public virtual void ResetHitResistance()
    {
        isHit = false;
        currentHitResistance = entityData.hitResistance;
    }

    public virtual void OnDrawGizmos()
    {
        if (Core != null)
        {
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * entityData.wallCheckDistance));
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));

            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance), 0.2f);
        }
    }
}
