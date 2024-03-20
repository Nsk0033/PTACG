using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Detect Haunt", fileName = "DecisionHaunt")]
public class DecisionHaunt : AIDecision
{
    public LayerMask targetMask;

    private Transform PlayerFound;

    public override bool Decide(StateController controller)
    {
        return CheckTarget(controller);
    }

    private bool CheckTarget(StateController controller)
    {

        PlayerFound = GameObject.FindWithTag("Player").gameObject.transform;
        if (PlayerFound != null)
        {
            controller.Target = PlayerFound;

            return true;
        }
        return false;
    }
}
