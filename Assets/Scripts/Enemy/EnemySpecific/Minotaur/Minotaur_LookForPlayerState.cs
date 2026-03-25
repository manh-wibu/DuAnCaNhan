using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur_LookForPlayerState : LookForPlayerState
{
    private Minotaur minotaur;

    public Minotaur_LookForPlayerState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayer stateData, Minotaur minotaur) : base(etity, stateMachine, animBoolName, stateData)
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
        else if (isAllTurnsDone)
        {
            stateMachine.ChangeState(minotaur.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
