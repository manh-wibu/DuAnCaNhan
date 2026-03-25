using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<bool> OnInteractInputChanged;

    private PlayerInput playerInput;

    private ShowObjects showObjects;

    public Vector2 RawMovementInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }

    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }

    public bool RollInput { get; private set; }

    public bool[] AttackInputs { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;

    private void Awake()
    {
        showObjects = FindObjectOfType<ShowObjects>();
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count];
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        CheckJumpInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f) return;

        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f) return;

        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnRollInput(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f) return;

        if (context.started)
        {
            RollInput = true;
        }
    }

    public void NormalAttackInput(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f) return;

        if (context.started)
            AttackInputs[(int)CombatInputs.normal] = true;

        if (context.canceled)
            AttackInputs[(int)CombatInputs.normal] = false;
    }

    public void StrongAttackInput(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f) return;

        if (context.started)
            AttackInputs[(int)CombatInputs.strong] = true;

        if (context.canceled)
            AttackInputs[(int)CombatInputs.strong] = false;
    }

    public void SpecialAttackInput(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f) return;

        if (context.started)
            AttackInputs[(int)CombatInputs.special] = true;

        if (context.canceled)
            AttackInputs[(int)CombatInputs.special] = false;
    }

    public void SkillAttackInput(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f) return;

        if (context.started)
            AttackInputs[(int)CombatInputs.skill] = true;

        if (context.canceled)
            AttackInputs[(int)CombatInputs.skill] = false;
    }

    public void AirAttackInput(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f) return;

        if (context.started)
            AttackInputs[(int)CombatInputs.air] = true;

        if (context.canceled)
            AttackInputs[(int)CombatInputs.air] = false;
    }

    public void DefendInput(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f) return;

        if (context.started)
            AttackInputs[(int)CombatInputs.defend] = true;

        if (context.canceled)
            AttackInputs[(int)CombatInputs.defend] = false;
    }

    public void OnChooseWind(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            showObjects.HandleChoose(0);
        }
    }

    public void OnChooseEarth(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            showObjects.HandleChoose(1);
        }
    }

    public void OnChooseWater(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            showObjects.HandleChoose(2);
        }
    }

    public void OnChooseFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            showObjects.HandleChoose(3);
        }
    }

    public void UseJumpInput() => JumpInput = false;

    public void UseRollInput() => RollInput = false;

    public void UseAttackInput(int i) => AttackInputs[i] = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }
}

public enum CombatInputs
{
    normal,
    strong,
    special,
    air,
    defend,
    skill
}