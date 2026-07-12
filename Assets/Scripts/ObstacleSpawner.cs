using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minSpawnInterval = 0.8f;
    [SerializeField] private float spawnIntervalDecreaseRate = 0.05f;

    [SerializeField] private float[] spawnPositionsY = { 0f, 1.5f };

    private float timeSinceLastSpawn = 0f;
    private float currentSpawnInterval;

    private void Start()
    {
        currentSpawnInterval = spawnInterval;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive || GameManager.Instance.IsPaused) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= currentSpawnInterval)
        {
            SpawnObstacle();
            timeSinceLastSpawn = 0f;
            DecreaseSpawnInterval();
        }
    }

    private void SpawnObstacle()
    {
        if (obstaclePrefab == null || spawnPoint == null)
        {
            Debug.LogError("Obstacle Prefab 或 Spawn Point 未设置！");
            return;
        }

        float randomY = spawnPositionsY[Random.Range(0, spawnPositionsY.Length)];
        Vector3 spawnPosition = new Vector3(spawnPoint.position.x, randomY, spawnPoint.position.z);

        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }

    private void DecreaseSpawnInterval()
    {
        currentSpawnInterval = Mathf.Max(
            currentSpawnInterval - spawnIntervalDecreaseRate,
            minSpawnInterval
        );
    }
}
