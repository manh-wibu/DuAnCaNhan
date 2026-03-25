using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    private bool canReceiveDamage = true;
    private bool isDefending = false;
    private const float DEFEND_DAMAGE_REDUCTION = 0.5f;

    [SerializeField]
    private float maxHealth;
    private float currentHealth;
    [SerializeField]
    private float maxPoise;
    private float currentPoise;

    [SerializeField]
    private float maxMana = 100f;
    private float currentMana = 0f;
    private float manaDecayRate = 0.5f;
    private float manaDecayTimer = 0f;
    private bool isInDecayMode = false;

    [SerializeField]
    private PlayerData playerData;

    public event System.Action<float, float> OnHealthChanged;
    public event System.Action<float, float> OnManaChanged;

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;
        currentPoise = maxPoise;
        currentMana = 0f;
    }

    public void SetCanReceiveDamage(bool canReceive)
    {
        canReceiveDamage = canReceive;
    }

    public void SetDefending(bool defending)
    {
        isDefending = defending;
    }

    public bool CanReceiveDamage() => canReceiveDamage;

    public override void LogicUpdate()
    {
        UpdateManaDecay();
    }

    private void UpdateManaDecay()
    {
        if (currentMana >= maxMana)
        {
            isInDecayMode = true;
            currentMana = maxMana;
        }

        if (isInDecayMode)
        {
            manaDecayTimer += Time.deltaTime;
            if (manaDecayTimer >= 1f)
            {
                currentMana -= manaDecayRate;
                manaDecayTimer = 0f;
                if (currentMana < 0) currentMana = 0f;

                // Cập nhật event mana khi decay
                OnManaChanged?.Invoke(currentMana, maxMana);
            }
        }

        if (currentMana <= 0)
        {
            isInDecayMode = false;
            currentMana = 0f;

            // Nếu đã giảm về 0 thì cập nhật event một lần nữa
            OnManaChanged?.Invoke(currentMana, maxMana);
        }
    }

    public void DecreaseHealth(float amount)
    {
        if (!canReceiveDamage) return;

        float finalDamage = amount;

        if (isDefending)
        {
            finalDamage = amount * DEFEND_DAMAGE_REDUCTION;
            if (currentMana < maxMana)
            {
                AddMana(1f);
            }
        }
        else if (playerData != null)
        {
            float multiplier = playerData.GetDefendMultiplier();
            finalDamage = amount * multiplier;
        }
        currentHealth -= finalDamage;
        if (!isDefending)
        {
            currentPoise = 0;
            OnPoiseZero?.Invoke();
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Health is zero!");
        }
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void DecreaseHealthWithDefend(float amount, float defendMultiplier)
    {
        if (!canReceiveDamage) return;

        float finalDamage = amount * defendMultiplier;
        currentHealth -= finalDamage;
        currentPoise = 0;
        OnPoiseZero?.Invoke();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Health is zero!");
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void IncreasedHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void DecreasePoise(float amount)
    {
        if (!canReceiveDamage) return;

        currentPoise -= amount;

        if (currentPoise <= 0)
        {
            currentPoise = 0;
            Debug.Log("Poise broken!");

            OnPoiseZero?.Invoke();
        }
    }

    public event System.Action OnPoiseZero;

    public void AddMana(float amount)
    {
        // Chỉ cộng mana khi không trong decay mode (chưa đạt 100)
        // Khi mana từ 100 về 0, mới được nạp lại
        if (!isInDecayMode)
        {
            currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
            OnManaChanged?.Invoke(currentMana, maxMana);
        }
    }

    public bool IsInDecayMode() => isInDecayMode;

    public void ConsumeMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana - amount, 0, maxMana);
        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    public float GetCurrentMana() => currentMana;
    public float GetMaxMana() => maxMana;
    public bool HasEnoughMana(float cost) => currentMana >= cost;

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
