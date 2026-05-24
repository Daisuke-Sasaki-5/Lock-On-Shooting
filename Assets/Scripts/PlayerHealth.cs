using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int health = 100;
    [SerializeField] private int currentHP;

    private void Start()
    {
        currentHP = health;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log("PlayerDamage" + damage);
        Debug.Log("CurrentHP" + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Ž€");
    }
}