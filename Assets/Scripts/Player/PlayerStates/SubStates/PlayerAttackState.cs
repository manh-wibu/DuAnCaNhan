using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;

    private int xInput;
    private float velocityToSet;
    private bool setVelocity;
    private bool shouldCheckFlip;
    protected ShowObjects showObjects;
    
    private Stats Stats
    {
        get => stats ?? core.GetCoreComponent(ref stats);
    }
    private Stats stats;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        if (showObjects == null)
        {
            showObjects = Object.FindObjectOfType<ShowObjects>();
        }
        
        isAbilityDone = false;
        
        if (weapon != null && weapon.IsOnCooldown())
        {
            Debug.Log($"Weapon còn cooldown, không thể attack. Weapon: {weapon.name}");
            isAbilityDone = true;
            return;
        }
        
        // Kiểm tra mana giống như cooldown
        if (weapon != null && Stats != null)
        {
            AggressiveWeaponDataSO weaponData = weapon.GetWeaponData() as AggressiveWeaponDataSO;
            if (weaponData != null && weaponData.AttackDetails.Length > 0)
            {
                float manaCost = weaponData.AttackDetails[0].manaCost;
                if (!Stats.HasEnoughMana(manaCost))
                {
                    Debug.Log($"Mana không đủ! Cần: {manaCost}, Hiện có: {Stats.GetCurrentMana()}");
                    isAbilityDone = true;
                    return;
                }
            }
        }
        
        Debug.Log("Weapon sẵn sàng, attack!");
        setVelocity = false;
        weapon.EnterWeapon();
    }

    public override void Exit()
    {
        base.Exit();    
        weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (weapon != null && weapon.IsOnCooldown())
        {
            return;
        }
        
        // Kiểm tra mana giống như cooldown
        if (weapon != null && Stats != null)
        {
            AggressiveWeaponDataSO weaponData = weapon.GetWeaponData() as AggressiveWeaponDataSO;
            if (weaponData != null && weaponData.AttackDetails.Length > 0)
            {
                float manaCost = weaponData.AttackDetails[0].manaCost;
                if (!Stats.HasEnoughMana(manaCost))
                {
                    return;
                }
            }
        }
        
        xInput = player.InputHandler.NormInputX;
        if (shouldCheckFlip)
        {
            Movement?.CheckIfShouldFlip(xInput);
        }
        if (setVelocity)
        {
            Movement?.SetVelocityX(velocityToSet * Movement.FacingDirection);
            
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this, core);
    }
    
    public void SetPlayerVelocity(float velocity)
    {
        Movement?.SetVelocityX(velocity * Movement.FacingDirection);
        velocityToSet = velocity;   
        setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }

    #region Animation Triggers
    public override void AnimationTrigger()
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
        if (Stats != null && Stats.GetCurrentMana() >= 50f)
        {
            if (showObjects == null)
            {
                showObjects = Object.FindObjectOfType<ShowObjects>();
            }
            if (showObjects == null || !showObjects.IsObjectSelected())
            {
                Time.timeScale = 0f;
            }
        }
    }
    #endregion
}
