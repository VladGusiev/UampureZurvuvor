using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject weakEnemyPrefab;
    [SerializeField] private GameObject StrongEnemyPrefab;
    [SerializeField] private GameObject flyingEnemyPrefab;
    [SerializeField] private GameObject playerObject;

    [Header("Spawning settings")]
    [SerializeField] private float maxSpawningDistance = 50f;
    [SerializeField] private float minSpawningDistance = 10f;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxEnemies = 10;
    private float timer = 0f;

    private GameObject[] allEnemies;
    private GameObject enemyPrefab;

    void Start()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void SpawnEnemy()
    {
        if (weakEnemyPrefab == null || StrongEnemyPrefab == null || playerObject == null) return;
        if (allEnemies.Length >= maxEnemies) return;

        GameObject enemyPrefab = GetRandomEnemyPrefab();

        InstantiateEnemy(enemyPrefab);

    }

    private GameObject GetRandomEnemyPrefab()
    {
        float roll = Random.Range(0f, 100f);
        if (roll <= 40f)
            enemyPrefab = weakEnemyPrefab;
        else if (roll <= 75f)
            enemyPrefab = flyingEnemyPrefab;
        else
            enemyPrefab = StrongEnemyPrefab;
        return enemyPrefab;
    }

    private void InstantiateEnemy(GameObject enemyPrefab)
    {
        EnemyStats enemyStats = enemyPrefab.GetComponent<EnemyStats>();
        float yCoordiante = 0f;

        if(enemyStats.enemyMovementType == EnemyMovementType.Airborne) yCoordiante = 10f;

        Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawningDistance, maxSpawningDistance);
        Vector3 spawnPosition = new Vector3(playerObject.transform.position.x + randomCircle.x, yCoordiante, playerObject.transform.position.z + randomCircle.y);

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
