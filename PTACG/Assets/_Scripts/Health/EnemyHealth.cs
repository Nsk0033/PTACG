﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject enemyHealthBarPrefab;
    [SerializeField] private Vector3 offSet = new Vector3(0f, 1f, 0f);
    [SerializeField] private int staffDamage = 4;
    [SerializeField] private int meleeDamage = 4;
    [SerializeField] private int biggerMeleeDamage = 6;
    [SerializeField] private int arrowDamage = 3;
    [SerializeField] private int yamatoDamage = 1;
	
    
    private Health enemyHealth;
    private Image enemyHealthBar;
    private GameObject enemyBar;
    
    private float enemyCurrentHealth;
    private float enemyMaxHealth;

    private void Start()
    {
        enemyHealth = GetComponent<Health>();
        if (enemyHealthBarPrefab != null)
        {
            enemyBar = Instantiate(enemyHealthBarPrefab, transform.position + offSet, Quaternion.identity);
            enemyBar.transform.parent = transform;
            enemyHealthBar = enemyBar.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        }
    }

    private void Update()
    {
        UpdateHealth();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && other.gameObject.layer != 11)		
        {
			TakeDamage(staffDamage);		
        }
		if (other.CompareTag("Arrow") && other.gameObject.layer != 11)		
        {
			TakeDamage(arrowDamage);		
        }
		if (other.CompareTag("Melee") && other.gameObject.layer != 11)		
        {
			TakeDamage(meleeDamage);		
        }
		if (other.CompareTag("BiggerMelee") && other.gameObject.layer != 11)		
        {
			TakeDamage(biggerMeleeDamage);		
        }
		if (other.CompareTag("YamatoWeapon") && other.gameObject.layer != 11)		
        {
			TakeDamage(yamatoDamage);		
        }
    }

    private void TakeDamage(int damage)
    {
        enemyHealth.TakeDamage(damage);
    }

    private void UpdateHealth()
    {
        if (enemyHealthBar != null)
        {
            enemyHealthBar.fillAmount = Mathf.Lerp(enemyHealthBar.fillAmount, enemyCurrentHealth / enemyMaxHealth, 10f * Time.deltaTime);
        }
    }
    
    public void UpdateEnemyHealth(float currentHealth, float maxHealth)
    {
        enemyCurrentHealth = currentHealth;
        enemyMaxHealth = maxHealth;
    }
}