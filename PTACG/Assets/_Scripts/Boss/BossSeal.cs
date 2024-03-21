using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSeal : MonoBehaviour
{
    [SerializeField] private GameObject _BossShield;
    private Health _health;
    private bool _shieldBroken;
    private List<GameObject> Crystals = new List<GameObject>();
    private CircleCollider2D _circleCollider;
    //[SerializeField] private Animator FBAniamtor;

    private float previousShield;


    private void Start()
    {
        _health = GetComponent<Health>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _BossShield.SetActive(false);
        previousShield = _health.CurrentShield;
    }

    private void Update()
    {
        SealBroken();
    }

    private void SealBroken()
    {
        _shieldBroken = _health.IsShieldBroken;
        if (_shieldBroken)
        {
            _BossShield.SetActive(true);
            _circleCollider.offset = new Vector2(_circleCollider.offset.x, 0);
        }
        else
            _BossShield.SetActive(false);
    }

    /*private void AnimatorTrigger() 
    {
        float currentShield = _health.CurrentShield;
        if (previousShield < currentShield) 
        {
            FBAniamtor.SetTrigger("SealGetHurt");
        }

        previousShield = currentShield;
        FBAniamtor.SetTrigger("SealGetNoHurt");
    }*/
}
