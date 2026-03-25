using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendWeapon : Weapon
{
    private bool isDefending = false;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public override void EnterWeapon()
    {
        base.EnterWeapon();
        isDefending = true;
    }

    public override void ExitWeapon()
    {
        base.ExitWeapon();
        isDefending = false;
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
    }

    public override void AnimationFinishTrigger()
    {
        if (!isDefending)
        {
            base.AnimationFinishTrigger();
        }
    }

    public bool IsDefending => isDefending;
}
