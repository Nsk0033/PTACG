using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class YamatoWeapon : Weapon
{
    [SerializeField] private Vector3 projectileSpawnPosition;
    [SerializeField] private Vector3 projectileSpread;
    [SerializeField] private GameObject swordSprite;

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
		//anim = GetComponentInChildren<Animator>();
        Pooler = GetComponent<ObjectPooler>();
    }

    protected override void RequestShot()
    {
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
		WeaponSpriteDisappear();
		CanShoot = false;
        // Get the mouse position in world space
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Randomize the projectile rotation on the z-axis
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Get Object from the pool
		GameObject projectilePooled = Pooler.GetObjectFromPool();
		projectilePooled.transform.position = mousePosition;
		projectilePooled.transform.rotation = randomRotation; // Apply random rotation
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
	
	private void WeaponSpriteDisappear()
	{
		swordSprite.SetActive(false);
		Invoke("WeaponSpriteAppear",0.1f);
	}
	
	private void WeaponSpriteAppear()
	{
		swordSprite.SetActive(true);
		
	}
}