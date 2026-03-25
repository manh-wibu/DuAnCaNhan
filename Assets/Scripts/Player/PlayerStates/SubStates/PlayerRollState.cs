using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerAbilityState
{
    public bool CanRoll { get; private set; }

    private float lastRollTime;

    public PlayerRollState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseRollInput();
        lastRollTime = Time.time;
        CanRoll = false;
        Movement?.SetVelocityX(playerData.rollVelocity * Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            if (Time.time >= startTime + playerData.rollTime)
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        float newVelocity = Movement.CurrentVelocity.x * (1 - playerData.rollDrag * Time.fixedDeltaTime);
        Movement?.SetVelocityX(newVelocity);
    }

    public bool CheckIfCanRoll()
    {
        return CanRoll && Time.time >= lastRollTime + playerData.rollCooldown;
    }

    public void ResetCanRoll()
    {
        CanRoll = true;
    }
}