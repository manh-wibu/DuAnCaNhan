using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float multiplyDamage = 1f; 

    [SerializeField]
    private float divideDamage = 1f;

    [SerializeField]
    private Animator anim;

    private float currentHealth;

    private GameManager GM;

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void DecreaseHealth(float amount)
    {
        float finalDamage = (amount * multiplyDamage) / divideDamage;
        currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Dead");
        StartCoroutine(DeadCoroutine());
    }

    private IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(0.8f);
        GM.Respawn();
        Destroy(gameObject);
    }
}