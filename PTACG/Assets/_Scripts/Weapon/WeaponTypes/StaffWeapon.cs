using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StaffWeapon : Weapon
{
    [SerializeField] private Vector3 projectileSpawnPosition;
    [SerializeField] private Vector3 projectileSpread;

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
        base.RequestShot();

        if (CanShoot)
        {
            EvaluateProjectileSpawnPosition();
            SpawnProjectile(ProjectileSpawnPosition);
        }
    }

    // Spawns a projectile from the pool, setting it's new direction based on the character's direction (WeaponOwner)
    private void SpawnProjectile(Vector2 spawnPosition)
    {
		// Get the mouse position in world space
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		CanShoot = false; 
        // Get Object from the pool
        GameObject projectilePooled = Pooler.GetObjectFromPool();
        projectilePooled.transform.position = mousePosition;
        projectilePooled.SetActive(true);

        // Get reference to the projectile
        Projectile projectile = projectilePooled.GetComponent<Projectile>();
        projectile.EnableProjectile();
		projectile.ProjectileOwner = WeaponOwner;

		// Play shoot sound
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
