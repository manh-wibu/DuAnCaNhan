using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerRollState RollState { get; private set; }
    public PlayerDeathState DeathState { get; private set; }
    public PlayerHitState HitState { get; private set; }
    public PlayerAttackState NormalAttackState { get; private set; }
    public PlayerAttackState StrongAttackState { get; private set; }
    public PlayerAttackState SpecialAttackState { get; private set; }
    public PlayerAttackState SkillAttackState { get; private set; }
    public PlayerAttackState AirAttackState { get; private set; }
    public PlayerDefendState DefendState { get; private set; }


    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public CapsuleCollider2D MovementCollider { get; private set; }
    public PlayerInventory  Inventory { get; private set; }
    private Stats Stats
    {
        get => stats ?? Core.GetCoreComponent(ref stats);
    }
    private Stats stats;
    #endregion

    #region Other Variables  
    private Vector2 workspace;
    private bool isDead;
    public bool IsDead => isDead;
    private Coroutine damageRecoveryCoroutine;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        RollState = new PlayerRollState(this, StateMachine, playerData, "roll");
        DeathState = new PlayerDeathState(this, StateMachine, playerData, "death");
        HitState = new PlayerHitState(this, StateMachine, playerData,"hit");
        NormalAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        StrongAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        SpecialAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        SkillAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        AirAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        DefendState = new PlayerDefendState(this, StateMachine, playerData, "attack");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        MovementCollider = GetComponent<CapsuleCollider2D>();
        Inventory = GetComponent<PlayerInventory>();
        NormalAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.normal]);
        StrongAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.strong]);
        SpecialAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.special]);
        AirAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.air]);
        DefendState.SetWeapon(Inventory.weapons[(int)CombatInputs.defend]);
        SkillAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.skill]);
        if (Stats != null)
        {
            Stats.OnPoiseZero += OnHit;
        }

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();
        
        // Cập nhật cooldown cho tất cả weapons
        if (Inventory != null && Inventory.weapons != null)
        {
            foreach (Weapon weapon in Inventory.weapons)
            {
                if (weapon != null)
                {
                    weapon.TickCooldown(Time.deltaTime);
                }
            }
        }
        
        if (!isDead && Stats != null && Stats.IsDead())
        {
            isDead = true;
            StateMachine.ChangeState(HitState);
            return;
        }
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnHit()
    {
        if (!isDead && Stats != null && Stats.IsDead())
        {
            isDead = true;
        }

        SetCanReceiveDamage(false);
        SetDamageRecovery(playerData.hitTime + 0.1f);

        StateMachine.ChangeState(HitState);
    }
    #endregion

    #region Damage Control
    private void SetDamageRecovery(float duration)
    {
        if (damageRecoveryCoroutine != null)
        {
            StopCoroutine(damageRecoveryCoroutine);
        }
        damageRecoveryCoroutine = StartCoroutine(DamageRecoveryCoroutine(duration));
    }

    private IEnumerator DamageRecoveryCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        SetCanReceiveDamage(true);
    }

    private void SetCanReceiveDamage(bool value)
    {
        if (Stats != null)
            Stats.SetCanReceiveDamage(value);
    }
    #endregion

    #region Other Functions
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    #endregion

}
