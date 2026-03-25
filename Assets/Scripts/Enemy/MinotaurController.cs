using UnityEngine;

public class MinotaurController : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private Vector2 knockbackForce;

    private float currentHealth;
    private float knockbackStartTime;

    private bool isKnockback;

    private Rigidbody2D rb;
    private Animator anim;

    private int damageDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isKnockback && Time.time >= knockbackStartTime + knockbackDuration)
        {
            isKnockback = false;
            rb.velocity = Vector2.zero;
        }
    }

    // hàm này phải giống IDamageable
    public void Damage(float amount)
    {
        Debug.Log(amount + " damage taken");

        currentHealth -= amount;

        // tìm player để xác định hướng knockback
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        damageDirection = player.position.x < transform.position.x ? 1 : -1;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Knockback();
        }
    }

    private void Knockback()
    {
        isKnockback = true;
        knockbackStartTime = Time.time;

        rb.velocity = new Vector2(knockbackForce.x * damageDirection, knockbackForce.y);

        if (anim != null)
            anim.SetTrigger("Knockback");
    }

    private void Die()
    {
        if (anim != null)
            anim.SetTrigger("Dead");

        Destroy(gameObject, 1.5f);
    }
}