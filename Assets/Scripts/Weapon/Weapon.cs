using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponDataSO weaponData;

    private Animator baseAnimator;
    private Animator weaponAnimator;

    protected PlayerAttackState state;

    protected Core core;

    protected virtual void Awake()
    {
        baseAnimator = transform.Find("Base").GetComponent<Animator>();
        weaponAnimator = transform.Find("Sword").GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);
        if (baseAnimator != null && baseAnimator.runtimeAnimatorController != null)
            baseAnimator.SetBool("attack", true);
        if (weaponAnimator != null && weaponAnimator.runtimeAnimatorController != null)
            weaponAnimator.SetBool("attack", true);
    }

    public virtual void ExitWeapon()
    {
        if (baseAnimator != null && baseAnimator.runtimeAnimatorController != null)
            baseAnimator.SetBool("attack", false);
        if (weaponAnimator != null && weaponAnimator.runtimeAnimatorController != null)
            weaponAnimator.SetBool("attack", false);
        gameObject.SetActive(false);
    }

    public virtual bool HasValidAnimators()
    {
        return baseAnimator != null && baseAnimator.runtimeAnimatorController != null 
            && weaponAnimator != null && weaponAnimator.runtimeAnimatorController != null;
    }

    #region Animation Triggers
    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }

    public virtual void AnimationTurnOffFlipTrigger()
    {
        state.SetFlipCheck(false);
    }

    public virtual void AnimationTurnOnFlipTrigger()
    {
        state.SetFlipCheck(true);
    }

    public virtual void AnimationActionTrigger()
    {

    }
    #endregion

    public virtual bool IsOnCooldown()
    {
        return false;
    }

    public virtual void TickCooldown(float deltaTime)
    {
        // Default implementation does nothing
    }

    public void InitializeWeapon(PlayerAttackState state, Core core)
    {
        this.state = state;
        this.core = core;
    }

    public WeaponDataSO GetWeaponData()
    {
        return weaponData;
    }

    public bool HasEnoughManaForAttack(Stats stats)
    {
        AggressiveWeaponDataSO aggressiveData = weaponData as AggressiveWeaponDataSO;
        if (aggressiveData != null && aggressiveData.AttackDetails.Length > 0)
        {
            float manaCost = aggressiveData.AttackDetails[0].manaCost;
            if (stats != null)
            {
                return stats.HasEnoughMana(manaCost);
            }
        }
        return true; // Nếu không phải aggressive weapon, cho phép attack
    }
}
