using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimation : MonoBehaviour
{
    public float detectArea = 13f;
    public LayerMask targetMask;
	private string currentState;
	private Collider2D targetCollider2D;
	private Animator animator;
	private bool canAtk = true;
	
	const string SKE_Idle = "Skeleton_Idle";
	const string SKE_Die = "Skeleton_Die";
	const string SKE_Attack = "Skeleton_Attack";
	const string SKE_Walk = "Skeleton_Walk";
	
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
			ChangeAnimationState(SKE_Walk);
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
