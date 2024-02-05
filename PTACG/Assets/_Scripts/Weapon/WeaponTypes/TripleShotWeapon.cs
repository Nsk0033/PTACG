using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TripleShotWeapon : Weapon
{
    [SerializeField] private Vector3 projectileSpawnPosition;
    [SerializeField] private Vector3 projectileSpread;
	[SerializeField] private float projectileSpreadAngle;

    // Controls the position of our projectile spawn
    public Vector3 ProjectileSpawnPosition { get; set; }

    // Returns the reference to the pooler in this GameObject
    public ObjectPooler Pooler { get; set; }

    private Vector3 projectileSpawnValue;
    private Vector3 randomProjectileSpread;

    protected override void Awake()
    {
	  base.Awake();
		
        projectileSpawnValue = projectileSpawnPosition;
        projectileSpawnValue.y = -projectileSpawnPosition.y; 

        Pooler = GetComponent<ObjectPooler>();
    }

    protected override void RequestShot()
    {
		Debug.Log("RequestShot method called");
        base.RequestShot();

        if (CanShoot)
        {
			Debug.Log("Shoot method called");
            EvaluateProjectileSpawnPosition();
            SpawnProjectile(ProjectileSpawnPosition);
        }
    }

    // Spawns a projectile from the pool, setting it's new direction based on the character's direction (WeaponOwner)
    private void SpawnProjectile(Vector2 spawnPosition)
	{
		CanShoot = false;
		// Get Object from the pool
		GameObject projectilePooled;

		// Original rotation
		Quaternion originalRotation = transform.rotation;

		// Spawn the bullets consecutively
		for (int i = -1; i <= 1; i++)
		{
			// Calculate rotation based on i (0 for the original rotation, 1 for +3, -1 for -3)
			float rotationAngle = i * projectileSpreadAngle;
			Quaternion spread = Quaternion.Euler(0f, 0f, rotationAngle);

			// Set direction and rotation
			Vector2 newDirection = WeaponOwner.GetComponent<CharacterFlip>().FacingRight ? spread * transform.right : spread * transform.right * -1;

			// Spawn the projectile
			projectilePooled = Pooler.GetObjectFromPool();
			projectilePooled.transform.position = spawnPosition;
			projectilePooled.SetActive(true);

			// Get reference to the projectile
			Projectile projectile = projectilePooled.GetComponent<Projectile>();
			projectile.EnableProjectile();
			projectile.ProjectileOwner = WeaponOwner;
			projectile.SetDirection(newDirection, originalRotation, WeaponOwner.GetComponent<CharacterFlip>().FacingRight);
		}

		SoundManager.Instance.PlaySound(SoundManager.Instance.ShootClip, 0.8f);

	}

    // Calculates the position where our projectile is going to be fired
    private void EvaluateProjectileSpawnPosition()
    {
        if (WeaponOwner.GetComponent<CharacterFlip>().FacingRight)
        {
            // Right side
            ProjectileSpawnPosition = transform.position + transform.rotation * projectileSpawnPosition;
        }
        else
        {
            // Left side
            ProjectileSpawnPosition = transform.position - transform.rotation * projectileSpawnValue;
        }       
    }

    private void OnDrawGizmosSelected()
    {
        EvaluateProjectileSpawnPosition();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(ProjectileSpawnPosition, 0.1f);
    }
}