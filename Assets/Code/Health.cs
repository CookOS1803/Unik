using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    public void TakeHealing(int healing)
    {
        currentHealth = Mathf.Clamp(currentHealth + healing, currentHealth, maxHealth);
    }
}
