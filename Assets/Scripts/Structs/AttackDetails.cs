using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponAttackDetails
{
    public float movementSpeed;
    public float damageAmount;

    public float knockbackStrength;
    public Vector2 knockbackAngle;
    
    public float cooldown;
    public float manaGain; 
    public float manaCost;  
}
