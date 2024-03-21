using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/ActionSteal", fileName = "ActionSteal")]
public class ActionSteal : AIAction
{
    //[SerializeField] private Animator animator;
    //[SerializeField] private GameObject MoneyBag;

    private void Start() 
    {
        //MoneyBag.SetActive(false);
    }

    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        // Stop
        controller.CharacterMovement.SetHorizontal(0f);
        controller.CharacterMovement.SetVertical(0f);
        //animator.SetTrigger("carry");
        //MoneyBag.SetActive(true);
        // Attack
        //controller.CharacterWeapon.CurrentWeapon.UseWeapon();
    }
}