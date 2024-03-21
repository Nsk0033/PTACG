using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAnimation : MonoBehaviour
{
    public float detectArea = 13f;
    public LayerMask targetMask;
	private string currentState;
	private Collider2D targetCollider2D;
	private Animator animator;
	private bool canAtk = true;
	
	const string SKE_Idle = "Reaper_Idle";
	const string SKE_Die = "Reaper_Death";
	const string SKE_Attack = "Reaper_Attack";
	
	
	void Start()
	{
		animator = GetComponent<Animator>();
	}
	
	void Update()
	{
		if(CheckTarget() && canAtk)
		{
			ChangeAnimationState(SKE_Attack);
			canAtk = false;
			Invoke("FinishAtk",1f);
		}
		else if(!canAtk)
		{
			currentState = SKE_Attack;
		}
		else
			ChangeAnimationState(SKE_Idle);
	}
	
	private bool CheckTarget()
    {
        targetCollider2D = Physics2D.OverlapCircle(transform.position, detectArea, targetMask);
        if (targetCollider2D != null)
        {
            return true;
        }

        return false;		
	}	
	
	private void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }
	
	private void FinishAtk()
	{
		canAtk = true;
		ChangeAnimationState(SKE_Idle);
	}
}
