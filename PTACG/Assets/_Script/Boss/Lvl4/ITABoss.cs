using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITABoss : MonoBehaviour
{
	const string ITA_Idle = "InTheAbyss_Boss-Idle";
	const string ITA_Walk = "InTheAbyss_Boss-Walk";
	const string ITA_AtkC = "InTheAbyss_Boss-AttackCombo";
	const string ITA_AtkD = "InTheAbyss_Boss-AttackDown";
	const string ITA_Dash = "InTheAbyss_Boss-DashAttack";
	const string ITA_Cast = "InTheAbyss_Boss-Cast";
	const string ITA_Summon = "InTheAbyss_Boss-Summon";
	const string ITA_Respawn = "InTheAbyss_Boss-Respawn";
	const string ITA_Start = "InTheAbyss_Boss-Start";
	
	private string currentState;
	Rigidbody2D rb2d;
	Animator animator;
	SpriteRenderer spriteRenderer;
	private Health bosshealth;
	//private CircleCollider2D circle2d;
	private bool canAttack = true;
	private bool isDead;
	private bool secondPhase;
	private bool respawn;
	private bool canUseSkill = true;
	private float randomAnimationTimer = 0f;
	private float randomAnimationInterval = 0f;
	
	[SerializeField] private float attackDelay = 2.55f;
	[SerializeField] private float bossMoveSpeed = 130f;
	[SerializeField] private float attackRange = 6f;
	[SerializeField] private float stopRange = 5f;
	[SerializeField] private float normalRandomTime = 7f;
	[SerializeField] private float secondPhaseRandomTime = 5f;
	[SerializeField] private float minRandomTime = 2f;
	[SerializeField] private GameObject Aura;
	[SerializeField] private Transform ShootPosition;
	[SerializeField] private Transform CenterPosition;
	[SerializeField] private GameObject Player;
	[SerializeField] private GameObject PlayerEnterCheck;
	[SerializeField] private GameObject darkboltPrefab;
	[SerializeField] private GameObject thunderBulletPrefab;
	[SerializeField] private GameObject darkSparkPrefab;
	[SerializeField] private GameObject slashPrefab;
	[SerializeField] private GameObject explosionPrefab;
	[SerializeField] private GameObject burstPrefab;
	[SerializeField] private GameObject ThunderIcon;
	[SerializeField] private GameObject Demon;
	[SerializeField] private GameObject Memory;
	
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
		if(!respawn && !itabt.hasPlayerEnter)
			ChangeITAAnimationState(ITA_Start);
		else if(!respawn && itabt.hasPlayerEnter)
		{
			Demon.SetActive(true);
			ChangeITAAnimationState(ITA_Respawn);
			Invoke("FinishRespawn",8f);
		}
		else
		{
			// Update the random animation timer
			randomAnimationTimer += Time.deltaTime;

			// Check if it's time to play a random animation
			if (randomAnimationTimer >= randomAnimationInterval && !IsAttacking() && canUseSkill && canAttack && !isDead && itabt.hasPlayerEnter)
			{
				PlayRandomAnimation();
				randomAnimationTimer = 0f; // Reset the timer
			}
			if(bosshealth.CurrentHealth <= 200)
			{
				ThunderIcon.SetActive(true);
				Aura.SetActive(true);
				secondPhase = true;
			}
			if(bosshealth.CurrentHealth <= 0)
			{
				isDead = true;
				Aura.SetActive(false);
				secondPhase = false;
			}
			if (IsNotMoving() && canAttack && !IsAttacking() && !isDead )
			{
				ChangeITAAnimationState(ITA_Idle);
			}
			
			
			if (itabt.hasPlayerEnter && !isDead)
			{
				
				if(withinAtkRange())
				{
					Debug.Log("NormalAtk");
					canAttack = false;
					if(!IsAttacking())
					{
						ChangeITAAnimationState(ITA_AtkC);
						Invoke("FinishAtk",attackDelay);
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
					ChangeITAAnimationState(ITA_Walk);
				}
				
			}
		}
		
		
    }
	
	private void PlayRandomAnimation()
	{
		// Choose a random animation
		int randomAnimation = Random.Range(0, 4); // Assuming you have 3 animations (0, 1, 2)
		string animationToPlay = ITA_AtkD; // Default to idle animation

		switch (randomAnimation)
		{
			case 0:
				animationToPlay = ITA_AtkD;
				break;
			case 1:
				animationToPlay = ITA_AtkC;
				break;
			case 2:
				animationToPlay = ITA_Summon;
				break;
			case 3:
				animationToPlay = ITA_Cast;
				break;
			// Add cases for more animations if needed
		}

		// Play the chosen animation
		ChangeITAAnimationState(animationToPlay);
		Debug.Log("RandomAnimation");
		// Set the interval for the next random animation
		if(!secondPhase)
		{
			randomAnimationInterval = Random.Range(minRandomTime, normalRandomTime);
		}	
		else
		{
			randomAnimationInterval = Random.Range(minRandomTime, secondPhaseRandomTime);
		}
		Invoke("FinishSkill", 1.1f);
	}
	
	private bool IsAttacking()
    {
        // Check if the magnitude of the velocity is close to zero
        if(currentState == ITA_AtkC || currentState == ITA_AtkD)
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
		ChangeITAAnimationState(ITA_Idle);
	}
	
	private void FinishSkill()
	{
		canAttack = true;
		canUseSkill = true;
		ChangeITAAnimationState(ITA_Idle);
	}
	
	private void FinishRespawn()
	{
		Memory.SetActive(false);
		respawn = true;
		ChangeITAAnimationState(ITA_Idle);
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
				SpawnDarkBolt();
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
	
	public void SpawnDarkBolt()
	{
		Vector3 direction = (Player.transform.position - ShootPosition.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		Instantiate(darkboltPrefab, ShootPosition.position, rotation);

		if (secondPhase)
		{
			// Spawn additional darkboltPrefabs at angles +90, +180, and +270 on the Z-axis
			Quaternion rotation90 = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
			Quaternion rotation180 = Quaternion.AngleAxis(angle + 180f, Vector3.forward);
			Quaternion rotation270 = Quaternion.AngleAxis(angle + 270f, Vector3.forward);

			Instantiate(darkboltPrefab, ShootPosition.position, rotation90);
			Instantiate(darkboltPrefab, ShootPosition.position, rotation180);
			Instantiate(darkboltPrefab, ShootPosition.position, rotation270);
		}
	}

	public void SpawnDarkSpark()
	{
		Vector3 direction = (Player.transform.position - ShootPosition.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		Instantiate(darkSparkPrefab, ShootPosition.position, rotation);

		if (secondPhase)
		{
			// Spawn two additional birds, one at +10 degrees and one at -10 degrees from the player direction
			Quaternion rotationPlus10 = Quaternion.AngleAxis(angle + 30f, Vector3.forward);
			Quaternion rotationMinus10 = Quaternion.AngleAxis(angle - 30f, Vector3.forward);
			Instantiate(darkSparkPrefab, ShootPosition.position, rotationPlus10);
			Instantiate(darkSparkPrefab, ShootPosition.position, rotationMinus10);
		}
	}
	
	public void SpawnThunderBullet()
	{
		Vector3 direction = (Player.transform.position - ShootPosition.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		Quaternion rotationPlus10 = Quaternion.AngleAxis(angle + 10f, Vector3.forward);
		Quaternion rotationMinus10 = Quaternion.AngleAxis(angle - 10f, Vector3.forward);
		
		Instantiate(thunderBulletPrefab, ShootPosition.position, rotation);
		Instantiate(thunderBulletPrefab, ShootPosition.position, rotationPlus10);
		Instantiate(thunderBulletPrefab, ShootPosition.position, rotationMinus10);

		if (secondPhase)
		{
			// Spawn two additional birds, one at +10 degrees and one at -10 degrees from the player direction
			Quaternion rotationPlus20 = Quaternion.AngleAxis(angle + 20f, Vector3.forward);
			Quaternion rotationMinus20 = Quaternion.AngleAxis(angle - 20f, Vector3.forward);
			Instantiate(thunderBulletPrefab, ShootPosition.position, rotationPlus20);
			Instantiate(thunderBulletPrefab, ShootPosition.position, rotationMinus20);
		}
	}
	
	public void SpawnExplosion()
	{
		Instantiate(explosionPrefab, CenterPosition.position, Quaternion.identity);
	}
	
	public void SpawnSlash()
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
		Instantiate(slashPrefab, CenterPosition.position, rotation);
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
				float offsetX = Random.Range(-6f, 6f);
				float offsetY = Random.Range(-6f, 6f);

				// Calculate the spawn position around the player
				Vector3 spawnPosition = Player.transform.position + new Vector3(offsetX, offsetY, 0f);

				// Instantiate the burstPrefab at the calculated position
				Instantiate(burstPrefab, spawnPosition, Quaternion.identity);
			}
		}
	}
}