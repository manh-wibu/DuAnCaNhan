using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : State
{
    private CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }
    private CollisionSenses collisionSenses;

    private Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }
    private Movement movement;

    protected D_HitState stateData;

    protected bool isHitTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;

    public HitState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_HitState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isHitTimeOver = false;
        isMovementStopped = false;
        isGrounded = CollisionSenses.Ground;
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isHitTimeOver = false;
        entity.SetCanReceiveDamage(false);
        Movement?.SetVelocity(stateData.hitKnockbackSpeed, stateData.hitKnockbackAngle, entity.lastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();
        entity.ResetHitResistance();
        entity.StartDamageRecovery(stateData.hitTime + 0.1f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + stateData.hitTime)
        {
            isHitTimeOver = true;
        }
        if (isGrounded && Time.time >= startTime + stateData.hitKnockbackTime && !isMovementStopped)
        {
            isMovementStopped = true;
            Movement?.SetVelocityX(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
