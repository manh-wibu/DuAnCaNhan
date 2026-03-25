using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newHitStateData", menuName = "Data/State Data/Hit State")]
public class D_HitState : ScriptableObject
{
    public float hitTime = 3f;

    public float hitKnockbackTime = 0.2f;
    public float hitKnockbackSpeed = 20f;
    public Vector2 hitKnockbackAngle;
}
