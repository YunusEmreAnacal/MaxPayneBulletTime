using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    [Header("NPC Ayarlarý")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Referanslar")]
    public Animator animator; // NPC'nin Animator'u
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (animator == null)
            animator = GetComponent<Animator>();
    }


    
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        currentHealth = 0;
        animator.SetTrigger("Die");
    }

}
