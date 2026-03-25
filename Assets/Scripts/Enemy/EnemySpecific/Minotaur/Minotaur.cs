using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : Entity
{
    public Minotaur_IdleState idleState { get; private set; }
    public Minotaur_MoveState moveState { get; private set; }
    public Minotaur_PlayerDetectedState playerDetectedState { get; private set; }
    public Minotaur_MeleeAttackState meleeAttackState { get; private set; }
    public Minotaur_LookForPlayerState lookForPlayerState { get; private set; }
    public Minotaur_HitState hitState { get; private set; }
    public Minotaur_DeadState deadState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerStateData;
    [SerializeField]
    private D_HitState hitStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    private Stats Stats
    {
        get => stats ?? Core.GetCoreComponent(ref stats);
    }
    private Stats stats;

    private bool isDead;
    public bool IsDead => isDead;

    public override void Awake()
    {
        base.Awake();
        moveState = new Minotaur_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new Minotaur_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new Minotaur_PlayerDetectedState(this, stateMachine, "detected", playerDetectedStateData, this);
        meleeAttackState = new Minotaur_MeleeAttackState(this, stateMachine, "atk", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new Minotaur_LookForPlayerState(this, stateMachine, "look", lookForPlayerStateData, this);
        hitState = new Minotaur_HitState(this, stateMachine, "hit", hitStateData, this);
        deadState = new Minotaur_DeadState(this, stateMachine, "death", this);

        if (atsm != null)
        {
            atsm.attackState = meleeAttackState;
            atsm.deadState = deadState;
        }
    }

    private void Start()
    {
        stateMachine.Initialize(moveState);
        if (Stats != null)
        {
            Stats.OnPoiseZero += OnHit;
        }
    }

    private void OnHit()
    {
        if (isDead) return;
        if (Stats != null && Stats.IsDead())
        {
            Die();
            return;
        }
        stateMachine.ChangeState(hitState);
    }

    public override void Damage(float amount)
    {
        if (!CanReceiveDamage()) return;

        base.Damage(amount);

        if (isDead)
        {
            if (stateMachine.currentState != hitState)
            {
                stateMachine.ChangeState(hitState);
            }
            return;
        }

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            lastDamageDirection = player.position.x < transform.position.x ? 1 : -1;
        }

        stateMachine.ChangeState(hitState);
    }

    public override void Die()
    {
        if (isDead) return;
        isDead = true;
        if (stateMachine.currentState != hitState)
        {
            stateMachine.ChangeState(hitState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
