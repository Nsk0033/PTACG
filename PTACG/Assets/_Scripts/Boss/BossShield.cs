using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour
{
    [SerializeField] private GameObject _BossShield;
    private Health _health;
    private bool _shieldBroken;
    private List<GameObject> Crystals = new List<GameObject>();
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _circleCollider;

    private void Start()
    {
        _BossShield.SetActive(false);
        _health = GetComponent<Health>();
        _shieldBroken = _health.isShieldBroken;
        foreach (Transform childObject in transform) 
        {
            if (childObject.name == "Sprite") 
            {
                _circleCollider = GetComponent<CircleCollider2D>();
            }
        }
    }

    private void Update()
    {
        Debug.Log("BossShield: " +_shieldBroken);
        if (_shieldBroken)
        {
            _BossShield.SetActive(true);
            _circleCollider.offset = new Vector2(_circleCollider.offset.x, 0);
        }
        else
            return;

        CrystalBroken(_BossShield.active);
    }

    private void findCrystals() 
    {
        GameObject[] findCrystals = GameObject.FindGameObjectsWithTag("Crystal");
        foreach (GameObject crystalsFound in findCrystals) 
        {
            Crystals.Add(crystalsFound);
        }
    }

    private void CrystalBroken(bool _BossShieldActivated) 
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) 
        {
            _health.TakeDamage(1);
        }
    }

}
