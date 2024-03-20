using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMeleeAtk : Weapon
{
    [SerializeField] private float attackDelay = 0.5f;

    private CircleCollider2D damageAreaCollider2D;
    private Animator animator;
    private bool attacking;

    private readonly int useMeleeAtk = Animator.StringToHash("Use Melee Atk");
    
    private void Start()
    {
        damageAreaCollider2D = GetComponentInChildren <CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
    }
    
    public override void UseWeapon()
    {
        StartCoroutine(Attack());
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //FlipMeeleWeapon();
    }

    private IEnumerator Attack()
    {
        if (attacking)
        {
            yield break;
        }

        // Attack
        attacking = true;
        damageAreaCollider2D.enabled = true;
        animator.SetTrigger(useMeleeAtk);

        // Stop Attack
        yield return new WaitForSeconds(attackDelay);
        damageAreaCollider2D.enabled = false;
        attacking = false;
    }

    /*private void FlipMeeleWeapon()
    {
        if (WeaponOwner.GetComponent<CharacterFlip>().FacingRight)
        {
            transform.GetChild(0).localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
        }
    }*/
}
