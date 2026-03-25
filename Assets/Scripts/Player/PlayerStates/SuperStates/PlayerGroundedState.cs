using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    private CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }
    private CollisionSenses collisionSenses;

    protected Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }
    private Movement movement;

    protected int xInput;

    private bool JumpInput;
    private bool isGrounded;
    private bool rollInput;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
        }
    }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetAmountOfJumpsLeft();
        player.RollState.ResetCanRoll(); 
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        JumpInput = player.InputHandler.JumpInput;
        rollInput = player.InputHandler.RollInput;
        
        // Get Stats component
        Stats stats = null;
        if (player.Core != null)
        {
            stats = player.Core.GetCoreComponent(ref stats);
        }
        
        if (player.InputHandler.AttackInputs[(int)CombatInputs.normal])
        {
            Weapon normalWeapon = player.Inventory.weapons[(int)CombatInputs.normal];
            if (normalWeapon != null && !normalWeapon.IsOnCooldown() && normalWeapon.HasEnoughManaForAttack(stats))
            {
                stateMachine.ChangeState(player.NormalAttackState);
            }
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.strong])
        {
            Weapon strongWeapon = player.Inventory.weapons[(int)CombatInputs.strong];
            if (strongWeapon != null && !strongWeapon.IsOnCooldown() && strongWeapon.HasEnoughManaForAttack(stats))
            {
                stateMachine.ChangeState(player.StrongAttackState);
            }
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.special])
        {
            Weapon specialWeapon = player.Inventory.weapons[(int)CombatInputs.special];
            if (specialWeapon != null && !specialWeapon.IsOnCooldown() && specialWeapon.HasEnoughManaForAttack(stats))
            {
                stateMachine.ChangeState(player.SpecialAttackState);
            }
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.skill])
        {
            Weapon skillWeapon = player.Inventory.weapons[(int)CombatInputs.skill];
            if (skillWeapon != null && skillWeapon.HasValidAnimators() && !skillWeapon.IsOnCooldown() && skillWeapon.HasEnoughManaForAttack(stats))
            {
                stateMachine.ChangeState(player.SkillAttackState);
            }
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.defend])
        {
            Weapon defendWeapon = player.Inventory.weapons[(int)CombatInputs.defend];
            if (defendWeapon != null && !defendWeapon.IsOnCooldown() && defendWeapon.HasEnoughManaForAttack(stats))
            {
                stateMachine.ChangeState(player.DefendState);
            }
        }
        else if (JumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if (rollInput && player.RollState.CheckIfCanRoll())
        {
            stateMachine.ChangeState(player.RollState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
