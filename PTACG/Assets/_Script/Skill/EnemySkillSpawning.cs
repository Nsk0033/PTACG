using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillSpawning : MonoBehaviour
{
    [SerializeField] private GameObject spawnProjectile;
    [SerializeField] private float spawnTime = 0.6f;
    private bool canSpawn = true;
    private Coroutine spawnCoroutine;

    private void Start()
    {
        StartSpawning();
    }

    private void StartSpawning()
	{
		if (spawnCoroutine == null)
		{
			spawnCoroutine = StartCoroutine(SpawnProjectiles());
		}
	}

    private void StopSpawning()
    {
        canSpawn = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnProjectiles()
	{
		while (canSpawn)
		{
			Instantiate(spawnProjectile, transform.position, Quaternion.identity);
			
			yield return new WaitForSeconds(spawnTime);
		}
	}

    private void OnEnable()
    {
        StartSpawning();
    }

    private void OnDisable()
    {
        StopSpawning();
    }
}
