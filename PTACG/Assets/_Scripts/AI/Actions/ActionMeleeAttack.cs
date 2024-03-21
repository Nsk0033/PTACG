using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Melee Attack", fileName = "MeleeAttack")]
public class ActionMeleeAttack : AIAction
{
    //private readonly int useMeeleAtk = Animator.StringToHash("UseMeleeAtk");

    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        // Stop
        controller.CharacterMovement.SetHorizontal(0f);
        controller.CharacterMovement.SetVertical(0f);

        // Attack
        //controller.CharacterWeapon.CurrentWeapon.UseWeapon();

        

    }
}