using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDash : CharacterComponents    
{
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float projectileSpawnInterval = 0.1f; // Interval for spawning projectiles
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile to spawn

    private bool isDashing;
    private float dashTimer;
    private Vector2 dashOrigin;
    private Vector2 dashDestination;
    private Vector2 newPosition;

	[SerializeField] private CharacterWeapon dashCharacterWeapon;
	
    protected override void Start()
    {
        base.Start();
        characterWeapon = GetComponent<CharacterWeapon>(); // Get the CharacterWeapon component
    }

    protected override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }
    }

    protected override void HandleAbility()
    {
        base.HandleAbility();

        if (isDashing)
        {
            if (dashTimer < dashDuration)
            {
                newPosition = Vector2.Lerp(dashOrigin, dashDestination, dashTimer / dashDuration);
                controller.MovePosition(newPosition);

                // Check if the current weapon is YamatoWeapon and spawn projectiles accordingly
                if (dashCharacterWeapon != null && dashCharacterWeapon.CurrentWeapon != null && dashCharacterWeapon.CurrentWeapon.gameObject.CompareTag("YamatoWeapon"))
                {
                    SpawnProjectiles();
                }

                dashTimer += Time.deltaTime;
            }
            else
            {
                StopDash();    
            }
        }
    }

    private void Dash()
    {
        isDashing = true;
        dashTimer = 0f;
        controller.NormalMovement = false;
        dashOrigin = transform.position;

        dashDestination = transform.position + (Vector3) controller.CurrentMovement.normalized * dashDistance;
    }

    private void StopDash()
    {
        isDashing = false;
        controller.NormalMovement = true;
    }   

    private void SpawnProjectiles()
    {
		// Spawn projectiles at regular intervals during the dash
		if (Time.time % projectileSpawnInterval < Time.deltaTime)
		{
			float randomZ = Random.Range(0f, 360f); // Random Z-axis rotation in degrees
			Vector3 spawnPosition = transform.position + new Vector3(0f, 0f, randomZ); // Calculate spawn position

			Instantiate(projectilePrefab, spawnPosition, Quaternion.Euler(0f, 0f, randomZ));
			// You may need to adjust the position and rotation of the spawned projectile as needed.
		}
	}
}