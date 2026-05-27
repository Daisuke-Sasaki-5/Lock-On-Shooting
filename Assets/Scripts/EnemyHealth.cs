using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemyの最大HP")]
    [SerializeField] private int maxHP = 100;

    private int currentHP;

    // 外部参照用
    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    public void Start()
    {
        currentHP = maxHP;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log("Hit Count : " + currentHP);

       if(currentHP <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 死亡処理\
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void Die()
    {
        GameManager.instance.AddScore(100);

        Destroy(gameObject);
    }
}
