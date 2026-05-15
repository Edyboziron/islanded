using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Edilecek Nesneler")]
    public List<GameObject> prefabsToSpawn;
    public int spawnCount = 10;

    [Header("Menzil Ayarlarý (Min - Max)")]
    public Vector3 minSpawnRange;
    public Vector3 maxSpawnRange;

    void Start()
    {
        SpawnObjects();
    }

    public void SpawnObjects()
    {
        if (prefabsToSpawn == null || prefabsToSpawn.Count == 0)
        {
            Debug.LogWarning("Spawn listesi boþ! Prefablarý atamayý unutma.");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, prefabsToSpawn.Count);
            GameObject prefabToInstantiate = prefabsToSpawn[randomIndex];

            // x, y, z küçük harf olmalý
            float randomX = Random.Range(minSpawnRange.x, maxSpawnRange.x);
            float randomY = Random.Range(minSpawnRange.y, maxSpawnRange.y);
            float randomZ = Random.Range(minSpawnRange.z, maxSpawnRange.z);
            Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

            Instantiate(prefabToInstantiate, spawnPosition, Quaternion.identity);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = (minSpawnRange + maxSpawnRange) / 2f;
        Vector3 size = new Vector3(
            Mathf.Abs(maxSpawnRange.x - minSpawnRange.x),
            Mathf.Abs(maxSpawnRange.y - minSpawnRange.y),
            Mathf.Abs(maxSpawnRange.z - minSpawnRange.z)
        );
        Gizmos.DrawWireCube(center, size);
    }
}