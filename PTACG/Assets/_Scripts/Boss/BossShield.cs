using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour
{
    [SerializeField] private GameObject _BossShield;
    private Health _health;
    private bool _shieldBroken;
    private List<GameObject> Crystals = new List<GameObject>();
    private CircleCollider2D _circleCollider;

    private void Start()
    {
        _health = GetComponent<Health>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _BossShield.SetActive(false);
    }

    private void Update()
    {
        SealBroken();
        CrystalBreak();
    }

    private void findCrystals() 
    {
        GameObject[] findCrystals = GameObject.FindGameObjectsWithTag("Crystal");
        foreach (GameObject crystalsFound in findCrystals) 
        {
            Crystals.Add(crystalsFound);
        }
        Debug.Log("Crystal List = " + Crystals.Count);
    }

    private void CrystalBreak() 
    {

        if (_BossShield.active)
        {
            SpriteRenderer _spriteRenderer = _BossShield.GetComponent<SpriteRenderer>();
            if (Crystals.Count == 3)
            {
                _spriteRenderer.color = Color.yellow;
            }
            else if (Crystals.Count == 2)
            {
                _spriteRenderer.color = new Color(1f, 0.65f, 0f);
            }
            else if (Crystals.Count == 1)
            {
                _spriteRenderer.color = Color.red;
            }
            else if (Crystals.Count == 0)
            {
                _BossShield.SetActive(false);
            }
            else
                return;
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) 
        {
            _health.TakeDamage(1);
        }
    }

}
