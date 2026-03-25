using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur_HitState : HitState
{
    private Minotaur minotaur;

    public Minotaur_HitState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_HitState stateData, Minotaur minotaur) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.minotaur = minotaur;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isHitTimeOver)
        {
            if (minotaur.IsDead)
            {
                stateMachine.ChangeState(minotaur.deadState);
            }
            else if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(minotaur.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(minotaur.lookForPlayerState);
            }
        }
    }
}
