using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefAnimation : MonoBehaviour
{
    [SerializeField] private float detectArea = 2f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private GameObject Money;
    private Collider2D targetCollider2D;
    private Animator animator;
    private string currentState;

    private bool isSteal;

    const string Tif_Idle = "Idle";
    const string Tif_Carry = "Carry";
    const string Tif_Run = "Run";

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        Money.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (CheckTarget()) 
        {
            if (isSteal == false) 
            {
                int randomNumber = Random.Range(1, 10);
                CoinManager.Instance.Coins -= randomNumber;
                if (CoinManager.Instance.Coins <= 0) 
                {
                    CoinManager.Instance.Coins = 0;
                }
                ChangeAnimationState(Tif_Carry);
                isSteal = true;
            }
            Money.SetActive(true);
        }
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
        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }
}
