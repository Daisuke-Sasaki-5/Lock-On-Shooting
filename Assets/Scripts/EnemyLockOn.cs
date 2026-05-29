using System;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class EnemyLockOn : MonoBehaviour
{
    [Header("メインカメラ")]
    [SerializeField] private Camera m_Camera;

    [Header("ロックオン可能な最大距離")]
    [SerializeField] private float lockOnDistance = 25f;

    [Header("どのくらい画面中央に近ければ対象にするか")]
    [SerializeField] private float maxLoakRange = 0.4f;

    // 現在ロックしている敵
    public Transform CurrentTarget { get; private set; }

    /// <summary>
    /// 毎フレームターゲット探索
    /// </summary>
    private void Update()
    {
        FindTarget();
    }

    /// <summary>
    /// 画面中央に一番近い敵を探す
    /// </summary>
    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // 一番条件の良かった敵
        Transform bestTarget = null;

        // 比較用
        // 小さいほど画面中央に近い
        float closestDistance = Mathf.Infinity;

        // 敵を1体ずつ確認
        foreach(GameObject enemy in enemies)
        {
            // ==== カメラから敵までの距離 ====
            float distanceToEnemy = Vector3.Distance(m_Camera.transform.position, enemy.transform.position);

            // 遠すぎきる敵は除外
            if (distanceToEnemy > lockOnDistance)
            {
                continue;
            }

            // ==== 敵を画面座標へ変換 ====
            Vector3 viewportPos = m_Camera.WorldToViewportPoint(enemy.transform.position);

            // カメラの後ろにいる敵を除外
            if(viewportPos.z < 0 )
            {
                continue;
            }

            // ==== 画面中央との距離を計算 ====
            Vector2 screenCenter = new Vector2(0.5f, 0.5f);
            Vector2 enemyScreenPos = new Vector2(viewportPos.x, viewportPos.y);

            // 画面中央からどれだけ離れているか
            float centerDistance = Vector2.Distance(screenCenter, enemyScreenPos);

            // 中央から離れすぎていたら除外
            if(centerDistance > maxLoakRange)
            {
                continue;
            }

            // ==== 一番中央に近い敵を保存 ====
            if(centerDistance < closestDistance)
            {
                closestDistance = centerDistance;
                
               EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

                if(enemyAI != null )
                {
                    Debug.Log("ああ" + enemyAI.TargetPoint);
                    bestTarget = enemyAI.TargetPoint;
                }
            }
        }

        // 最終的なターゲット
        CurrentTarget = bestTarget;

        if(CurrentTarget != null)
        {
            Debug.Log(CurrentTarget.name);
        }
    }

    /// <summary>
    /// SceneViewでロック範囲確認
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if(m_Camera == null)
        {
            return;
        }

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(m_Camera.transform.position, lockOnDistance);
    }
}
