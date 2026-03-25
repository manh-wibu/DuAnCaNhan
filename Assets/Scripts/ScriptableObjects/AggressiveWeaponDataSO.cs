using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAggressiveWeaponData", menuName = "Data/WeaponData/Aggressive Weapon")]
public class AggressiveWeaponDataSO : WeaponDataSO
{
    [SerializeField] private WeaponAttackDetails[] attackDetails;

    public WeaponAttackDetails[] AttackDetails { get => attackDetails; private set => attackDetails = value; }

    private void OnEnable()
    {
        amountOfAttack = attackDetails.Length;
        movementSpeed = new float[amountOfAttack];
        for (int i = 0; i < amountOfAttack; i++)
        {
            movementSpeed[i] = attackDetails[i].movementSpeed;
        }
    }
}
