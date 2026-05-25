using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    [Header("本体HP")]
    [SerializeField] private EnemyHealth enemyHealth;

    [Header("ヘッド判定")]
    [SerializeField] private bool isHead;

    [Header("ダメージ")]
    [SerializeField] private int bodyDamage = 20;

    [Header("ヘッドダメージ")]
    [SerializeField] private int headDamage = 50;

    /// <summary>
    /// 被弾処理
    /// </summary>
    public void Hit()
    {
        if(isHead)
        {
            enemyHealth.TakeDamage(headDamage);
            Debug.Log("ヘッドショット");
        }
        else
        {
            enemyHealth.TakeDamage(bodyDamage);
        }
    }
}
