using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public float spawnInterval = 2f;
    public int MaxConcurrentEnemies = 4;

    private float timer;
    private List<GameObject> activeEnemies = new();

    void Update()
    {
        if (!enabled) return;

        // リストからすでに破棄されたnullを掃除
        activeEnemies.RemoveAll(e => e == null);

        // 4体以上なら出現を制限
        if (activeEnemies.Count >= MaxConcurrentEnemies) return;

        // タイマー処理
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        int randomCount = Random.Range(0, 3);
        GameObject enemyPrefab = randomCount switch
        {
            0 => enemyPrefab2,
            _ => enemyPrefab1
        };

        // スポーン位置はこのオブジェクト
        var go = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        activeEnemies.Add(go);
    }
}
