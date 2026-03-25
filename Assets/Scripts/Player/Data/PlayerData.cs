using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;
    [Range(-1f, 1f)]
    public float movementLevel = 0f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;
    [Range(-1f, 1f)]
    public float jumpLevel = 0f;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 3f;

    [Header("Roll State")]
    public float rollCooldown = 0.5f;
    public float rollTime = 0.2f;
    public float rollVelocity = 20f;
    public float rollDrag = 10f;
    public float rollEndYMultiplier = 0.2f;

    [Header("Hit State")]
    public float hitTime = 2f;

    [Header("Defend")]
    [Range(-1f, 1f)]
    public float defendLevel = 0f;

    [Header("Damage")]
    [Range(-1f, 1f)]
    public float damageLevel = 0f;

    private void OnValidate()
    {
        defendLevel = Mathf.Round(defendLevel * 2f) / 2f;
        movementLevel = Mathf.Round(movementLevel * 2f) / 2f;
        jumpLevel = Mathf.Round(jumpLevel * 2f) / 2f;
    }

    public float GetMovementdMultiplier()
    {
        float x = movementLevel;
        switch (x)
        {
            case -1f: return 0.5f;
            case -0.5f: return 0.75f;
            case 0f: return 1f;
            case 0.5f: return 1.5f;
            case 1f: return 2f;
            default: return 1f;
        }
    }

    public float GetJumpdMultiplier()
    {
        float x = jumpLevel;
        switch (x)
        {
            case -1f: return 0.5f;
            case -0.5f: return 0.75f;
            case 0f: return 1f;
            case 0.5f: return 1.5f;
            case 1f: return 2f;
            default: return 1f;
        }
    }

    public float GetDefendMultiplier()
    {
        float x = defendLevel;
        switch (x)
        {
            case -1f: return 0.5f;
            case -0.5f: return 0.75f;
            case 0f: return 1f;
            case 0.5f: return 1.5f;
            case 1f: return 2f;
            default: return 1f;
        }
    }

    public float GetDamageMultiplier()
    {
        float x = damageLevel;
        switch (x)
        {
            case -1f: return 0.5f;
            case -0.5f: return 0.75f;
            case 0f: return 1f;
            case 0.5f: return 1.5f;
            case 1f: return 2f;
            default: return 1f;
        }
    }
}

