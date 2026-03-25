using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, normalAttackRadius, normalAttackDamage;
    [SerializeField]
    private Transform normalAttackHitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;

    private bool gotInput, isAttacking, isNormalAttack;

    private float lastInputTime = Mathf.NegativeInfinity;

    private float[] attackDetails = new float[2];

    private Animator anim;

    private PlayerController PC;
    private PlayerStatus PS;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("can_atk", combatEnabled);
        PC = GetComponent<PlayerController>();
        PS = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (combatEnabled)
            {
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isNormalAttack = true;
                anim.SetBool("normal_atk", isNormalAttack);
                anim.SetBool("isAtk", isAttacking);
            }
        }
        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(normalAttackHitBoxPos.position, normalAttackRadius, whatIsDamageable);
        attackDetails[0] = normalAttackDamage;
        attackDetails[1] = transform.position.x;
        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
        }
    }

    private void FinishAttack()
    {
        isAttacking = false;
        anim.SetBool("isAtk", false);
        anim.SetBool("normal_atk", false);
    }

    private void Damage(float[] attackDetails)
    {
        if (!PC.GetRollStatus())
        {
            int direction;
            PS.DecreaseHealth(attackDetails[0]);
            if (attackDetails[1] < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            PC.Knockback(direction);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(normalAttackHitBoxPos.position, normalAttackRadius);
    }

}
