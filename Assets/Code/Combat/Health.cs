using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action onChange;
    public event Action onDeath;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public float normalizedHealth => (float)currentHealth / maxHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        onChange?.Invoke();

        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
            enabled = false;
        }
    }

    public void TakeHealing(int healing)
    {
        currentHealth = Mathf.Clamp(currentHealth + healing, currentHealth, maxHealth);

        onChange?.Invoke();
    }
}
