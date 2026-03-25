using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponAtkData", menuName = "Data/Weapon Data/Weapon Attack Data")]
public class WeaponAtkDataSO : WeaponDataSO
{
    [Header("Attack Data")]
    public AggressiveWeaponDataSO aggressiveData;
    [SerializeField]
    private PlayerData playerData;

    [Header("Animator Controllers")]
    public RuntimeAnimatorController baseAnimatorController;
    public RuntimeAnimatorController swordAnimatorController;
}
