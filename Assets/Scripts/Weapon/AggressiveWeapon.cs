using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressiveWeapon : Weapon
{
    private Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }
    private Movement movement;

    protected AggressiveWeaponDataSO aggressiveWeaponData;

    private List<IDamageable> detectedDamageable = new List<IDamageable>();
    private List<IKnockBackable> detectedKnockBackable = new List<IKnockBackable>();

    [SerializeField] 
    private PlayerData playerData;
    
    private float currentCooldown = 0f;

    protected override void Awake()
    {
        base.Awake();
        if (weaponData == null)
        {
            Debug.LogWarning("WeaponData not assigned - weapon will not deal damage", gameObject);
            return;
        }

        if (weaponData.GetType() == typeof(AggressiveWeaponDataSO))
        {
            aggressiveWeaponData = (AggressiveWeaponDataSO)weaponData;
        }
        else
        {
            Debug.LogError("Wrong data for the weapon - expected AggressiveWeaponDataSO", gameObject);
        }
    }

    public float GetRemainingCooldown()
    {
        return currentCooldown;
    }

    public override void TickCooldown(float deltaTime)
    {
        if (currentCooldown > 0f)
        {
            currentCooldown -= deltaTime;
            if (currentCooldown <= 0f)
            {
                currentCooldown = 0f;
            }
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
        CheckMeleeAttack();
    }

    private void CheckMeleeAttack()
    {
        if (aggressiveWeaponData == null || aggressiveWeaponData.AttackDetails == null || aggressiveWeaponData.AttackDetails.Length == 0)
        {
            return;
        }
        
        // Kiểm tra nếu vẫn còn cooldown
        if (currentCooldown > 0f)
        {
            return;
        }
        
        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[0];

        // Kiểm tra xem có lấy được Stats không để trừ manaCost
        Stats stats = null;
        if (core != null)
        {
            stats = core.GetCoreComponent(ref stats);
        }
        
        // Nếu không đủ mana, không attack
        if (stats != null && !stats.HasEnoughMana(details.manaCost))
        {
            return;
        }

        bool hasHit = false;
        foreach (IDamageable item in detectedDamageable.ToList())
        {
            item.Damage(details.damageAmount * playerData.GetDamageMultiplier());
            hasHit = true;
        }
        
        foreach (IKnockBackable item in detectedKnockBackable.ToList())
        {
            item.KnockBack(details.knockbackAngle, details.knockbackStrength, Movement.FacingDirection);
        }
        
        // Trừ manaCost (chỉ cần xài là trừ, không cần trúng enemy)
        if (stats != null)
        {
            stats.ConsumeMana(details.manaCost);
        }
        
        // Nếu attack trúng enemy, cộng mana theo manaGain
        if (hasHit && stats != null && !stats.IsInDecayMode())
        {
            stats.AddMana(details.manaGain);
        }
        
        // Thiết lập cooldown sau khi attack xong
        currentCooldown = details.cooldown;
    }

    public void AddToDetected(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            detectedDamageable.Add(damageable);
        }
        IKnockBackable knockbackable = collision.GetComponent<IKnockBackable>();
        if (knockbackable != null)
        {
            detectedKnockBackable.Add(knockbackable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            detectedDamageable.Remove(damageable);
        }
    }
    
    // Hàm để reset cooldown khi cần (dùng cho ExitWeapon)
    public override void ExitWeapon()
    {
        base.ExitWeapon();
    }

    public override bool IsOnCooldown()
    {
        return currentCooldown > 0f;
    }
}
