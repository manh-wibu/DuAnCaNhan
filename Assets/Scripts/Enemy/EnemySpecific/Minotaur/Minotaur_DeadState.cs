using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur_DeadState : DeadState
{
    private Minotaur minotaur;

    private Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }
    private Movement movement;

    private CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }
    private CollisionSenses collisionSenses;

    public Minotaur_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Minotaur minotaur)
        : base(entity, stateMachine, animBoolName)
    {
        this.minotaur = minotaur;
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityZero();
        if (Movement != null && Movement.RB != null)
        {
            Movement.RB.velocity = Vector2.zero;
            Movement.RB.gravityScale = 0f;
            Movement.RB.isKinematic = true;
        }
        Collider2D col = minotaur.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement?.SetVelocityZero();
        if (isAnimationFinished)
        {
            Debug.Log("Minotaur chết hoàn toàn");
            minotaur.gameObject.SetActive(false);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}