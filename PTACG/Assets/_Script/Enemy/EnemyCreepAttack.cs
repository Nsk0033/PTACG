using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreepAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPosition;
    private GameObject player;

    void Start()
    {
        // Find the player GameObject
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
        }
    }

    public void EnemyShoot()
    {
        if (player != null)
        {
            // Calculate the direction towards the player
            Vector3 direction = (player.transform.position - shootPosition.position).normalized;

            // Rotate towards the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Spawn a projectile at shootPosition with the calculated rotation
            Instantiate(projectilePrefab, shootPosition.position, rotation);
        }
    }
	
	public void EnemyShootMultiple()
    {
        if (player != null && shootPosition != null)
		{
			// Calculate the direction towards the player
			Vector3 direction = (player.transform.position - shootPosition.position).normalized;

			// Calculate the base rotation towards the player
			float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			
			// Calculate rotations with offsets of +20 and -20 degrees
			Quaternion rotationPlus20 = Quaternion.AngleAxis(baseAngle + 20f, Vector3.forward);
			Quaternion rotationMinus20 = Quaternion.AngleAxis(baseAngle - 20f, Vector3.forward);

			// Spawn projectiles at shootPosition with the calculated rotations
			Instantiate(projectilePrefab, shootPosition.position, rotationPlus20);
			Instantiate(projectilePrefab, shootPosition.position, rotationMinus20);
		}
    }
}
