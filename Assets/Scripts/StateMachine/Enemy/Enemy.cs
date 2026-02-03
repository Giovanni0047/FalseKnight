using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [field: SerializeField]public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public virtual void Die()
    {
        
    }

    public virtual void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
}
