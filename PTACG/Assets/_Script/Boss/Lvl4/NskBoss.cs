using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NskBoss : MonoBehaviour
{
	const string NSK_Idle = "Nsk_idle";
	const string NSK_Walk = "Nsk_Walk";
	const string NSK_Cast = "Nsk_cast";
	const string NSK_Aoe = "Nsk_aoe";
	const string NSK_SwordCast = "Nsk_swordcast";
	const string NSK100_Atk = "Nsk100_atk";
	const string NSK100_Atkcombo = "Nsk100_atkcombo";
	const string NSK100_Battlecry = "Nsk100_battlecry";
	const string NSK100_Idle = "Nsk100_idle";
	const string NSK100_Walk = "Nsk100_walk";
	const string NSK100_Swordatk = "Nsk100_swordatk";
	
	private string currentState;
	Rigidbody2D rb2d;
	Animator animator;
	SpriteRenderer spriteRenderer;
	private Health bosshealth;
	//private CircleCollider2D circle2d;
	private bool canAttack = true;
	private bool isDead;
	private bool secondPhase;
	private bool thirdPhase;
	private float attackCooldownDuration = 3f;
    private float timeSinceLastAttack = 0f;
	
	private bool canUseSkill = true;
	private float randomAnimationTimer = 0f;
	private float randomAnimationInterval = 0f;
	
	[SerializeField] private float attackDelay = 1.5f;
	[SerializeField] private float bossMoveSpeed = 150f;
	[SerializeField] private float attackRange = 6f;
	[SerializeField] private float stopRange = 5f;
	[SerializeField] private float normalRandomTime = 6f;
	[SerializeField] private float secondPhaseRandomTime = 4.5f;
	[SerializeField] private float minRandomTime = 2f;
	[SerializeField] private GameObject Aura1;
	[SerializeField] private GameObject Aura2;
	[SerializeField] private Transform ShootPosition1;
	//[SerializeField] private Transform ShootPosition2;
	[SerializeField] private Transform CenterPosition;
	[SerializeField] private GameObject Player;
	[SerializeField] private GameObject PlayerEnterCheck;
	[SerializeField] private GameObject darkboltPrefab;
	[SerializeField] private GameObject thunderBulletPrefab;
	[SerializeField] private GameObject darkSparkPrefab;
	[SerializeField] private GameObject slashPrefab1;
	[SerializeField] private GameObject slashPrefab2;
	[SerializeField] private GameObject explosionPrefab;
	[SerializeField] private GameObject burstPrefab;
	[SerializeField] private GameObject ThunderIcon1;
	[SerializeField] private GameObject ThunderIcon2;
	[SerializeField] private GameObject thunderBirdPrefab;
	[SerializeField] private GameObject lightningStrike;
	
	//private Coroutine randomAnimationCoroutine;
	
    // Start is called before the first frame update
    void Start()
    {
		
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		bosshealth = GetComponent<Health>();
		//circle2d = GetComponent<CircleCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		//randomAnimationCoroutine = StartCoroutine(PlayRandomAnimation());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		ITABossTrigger itabt = PlayerEnterCheck.GetComponent<ITABossTrigger>();
		if(!itabt.hasPlayerEnter)
			ChangeITAAnimationState(NSK_Idle);
		if(itabt.hasPlayerEnter)
		{
			// Update the random animation timer
			randomAnimationTimer += Time.deltaTime;

			// Check if it's time to play a random animation
			if (randomAnimationTimer >= randomAnimationInterval && !IsAttacking() && canUseSkill && canAttack && !isDead && itabt.hasPlayerEnter)
			{
				PlayRandomAnimation();
				randomAnimationTimer = 0f; // Reset the timer
			}
			if(bosshealth.CurrentHealth <= 400 && !thirdPhase && bosshealth.CurrentHealth > 250)
			{
				ThunderIcon1.SetActive(true);
				Aura1.SetActive(true);
				secondPhase = true;
			}
			else if(bosshealth.CurrentHealth <= 250 && secondPhase)
			{
				ThunderIcon2.SetActive(true);
				Aura2.SetActive(true);
				thirdPhase = true;
			}
			if(bosshealth.CurrentHealth <= 0)
			{
				isDead = true;
				Aura1.SetActive(false);
				Aura2.SetActive(false);
				secondPhase = false;
				thirdPhase = false;
			}
			if (IsNotMoving() && canAttack && !IsAttacking() && !isDead )
			{
				ChangeITAAnimationState(NSK_Idle);
			}
			
			
			if (itabt.hasPlayerEnter && !isDead)
			{
				
				if(withinAtkRange())
				{
					Debug.Log("NormalAtk");
					canAttack = false;
					if(!IsAttacking())
					{
						if(!secondPhase && !thirdPhase)
						{
							ChangeITAAnimationState(NSK_Cast);
							Invoke("FinishAtk",attackDelay);
						}
						else if(secondPhase && !thirdPhase)
						{
							ChangeITAAnimationState(NSK_SwordCast);
							Invoke("FinishAtk",attackDelay);
						}
						else if(secondPhase && !thirdPhase)
						{
							ChangeITAAnimationState(NSK100_Atkcombo);
							Invoke("FinishAtk",attackDelay);
						}
					}
				}
				else if (outsideAtkRange() && !IsAttacking())
				{
					// Get the position of the player
					Vector3 playerPosition = Player.transform.position;

					// Calculate the direction to the player
					Vector3 moveDirection = (playerPosition - transform.position).normalized;

					// Add a force to the boss in the direction of the player
					rb2d.velocity = moveDirection * bossMoveSpeed;
					rb2d.AddForce(moveDirection * bossMoveSpeed);
					
					
					// Flip the boss sprite if necessary
					if (moveDirection.x < 0)
					{
						// Facing right
						// Invert the sprite's x scale to face left
						if (spriteRenderer.transform.localScale.x > 0)
						{
							spriteRenderer.transform.localScale = new Vector3(-1 * Mathf.Abs(spriteRenderer.transform.localScale.x), spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
						}
					}
					else if (moveDirection.x > 0)
					{
						// Facing left
						// Invert the sprite's x scale to face right
						if (spriteRenderer.transform.localScale.x < 0)
						{
							spriteRenderer.transform.localScale = new Vector3(Mathf.Abs(spriteRenderer.transform.localScale.x), spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
						}
					}
					if(secondPhase && thirdPhase)
						ChangeITAAnimationState(NSK100_Walk);
					else
						ChangeITAAnimationState(NSK_Walk);
				}
				
			}
		}
		if(IsIdle())
		{
			canAttack = true;
			canUseSkill = true;
		}
		if (!canAttack)
        {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= attackCooldownDuration)
            {
                canAttack = true;
                timeSinceLastAttack = 0f;
            }
        }
    }
	
	private void PlayRandomAnimation()
	{
		// Choose a random animation
		if(secondPhase && thirdPhase)
		{
			int randomAnimation = Random.Range(0, 5); // Assuming you have 3 animations (0, 1, 2)
			string animationToPlay = NSK100_Battlecry; // Default to idle animation

			switch (randomAnimation)
			{
				case 0:
					animationToPlay = NSK100_Atk;
					break;
				case 1:
					animationToPlay = NSK100_Atkcombo;
					break;
				case 2:
					animationToPlay = NSK100_Battlecry;
					break;
				case 3:
					animationToPlay = NSK100_Swordatk;
					break;
				case 4:
					animationToPlay = NSK_SwordCast;
					break;
				// Add cases for more animations if needed
			}

			// Play the chosen animation
			ChangeITAAnimationState(animationToPlay);
			Debug.Log("RandomAnimation");
			// Set the interval for the next random animation
			
			randomAnimationInterval = Random.Range(minRandomTime, secondPhaseRandomTime);
			Invoke("FinishSkill", 2.5f);
		}
		else if(secondPhase && !thirdPhase)
		{
			int randomAnimation = Random.Range(0, 4); // Assuming you have 3 animations (0, 1, 2)
			string animationToPlay = NSK_SwordCast; // Default to idle animation

			switch (randomAnimation)
			{
				case 0:
					animationToPlay = NSK_Aoe;
					break;
				case 1:
					animationToPlay = NSK_Cast;
					break;
				case 2:
					animationToPlay = NSK_SwordCast;
					break;
				case 3:
					animationToPlay = NSK100_Atk;
					break;
				// Add cases for more animations if needed
			}

			// Play the chosen animation
			ChangeITAAnimationState(animationToPlay);
			Debug.Log("RandomAnimation");
			// Set the interval for the next random animation
			
			randomAnimationInterval = Random.Range(minRandomTime, secondPhaseRandomTime);
			Invoke("FinishSkill", 2.5f);
		}
		else
		{
			int randomAnimation = Random.Range(0, 3); // Assuming you have 3 animations (0, 1, 2)
			string animationToPlay = NSK_Aoe; // Default to idle animation

			switch (randomAnimation)
			{
				case 0:
					animationToPlay = NSK_Aoe;
					break;
				case 1:
					animationToPlay = NSK_Cast;
					break;
				case 2:
					animationToPlay = NSK_SwordCast;
					break;
				// Add cases for more animations if needed
			}

			// Play the chosen animation
			ChangeITAAnimationState(animationToPlay);
			Debug.Log("RandomAnimation");
			// Set the interval for the next random animation
			
			randomAnimationInterval = Random.Range(minRandomTime, normalRandomTime);
			Invoke("FinishSkill", 2.5f);
		}
	}
	
	private bool IsAttacking()
    {
        // Check if the magnitude of the velocity is close to zero
        if(currentState == NSK_Cast || currentState == NSK_SwordCast || currentState == NSK100_Atkcombo)
			return true;
		else
			return false;
	}
	
	private bool IsIdle()
    {
        // Check if the magnitude of the velocity is close to zero
        if(currentState == NSK100_Idle || currentState == NSK_Idle)
			return true;
		else
			return false;
	}
	
	private bool IsNotMoving()
    {
        // Check if the magnitude of the velocity is close to zero
        return Mathf.Approximately(rb2d.velocity.sqrMagnitude, 0f);
    }
	
	private bool withinAtkRange()
	{
		 // Calculate the distance between the boss and the player
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        return distance <= attackRange;
	}
	
	private bool outsideAtkRange()
	{
		if (canAttack)
		{
			 // Calculate the distance between the boss and the player
			float distance = Vector3.Distance(transform.position, Player.transform.position);
			return distance > stopRange;
		}
		else
			return false;
	}
	
	
	
	/*private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Check if the colliding object is within the CircleCollider2D
            if (circle2d != null && other.Distance(circle2d).isOverlapped)
            {
                // Perform actions when the player enters the circle collider
                Debug.Log("Player entered the boss's aura.");
				ChangeAnimationState(DQB_Atk);
				Invoke("FinishAtk",attackDelay);
            }
        }
		else
			Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), other, true);
    }*/
	
	private void FinishAtk()
	{
		Debug.Log("FinishAtk");
		canAttack = true;
		ChangeITAAnimationState(NSK100_Idle);
	}
	
	private void FinishSkill()
	{
		canAttack = true;
		canUseSkill = true;
		ChangeITAAnimationState(NSK100_Idle);
	}
	
	private void FinishRespawn()
	{
		ChangeITAAnimationState(NSK100_Idle);
	}
	
	//change animation ========================================================================
    private void ChangeITAAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }
	
	public void RandomProjectile()
	{
		int randomNumber = Random.Range(0, 3);
		switch (randomNumber)
		{
			case 0:
				SpawnDarkBoltAndThunderBullet();
				break;
			case 1:
				SpawnDarkSpark();
				break;
			case 2:
				SpawnThunderBullet();
				break;
			// Add cases for more animations if needed
		}
	}
	
	public void SpawnDarkBoltAndThunderBullet()
	{
		Vector3 direction = (Player.transform.position - CenterPosition.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		Instantiate(darkboltPrefab, CenterPosition.position, rotation);
		
		Quaternion rotationPlus10 = Quaternion.AngleAxis(angle + 10f, Vector3.forward);
		Quaternion rotationMinus10 = Quaternion.AngleAxis(angle - 10f, Vector3.forward);
		
		Instantiate(thunderBulletPrefab, CenterPosition.position, rotation);
		Instantiate(thunderBulletPrefab, CenterPosition.position, rotationPlus10);
		Instantiate(thunderBulletPrefab, CenterPosition.position, rotationMinus10);

		if (secondPhase)
		{
			// Spawn additional darkboltPrefabs at angles +90, +180, and +270 on the Z-axis
			Quaternion rotation90 = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
			Quaternion rotation180 = Quaternion.AngleAxis(angle + 180f, Vector3.forward);
			Quaternion rotation270 = Quaternion.AngleAxis(angle + 270f, Vector3.forward);

			Instantiate(darkboltPrefab, CenterPosition.position, rotation90);
			Instantiate(darkboltPrefab, CenterPosition.position, rotation180);
			Instantiate(darkboltPrefab, CenterPosition.position, rotation270);
			
			Quaternion rotationPlus100 = Quaternion.AngleAxis(angle + 100f, Vector3.forward);
			Quaternion rotationMinus80 = Quaternion.AngleAxis(angle + 80f, Vector3.forward);
			
			Instantiate(thunderBulletPrefab, CenterPosition.position, rotation90);
			Instantiate(thunderBulletPrefab, CenterPosition.position, rotationPlus100);
			Instantiate(thunderBulletPrefab, CenterPosition.position, rotationMinus80);
			
			Quaternion rotationPlus190 = Quaternion.AngleAxis(angle + 190f, Vector3.forward);
			Quaternion rotationMinus170 = Quaternion.AngleAxis(angle + 170f, Vector3.forward);
			
			Instantiate(thunderBulletPrefab, CenterPosition.position, rotation180);
			Instantiate(thunderBulletPrefab, CenterPosition.position, rotationPlus190);
			Instantiate(thunderBulletPrefab, CenterPosition.position, rotationMinus170);
			
			Quaternion rotationPlus280 = Quaternion.AngleAxis(angle + 280f, Vector3.forward);
			Quaternion rotationMinus260 = Quaternion.AngleAxis(angle + 260f, Vector3.forward);
			
			Instantiate(thunderBulletPrefab, CenterPosition.position, rotation270);
			Instantiate(thunderBulletPrefab, CenterPosition.position, rotationPlus280);
			Instantiate(thunderBulletPrefab, CenterPosition.position, rotationMinus260);
		
		}
	}

	public void SpawnDarkSpark()
	{
		Vector3 direction = (Player.transform.position - ShootPosition1.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		Instantiate(darkSparkPrefab, ShootPosition1.position, rotation);

		if (secondPhase)
		{
			// Spawn two additional birds, one at +10 degrees and one at -10 degrees from the player direction
			Quaternion rotationPlus10 = Quaternion.AngleAxis(angle + 30f, Vector3.forward);
			Quaternion rotationMinus10 = Quaternion.AngleAxis(angle - 30f, Vector3.forward);
			Instantiate(darkSparkPrefab, ShootPosition1.position, rotationPlus10);
			Instantiate(darkSparkPrefab, ShootPosition1.position, rotationMinus10);
		}
		
		if(secondPhase && thirdPhase)
		{
			Quaternion rotationPlus60 = Quaternion.AngleAxis(angle + 60f, Vector3.forward);
			Quaternion rotationMinus60 = Quaternion.AngleAxis(angle - 60f, Vector3.forward);
			Instantiate(darkSparkPrefab, ShootPosition1.position, rotationPlus60);
			Instantiate(darkSparkPrefab, ShootPosition1.position, rotationMinus60);
		}
	}
	
	public void SpawnThunderBullet()
	{
		Vector3 direction = (Player.transform.position - ShootPosition1.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		Quaternion rotationPlus10 = Quaternion.AngleAxis(angle + 10f, Vector3.forward);
		Quaternion rotationMinus10 = Quaternion.AngleAxis(angle - 10f, Vector3.forward);
		
		Instantiate(thunderBulletPrefab, ShootPosition1.position, rotation);
		Instantiate(thunderBulletPrefab, ShootPosition1.position, rotationPlus10);
		Instantiate(thunderBulletPrefab, ShootPosition1.position, rotationMinus10);

		if (secondPhase)
		{
			// Spawn two additional birds, one at +10 degrees and one at -10 degrees from the player direction
			Quaternion rotationPlus20 = Quaternion.AngleAxis(angle + 20f, Vector3.forward);
			Quaternion rotationMinus20 = Quaternion.AngleAxis(angle - 20f, Vector3.forward);
			Instantiate(thunderBulletPrefab, ShootPosition1.position, rotationPlus20);
			Instantiate(thunderBulletPrefab, ShootPosition1.position, rotationMinus20);
		}
	}
	
	public void SpawnExplosion()
	{
		Instantiate(explosionPrefab, CenterPosition.position, Quaternion.identity);
	}
	
	public void SpawnSlash1()
	{
		// Get the direction the boss is facing
		Vector3 bossDirection = spriteRenderer.transform.localScale.x > 0 ? Vector3.right : Vector3.left;

		// Calculate the rotation angle based on the boss's facing direction
		float angle = Vector3.Angle(Vector3.right, bossDirection);
		if (bossDirection.y < 0)
		{
			angle = 360f - angle;
		}

		// Create a rotation from the angle
		Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

		// Spawn the Slash with the calculated rotation
		Instantiate(slashPrefab1, CenterPosition.position, rotation);
	}
	
	public void SpawnSlash2()
	{
		// Get the direction the boss is facing
		Vector3 bossDirection = spriteRenderer.transform.localScale.x > 0 ? Vector3.right : Vector3.left;

		// Calculate the rotation angle based on the boss's facing direction
		float angle = Vector3.Angle(Vector3.right, bossDirection);
		if (bossDirection.y < 0)
		{
			angle = 360f - angle;
		}

		// Create a rotation from the angle
		Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

		// Spawn the Slash with the calculated rotation
		Instantiate(slashPrefab2, CenterPosition.position, rotation);
	}
	
	public void SpawnBurst()
	{
		if(!secondPhase)
		{
			// Loop to spawn 5 burstPrefabs
			for (int i = 0; i < 5; i++)
			{
				// Generate random offsets for X and Y within ±10 range
				float offsetX = Random.Range(-7f, 7f);
				float offsetY = Random.Range(-7f, 7f);

				// Calculate the spawn position around the player
				Vector3 spawnPosition = Player.transform.position + new Vector3(offsetX, offsetY, 0f);

				// Instantiate the burstPrefab at the calculated position
				Instantiate(burstPrefab, spawnPosition, Quaternion.identity);
			}
		}
		if(secondPhase)
		{
			// Loop to spawn 5 burstPrefabs
			for (int i = 0; i < 10; i++)
			{
				// Generate random offsets for X and Y within ±10 range
				float offsetX = Random.Range(-5f, 5f);
				float offsetY = Random.Range(-5f, 5f);

				// Calculate the spawn position around the player
				Vector3 spawnPosition = Player.transform.position + new Vector3(offsetX, offsetY, 0f);

				// Instantiate the burstPrefab at the calculated position
				Instantiate(burstPrefab, spawnPosition, Quaternion.identity);
			}
		}
		if(secondPhase && thirdPhase)
		{
			// Loop to spawn 5 burstPrefabs
			for (int i = 0; i < 5; i++)
			{
				// Generate random offsets for X and Y within ±10 range
				float offsetX = Random.Range(-3f, 3f);
				float offsetY = Random.Range(-3f, 3f);

				// Calculate the spawn position around the player
				Vector3 spawnPosition = Player.transform.position + new Vector3(offsetX, offsetY, 0f);

				// Instantiate the burstPrefab at the calculated position
				Instantiate(burstPrefab, spawnPosition, Quaternion.identity);
			}
		}
	}
	
	public void EightDirectionSpawn()
	{
		// Loop through 8 directions (45 degrees apart)
		for (int i = 0; i < 8; i++)
		{
			// Calculate the rotation angle for each direction
			float angle = i * 45f;

			// Convert the angle to a Quaternion rotation
			Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

			// Spawn the thunderBirdPrefab with the calculated rotation
			Instantiate(thunderBirdPrefab, CenterPosition.position, rotation);
			if(thirdPhase)
			{
				Instantiate(darkSparkPrefab, CenterPosition.position, rotation);
			}
		}
		
	}
	
	public void RandomDirection()
	{
		int randomNumber = Random.Range(0, 3);
		switch (randomNumber)
		{
			case 0:
				EightDirectionSpawn();
				break;
			case 1:
				SpawnDarkBoltAndThunderBullet();
				break;
			case 2:
				SpawnBurst();
				break;
			// Add cases for more animations if needed
		}
	}

}