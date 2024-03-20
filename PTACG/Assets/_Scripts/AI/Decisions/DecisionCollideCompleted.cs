using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Collide Completed", fileName = "DecisionCollideCompleted")]
public class DecisionCollideCompleted : AIDecision
{
    public override bool Decide(StateController controller)
    {
        return CollideCompleted(controller);
    }

    private bool CollideCompleted(StateController controller)
    {
        if (controller.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length
        > controller.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            return true;
        }

        return false;
    }
}