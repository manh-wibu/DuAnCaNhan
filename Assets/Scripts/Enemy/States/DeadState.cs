using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
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

    protected bool isAnimationFinished;

    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        isAnimationFinished = false;
        Movement?.SetVelocityX(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            entity.gameObject.SetActive(false);
        }
    }

    public void FinishAnimation()
    {
        isAnimationFinished = true;
    }
}