using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToSpawn;
    private BoxCollider2D spawnArea;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int maxSpawnAttempts = 10;
    [SerializeField] private int maxSpawnNumber = 6;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private bool playerInsideCollider = false;

    void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;
            SpawnObjects();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;
            DestroySpawnedObjects();
        }
    }

    private void SpawnObjects()
    {
        if (!playerInsideCollider) return;

        foreach (GameObject obj in objectsToSpawn)
        {
            int numToSpawn = Random.Range(1, maxSpawnNumber); // Randomize the number of objects to spawn

            for (int i = 0; i < numToSpawn; i++)
            {
                // Get random point within the bounds of the box collider
                Vector2 randomPoint = GetRandomPointInsideBoxCollider(spawnArea);

                if (randomPoint != Vector2.zero)
                {
                    // Instantiate the object at the random position
                    GameObject spawnedObject = Instantiate(obj, randomPoint, Quaternion.identity);
                    spawnedObjects.Add(spawnedObject); // Add the spawned object to the list
                }
            }
        }
    }

    private void DestroySpawnedObjects()
    {
        foreach (GameObject spawnedObject in spawnedObjects)
        {
            Destroy(spawnedObject);
        }
        spawnedObjects.Clear(); // Clear the list after destroying the objects
    }

    private Vector2 GetRandomPointInsideBoxCollider(BoxCollider2D collider)
    {
        Vector2 randomPoint = new Vector2(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y)
        );

        int attempts = 0;
        // Attempt to find a valid spawn position within the box collider
        while (IsObstacleBlocking(randomPoint) && attempts < maxSpawnAttempts)
        {
            randomPoint = new Vector2(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                Random.Range(collider.bounds.min.y, collider.bounds.max.y)
            );
            attempts++;
        }

        return attempts < maxSpawnAttempts ? randomPoint : Vector2.zero;
    }

    private bool IsObstacleBlocking(Vector2 point)
    {
        // Check if there's any obstacle (wall) at the given point
        Collider2D hitCollider = Physics2D.OverlapPoint(point, obstacleLayer);
        return hitCollider != null;
    }
}
