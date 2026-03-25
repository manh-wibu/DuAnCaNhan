using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefendState : PlayerAttackState
{
    private Weapon weapon;
    private int defenseInputIndex = (int)CombatInputs.defend;
    private bool hasTriggeredByMana = false;

    private Stats Stats
    {
        get => stats ?? core.GetCoreComponent(ref stats);
    }
    private Stats stats;

    public PlayerDefendState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
        : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        hasTriggeredByMana = false;

        if (Stats != null)
        {
            Stats.SetDefending(true);
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (Stats != null)
        {
            Stats.SetDefending(false);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Stats != null && Stats.GetCurrentMana() >= 50f)
        {
            if (showObjects == null)
            {
                showObjects = Object.FindObjectOfType<ShowObjects>();
            }
            if (showObjects == null || !showObjects.IsObjectSelected())
            {
                if (Time.timeScale != 0f)
                {
                    Time.timeScale = 0f;
                }
            }
        }
        
        if (!player.InputHandler.AttackInputs[defenseInputIndex])
        {
            isAbilityDone = true;
        }
    }

    public override void AnimationFinishTrigger()
    {
        if (!player.InputHandler.AttackInputs[defenseInputIndex])
        {
            base.AnimationFinishTrigger();
        }
    }
}
