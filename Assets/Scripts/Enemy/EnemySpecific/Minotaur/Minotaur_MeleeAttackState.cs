using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur_MeleeAttackState : MeleeAttackState
{
    private Minotaur minotaur;

    public Minotaur_MeleeAttackState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Minotaur minotaur) : base(etity, stateMachine, animBoolName, attackPosition, stateData)
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
        if (isAnimationFinished)
        {
            if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(minotaur.playerDetectedState);
            }
            else if (!isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(minotaur.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
    public override void FinishAttack()
    {
        base.FinishAttack();
    }
}
