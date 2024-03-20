using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DQBoss : MonoBehaviour
{
	const string DQB_Idle = "DemonQuest_boss-idle";
	const string DQB_Walk = "DemonQuest_boss-walk";
	const string DQB_Atk = "DemonQuest_boss-atk";
	const string DQB_Atk1 = "DemonQuest_boss-atk1";
	const string DQB_Die = "DemonQuest_boss-die";
	const string DQB_Hurt = "DemonQuest_boss-hurt";
	
	private string currentState;
	Rigidbody2D rb2d;
	Animator animator;
	SpriteRenderer spriteRenderer;
	private Health bosshealth;
	//private CircleCollider2D circle2d;
	private bool canAttack = true;
	private bool isDead;
	private bool secondPhase;
	private bool canUseSkill = true;
	private float randomAnimationTimer = 0f;
	private float randomAnimationInterval = 0f;
	
	[SerializeField] private float attackDelay = 1.5f;
	[SerializeField] private float bossMoveSpeed = 6f;
	[SerializeField] private float attackRange = 6f;
	[SerializeField] private float stopRange = 5f;
	[SerializeField] private float normalRandomTime = 8f;
	[SerializeField] private float secondPhaseRandomTime = 5f;
	[SerializeField] private float minRandomTime = 3f;
	[SerializeField] private GameObject Aura;
	[SerializeField] private Transform ShootPosition;
	[SerializeField] private GameObject Player;
	[SerializeField] private GameObject PlayerEnterCheck;
	[SerializeField] private GameObject boltPrefab;
	[SerializeField] private GameObject birdPrefab;
	
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
		DemonQuestBossFight dqbf = PlayerEnterCheck.GetComponent<DemonQuestBossFight>();
		// Update the random animation timer
		randomAnimationTimer += Time.deltaTime;

		// Check if it's time to play a random animation
		if (randomAnimationTimer >= randomAnimationInterval && !IsAttacking() && canUseSkill && canAttack && !isDead && dqbf.hasPlayerEnter)
		{
			PlayRandomAnimation();
			randomAnimationTimer = 0f; // Reset the timer
		}
		if(bosshealth.CurrentHealth <= 180)
		{
			Aura.SetActive(true);
			secondPhase = true;
		}
        if(bosshealth.CurrentHealth <= 0)
		{
			isDead = true;
			ChangeAnimationState(DQB_Die);
			Aura.SetActive(false);
			secondPhase = false;
		}
		if (IsNotMoving() && canAttack && !IsAttacking() && !isDead )
        {
            ChangeAnimationState(DQB_Idle);
        }
        
		
		if (dqbf.hasPlayerEnter && !isDead)
		{

			if(withinAtkRange())
			{
				Debug.Log("NormalAtk");
				canAttack = false;
				if(!IsAttacking())
				{
					ChangeAnimationState(DQB_Atk);
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
				ChangeAnimationState(DQB_Walk);
			}
			
		}
		
		/*if(!canAttack)
		{
			if(currentState != DQB_Atk || currentState != DQB_Atk1 )
				ChangeAnimationState(DQB_Atk);
		}*/
		
    }
	
	private void PlayRandomAnimation()
	{
		// Choose a random animation
		int randomAnimation = Random.Range(0, 2); // Assuming you have 3 animations (0, 1, 2)
		string animationToPlay = DQB_Atk1; // Default to idle animation

		switch (randomAnimation)
		{
			case 0:
				animationToPlay = DQB_Atk1;
				break;
			case 1:
				animationToPlay = DQB_Atk;
				break;
			// Add cases for more animations if needed
		}

		// Play the chosen animation
		ChangeAnimationState(animationToPlay);
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
        if(currentState == DQB_Atk || currentState == DQB_Atk1)
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
		ChangeAnimationState(DQB_Idle);
	}
	
	private void FinishSkill()
	{
		canAttack = true;
		canUseSkill = true;
		ChangeAnimationState(DQB_Idle);
	}
	
	//change animation ========================================================================
    private void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }
	
	public void SpawnBolt()
	{
		Vector3 direction = (Player.transform.position - ShootPosition.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		Instantiate(boltPrefab, ShootPosition.position, rotation);

		if (secondPhase)
		{
			// Spawn two additional bolts, one at +10 degrees and one at -10 degrees from the player direction
			Quaternion rotationPlus10 = Quaternion.AngleAxis(angle + 10f, Vector3.forward);
			Quaternion rotationMinus10 = Quaternion.AngleAxis(angle - 10f, Vector3.forward);
			Instantiate(boltPrefab, ShootPosition.position, rotationPlus10);
			Instantiate(boltPrefab, ShootPosition.position, rotationMinus10);
		}
	}

	public void SpawnBird()
	{
		Vector3 direction = (Player.transform.position - ShootPosition.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		Instantiate(birdPrefab, ShootPosition.position, rotation);

		if (secondPhase)
		{
			// Spawn two additional birds, one at +10 degrees and one at -10 degrees from the player direction
			Quaternion rotationPlus10 = Quaternion.AngleAxis(angle + 10f, Vector3.forward);
			Quaternion rotationMinus10 = Quaternion.AngleAxis(angle - 10f, Vector3.forward);
			Instantiate(birdPrefab, ShootPosition.position, rotationPlus10);
			Instantiate(birdPrefab, ShootPosition.position, rotationMinus10);
		}
	}
}