using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("敵Prefab")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("スポーンポイント")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("スポーン設定")]
    [SerializeField] private float spawnInterval = 2f;

    [Header("最大スポーン数")]
    [SerializeField] private int maxEnemySpawnCount = 12;

    [SerializeField] private Transform player;

    // 1回で何体出すか
    [SerializeField] private int spawnCount = 1;

    private float timer;

    private void Update()
    {
        // ゲームオーバー中は停止
        if (GameManager.instance.IsGameOver) return;

        timer += Time.deltaTime;

        if(timer >= spawnInterval)
        {
            SpawnEnemies();
            timer = 0f;
        }
    }

    private void SpawnEnemies()
    {
        GameObject[] enemis = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemis.Length >= maxEnemySpawnCount)
        {
            return;
        }

            // spawnCount回スポーン
            for (int i = 0; i < spawnCount; i++)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        // スポーンポイントがない場合
        if(spawnPoints.Length == 0)
        {
            Debug.Log("スポーンポイント未設定");
            return;
        }

        // ランダム選択
        int index = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform point = spawnPoints[index];

        // プレイヤー方向
        Vector3 direction = player.position - point.position;

        // Y軸だけ回転
        direction.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(direction);

        // 敵生成
        GameObject enemy = Instantiate(enemyPrefab, point.position, rotation);

        // EnemyAI取得
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

        // player設定
        if(enemyAI != null )
        {
            enemyAI.player = player;
        }
    }
}
