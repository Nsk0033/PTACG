﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl4BossHealth : MonoBehaviour
{   
    public static Action OnBossDead;
 
    [Header("Health")]
    [SerializeField] private float initialHealth = 10f;
    [SerializeField] private float maxHealth = 10f;

    [Header("Shield")] 
    [SerializeField] private float initialShield = 5f;
    [SerializeField] private float maxShield = 5f;

    [Header("Settings")] 
    [SerializeField] private bool destroyObject;
	[SerializeField] private float damageCooldown = 0.75f; // Cooldown period after taking damage
	[SerializeField] private bool canTakeDamage = true; // Flag to control if the player can take damage

    private Character character;
    private CharacterController controller;
    new Collider2D collider2D;
    private SpriteRenderer spriteRenderer;
    private EnemyHealth enemyHealth;
    private BossBaseShot bossBaseShot;

    private bool isPlayer;
    private bool shieldBroken;
	


    // Controls the current health of the object    
    public float CurrentHealth { get; set; }

    // Returns the current health of this character
    public float CurrentShield { get; set; }
    
    private void Awake()
    {
        character = GetComponent<Character>();
        controller = GetComponent<CharacterController>();
        collider2D = GetComponent<Collider2D>();      
        enemyHealth = GetComponent<EnemyHealth>();  
        bossBaseShot = GetComponent<BossBaseShot>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        CurrentHealth = initialHealth;
        CurrentShield = initialShield;

        if (character != null)
        {
            isPlayer = character.CharacterType == Character.CharacterTypes.Player;
        }
         
        UpdateCharacterHealth();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(1);
        }
    }

    // Take the amount of damage we pass in parameters
    public void TakeDamage(int damage)
    {
		if (!canTakeDamage && isPlayer)
        {
            return; // If player cannot take damage, exit the function
        }
		
        if (CurrentHealth <= 0)
        {
            return;
        }

        if (!shieldBroken && character != null && initialShield > 0)
        {
            CurrentShield -= damage;
			StartCoroutine(DamageCooldown()); // Start the cooldown coroutine
            UpdateCharacterHealth();

            if (CurrentShield <= 0)
            {
                shieldBroken = true;
            }
            return;
        }
        
        CurrentHealth -= damage;
		StartCoroutine(DamageCooldown()); // Start the cooldown coroutine
        UpdateCharacterHealth();

        if (CurrentHealth <= 0)
        {
            Die();
        }
		
		
    }
	
	private IEnumerator DamageCooldown()
    {
        canTakeDamage = false; // Player cannot take damage during cooldown
        yield return new WaitForSeconds(damageCooldown); // Wait for the cooldown duration
        canTakeDamage = true; // Enable taking damage again after cooldown
    }

    // Kills the game object
    private void Die()
    {
        if (character != null)
        {
            collider2D.enabled = false;
            spriteRenderer.enabled = false;

            character.enabled = false;
            controller.enabled = false;
        }

        if (bossBaseShot != null)
        {
            OnBossDead?.Invoke();
        }

        if (destroyObject)
        {
            DestroyObject();
        }
    }
    
    // Revive this game object    
    public void Revive()
    {
        if (character != null)
        {
            collider2D.enabled = true;
            spriteRenderer.enabled = true;

            character.enabled = true;
            controller.enabled = true;
        }

        gameObject.SetActive(true);

        CurrentHealth = initialHealth;
        CurrentShield = initialShield;

        shieldBroken = false;
       
        UpdateCharacterHealth();
    }

    public void GainHealth(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        UpdateCharacterHealth();
    }
	
    public void GainShield(int amount)
    {
        CurrentShield = Mathf.Min(CurrentShield + amount, maxShield);
        UpdateCharacterHealth();
    }
	
    // If destroyObject is selected, we destroy this game object
    private void DestroyObject()
    {
        gameObject.SetActive(false);
    }

    private void UpdateCharacterHealth()
    {
        // Update Enemy health
        if (enemyHealth != null && bossBaseShot == null)
        {
            enemyHealth.UpdateEnemyHealth(CurrentHealth, maxHealth);
        }

        // Update Boss health
        if (bossBaseShot != null && character.CharacterType == Character.CharacterTypes.AI)
        {
            Lvl4UIManager.Instance.UpdateBossHealth(gameObject.tag, CurrentHealth, maxHealth);
        }  
      
        // Update Player health
        if (character != null && bossBaseShot == null && character.CharacterType == Character.CharacterTypes.Player)
        {
            UIManager.Instance.UpdateHealth(CurrentHealth, maxHealth, CurrentShield, maxShield, isPlayer);
        }
    }   
}