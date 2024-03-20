using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    //[SerializeField] private GameObject spawnProjectile;
    private ObjectPooler Pooler;
    [SerializeField] private float spawnTime = 0.6f;
    private bool canSpawn = true;
    private Coroutine spawnCoroutine;

    private void Start()
    {
        Pooler = GetComponent<ObjectPooler>();
        StartSpawning();
    }

    private void StartSpawning()
	{
		if (Pooler == null)
		{
			Debug.LogError("Pooler is not assigned in ProjectileSpawn script!");
		}

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
            GameObject projectilePooled = Pooler.GetObjectFromPool();
            projectilePooled.transform.position = transform.position;
            projectilePooled.SetActive(true);

            Projectile projectile = projectilePooled.GetComponent<Projectile>();
            projectile.EnableProjectile();
			Debug.Log("SPAWN!");
			
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