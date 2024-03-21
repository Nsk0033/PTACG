using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBase : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] private Sprite damagedSprite;

    [Header("Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private bool isDamageable;
        
    private Health health;  
    private SpriteRenderer spriteRenderer; 
    private JarReward jarReward;
    new Collider2D collider2D;

    private void Start()
    {
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jarReward = GetComponent<JarReward>();
        collider2D = GetComponent<Collider2D>();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && other.gameObject.layer != 11)		
        {
			TakeDamage();		
        }
		if (other.CompareTag("Arrow") && other.gameObject.layer != 11)		
        {
			TakeDamage();		
        }
		if (other.CompareTag("Melee") && other.gameObject.layer != 11)		
        {
			TakeDamage();		
        }
		if (other.CompareTag("BiggerMelee") && other.gameObject.layer != 11)		
        {
			TakeDamage();		
        }
		if (other.CompareTag("YamatoWeapon") && other.gameObject.layer != 11)		
        {
			TakeDamage();		
        }
    }

    private void TakeDamage()
    {
        health.TakeDamage(damage);

        if (health.CurrentHealth > 0)
        {
            if (isDamageable)
            {
                spriteRenderer.sprite = damagedSprite;
            }
        }
        
        if (health.CurrentHealth <= 0)
        {
            if (isDamageable)
            {
                // Box
                Destroy(gameObject);
            }
            else
            {
                // Jar
                spriteRenderer.sprite = damagedSprite;
                collider2D.enabled = false;   
                jarReward.GiveReward();             
            }
        }
    }
}