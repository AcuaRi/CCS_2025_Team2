using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public string key;            // Key to identify the enemy's type (Ex: "EnemyType1")
    public GameObject enemyPrefab; // Corresponding enemy prefab
}

public class EnemyGenerator : MonoBehaviour
{
    public static EnemyGenerator Instance { get; private set; }

    [Header("Spawn Settings")]
    [Tooltip("The central location where the enemy will be created")]
    public Transform spawnOrigin;
    [Tooltip("Width of generate area")]
    public float spawnWidth = 5f;

    [Header("Enemy Prefab Data")]
    [Tooltip("Register Prefab List")]
    public List<EnemyData> enemyDataList;

    // Internally used dictionary (key -> enemy prefab)
    private Dictionary<string, GameObject> enemyDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        
        enemyDictionary = new Dictionary<string, GameObject>();
        foreach (var data in enemyDataList)
        {
            if (!enemyDictionary.ContainsKey(data.key))
            {
                enemyDictionary.Add(data.key, data.enemyPrefab);
            }
            else
            {
                Debug.LogWarning("Duplicate key found:" + data.key);
            }
        }
    }

    /// <summary>
    /// Create (numOnce) pre-fabs of the specified type (key) at a time, and spawn repeatedly (iterations) times at intervals of (generateInterval) seconds.
    /// </summary>
    /// <param name="enemyKey">Key of the Enemy to be generated (the value registered with the Inspector)</param>
    /// <param name="numOnce">Number of enemies to spawn at once</param>
    /// <param name="generateInterval">Interval (in seconds) that you create</param>
    /// <param name="iterations">Number of iterations</param>
    public void GenerateEnemy(string enemyKey, int numOnce, float generateInterval, int iterations)
    {
        if (!enemyDictionary.ContainsKey(enemyKey))
        {
            Debug.LogError("EnemyGenerator: No enemy prefabs corresponding to the key: " + enemyKey);
            return;
        }

        GameObject enemyPrefab = enemyDictionary[enemyKey];

        StartCoroutine(SpawnEnemyCoroutine(enemyPrefab, numOnce, generateInterval, iterations));
    }

    private IEnumerator SpawnEnemyCoroutine(GameObject enemyPrefab, int numOnce, float generateInterval, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < numOnce; j++)
            {
                float randomX = Random.Range(-spawnWidth / 2f, spawnWidth / 2f);
                Vector3 spawnPos = spawnOrigin.position + new Vector3(randomX, 0f, 0f);
                Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation);
            }
            yield return new WaitForSeconds(generateInterval);
        }
    }
}
