using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class BossSeal : MonoBehaviour
{
    [SerializeField] private GameObject _BossShield;
    private Health _health;
    private bool _shieldBroken;
    private float previousShield;
    private bool sealBreak;


    private void Start()
    {
        _health = GetComponent<Health>();
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

            Debug.Log("Boss Shield status = " + _BossShield.active + " Boss Shield activated = " + sealBreak);

            if (_BossShield.active && _shieldBroken)
            {
                _health.CurrentHealth = 50;
            }
            else if(!_BossShield.active && sealBreak)
            {
                Debug.Log("health become 30");
                _health.CurrentHealth = 100f;
                sealBreak = false;
            }

            Debug.Log("Boss Shield status after = " + _BossShield.active + " Boss Shield activated after = " + sealBreak);
        }
        else
            _BossShield.SetActive(false);
    }
}
