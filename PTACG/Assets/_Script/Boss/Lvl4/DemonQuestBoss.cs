using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonQuestBoss : MonoBehaviour
{
	const string DQB_Idle = "DemonQuest_boss-idle";
	const string DQB_Walk = "DemonQuest_boss-walk";
	const string DQB_Atk = "DemonQuest_boss-atk";
	const string DQB_Die = "DemonQuest_boss-die";
	const string DQB_Hurt = "DemonQuest_boss-hurt";
	
	private string currentState;
	Rigidbody2D rb2d;
	Animator animator;
	SpriteRenderer spriteRenderer;
	private Health bosshealth;
	private CircleCollider2D circle2d;
	private bool canAttack;
	
	[SerializeField] private float attackDelay = 0.3f;
	[SerializeField] private float bossMoveSpeed = 6f;
	[SerializeField] private GameObject Aura;
	[SerializeField] private Transform ShootPosition;
	[SerializeField] private GameObject Player;
	[SerializeField] private GameObject PlayerEnterCheck;
	
    // Start is called before the first frame update
    void Start()
    {
		
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		bosshealth = GetComponent<Health>();
		circle2d = GetComponent<CircleCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		if(bosshealth.CurrentHealth <= 150)
		{
			Aura.SetActive(true);
		}
        if(bosshealth.CurrentHealth <= 0)
		{
			ChangeAnimationState(DQB_Die);
		}
		else if (IsNotMoving())
        {
            ChangeAnimationState(DQB_Idle);
        }
        else
        {
            ChangeAnimationState(DQB_Walk);
        }
		DemonQuestBossFight dqbf = PlayerEnterCheck.GetComponent<DemonQuestBossFight>();
		if (dqbf.hasPlayerEnter)
		{
			// Get the position of the player
			Vector3 playerPosition = Player.transform.position;

			// Calculate the direction to the player
			Vector3 moveDirection = (playerPosition - transform.position).normalized;

			// Set the boss's velocity to move towards the player
			rb2d.velocity = moveDirection * bossMoveSpeed;
			rb2d.AddForce(moveDirection * bossMoveSpeed);

			// Flip the boss sprite if necessary
			if (moveDirection.x < 0)
			{
				// Facing right
				// Flip the sprite horizontally if it's not already facing right
				if (!spriteRenderer.flipX)
				{
					spriteRenderer.flipX = true;
				}
			}
			else if (moveDirection.x > 0)
			{
				// Facing left
				// Flip the sprite horizontally if it's not already facing left
				if (spriteRenderer.flipX)
				{
					spriteRenderer.flipX = false;
				}
			}
		}
		
    }
	
	private bool IsNotMoving()
    {
        // Check if the magnitude of the velocity is close to zero
        return Mathf.Approximately(rb2d.velocity.sqrMagnitude, 0f);
    }
	
	private void OnTriggerEnter2D(Collider2D other)
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
    }
	
	private void FinishAtk()
	{
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
}