using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;

    // 外部参照用
    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    private void Start()
    {
        currentHP = maxHP;

        UIManager.Instance.UpdateHPUI();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // 0以下防止
        if(currentHP < 0)
        {
            currentHP = 0;
        }

        Debug.Log("PlayerDamage" + damage);
        Debug.Log("CurrentHP" + currentHP);

        // UI更新
        UIManager.Instance.UpdateHPUI();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("死");

        GameManager.instance.GameOver();
    }
}