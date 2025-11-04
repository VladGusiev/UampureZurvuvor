using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject weakEnemyPrefab;
    [SerializeField] private GameObject StrongEnemyPrefab;
    [SerializeField] private GameObject playerObject;

    [Header("Spawning settings")]
    [SerializeField] private float maxSpawningDistance = 50f;
    [SerializeField] private float minSpawningDistance = 10f;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxEnemies = 10;
    private float timer = 0f;

    private GameObject[] allEnemies;

    void Start()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void SpawnEnemy()
    {
        if (weakEnemyPrefab == null || StrongEnemyPrefab == null || playerObject == null) return;

        // Randomly choose enemy type to spawn
        GameObject enemyPrefab = (Random.value < 0.7f) ? weakEnemyPrefab : StrongEnemyPrefab;

        // Spawn at random position around player within specified distance, at y=0
        Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawningDistance, maxSpawningDistance);
        Vector3 spawnPosition = new Vector3(playerObject.transform.position.x + randomCircle.x, 1f, playerObject.transform.position.z + randomCircle.y);

        if (allEnemies.Length >= maxEnemies) return;
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }
}
