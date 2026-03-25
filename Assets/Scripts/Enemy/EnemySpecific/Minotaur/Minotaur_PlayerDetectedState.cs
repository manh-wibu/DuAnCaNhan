using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur_PlayerDetectedState : PlayerDetectedState
{
    private Minotaur minotaur;

    public Minotaur_PlayerDetectedState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Minotaur minotaur) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.minotaur = minotaur;
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
        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(minotaur.meleeAttackState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(minotaur.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
