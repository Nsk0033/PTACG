using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    [SerializeField] private GameObject spawnProjectile;
    [SerializeField] private float spawnTime = 0.6f;
	private bool canSpawn = true;

	
	private void Start()
    {
        StartCoroutine(SpawnProjectiles());
    }
	
	
	private IEnumerator SpawnProjectiles()
    {
        while (canSpawn)
        {
            // Instantiate the projectile at the current GameObject's position and rotation
            Instantiate(spawnProjectile, transform.position, Quaternion.identity);
			Debug.Log("SPAWN!");
            // Wait for 1 second before spawning the next projectile
            yield return new WaitForSeconds(spawnTime);
        }
    }
	
	public void StopSpawning()
    {
        canSpawn = false;
    }
	
	 private void OnEnable()
    {
        StartCoroutine(SpawnProjectiles());
    }

    private void OnDisable()
    {
        StopCoroutine(SpawnProjectiles());
    }
    
}