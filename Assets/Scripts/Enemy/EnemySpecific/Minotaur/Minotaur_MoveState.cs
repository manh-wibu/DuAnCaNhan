using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur_MoveState : MoveState
{
    private Minotaur minotaur;

    public Minotaur_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Minotaur minotaur) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.minotaur = minotaur;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(minotaur.playerDetectedState);
        }
        else if (isDetectingWall || !isDetectingLedge)
        {
            minotaur.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(minotaur.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
