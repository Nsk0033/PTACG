using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;	

public class Lvl4UIManager : Singleton<Lvl4UIManager>
{
    [Header("Settings")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image shieldBar;
    [SerializeField] private TextMeshProUGUI currentHealthTMP;
    [SerializeField] private TextMeshProUGUI currentShieldTMP;

    [Header("Weapon")]
    //[SerializeField] private TextMeshProUGUI currentAmmoTMP;
    [SerializeField] private Image weaponImage;

    [Header("Text")] 
    [SerializeField] private TextMeshProUGUI coinsTMP;

    [Header("Boss")] 
	[SerializeField] private Image DemonQuestBossHealth;
	[SerializeField] private Image InTheAbyssBossHealth;
	[SerializeField] private Image NskBossHealth;
    [SerializeField] private GameObject DemonQuestBossHealthBarPanel;
    [SerializeField] private GameObject InTheAbyssBossHealthBarPanel;
    [SerializeField] private GameObject NskBossHealthBarPanel;
	
    private float playerCurrentHealth;
    private float playerMaxHealth;
    private float playerMaxShield;
    private float playerCurrentShield;
    private bool isPlayer;

    private int playerCurrentAmmo;
    private int playerMaxAmmo;

    private float DemonQuestBossCurrentHealth;
    private float InTheAbyssBossCurrentHealth;
    private float NskBossCurrentHealth;
    private float DemonQuestBossMaxHealth;
    private float InTheAbyssBossMaxHealth;
    private float NskBossMaxHealth;

    private void Update()
    {
        InternalUpdate();
    }
    
    public void UpdateHealth(float currentHealth, float maxHealth, float currentShield, float maxShield , bool isThisMyPlayer)
    { 
        playerCurrentHealth = currentHealth;
        playerMaxHealth = maxHealth; 
        playerCurrentShield = currentShield;
        playerMaxShield = maxShield;
        isPlayer = isThisMyPlayer;       
    }

    public void UpdateBossHealth(string bossTag, float currentHealth, float maxHealth)
    {
		switch (bossTag)
		{
			case "DemonQuest":
				DemonQuestBossCurrentHealth = currentHealth;
				DemonQuestBossMaxHealth = maxHealth;
				break;
			case "InTheAbyss":
				InTheAbyssBossCurrentHealth = currentHealth;
				InTheAbyssBossMaxHealth = maxHealth;
				break;
			case "NskBoss":
				NskBossCurrentHealth = currentHealth;
				NskBossMaxHealth = maxHealth;
				break;
			
		}
        
    }

    public void UpdateWeaponSprite(Sprite weaponSprite)
    { 
        weaponImage.sprite = weaponSprite;
        weaponImage.SetNativeSize();
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        playerCurrentAmmo = currentAmmo;
        playerMaxAmmo = maxAmmo;
    }

    private void InternalUpdate()
    {
        if (isPlayer)
        {        
           healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, playerCurrentHealth / playerMaxHealth, 10f * Time.deltaTime);
           currentHealthTMP.text = playerCurrentHealth.ToString() + "/" + playerMaxHealth.ToString(); 

           shieldBar.fillAmount = Mathf.Lerp(shieldBar.fillAmount, playerCurrentShield / playerMaxShield, 10f * Time.deltaTime);
           currentShieldTMP.text = playerCurrentShield.ToString() + "/" + playerMaxShield.ToString();
        }

        // Update Ammo
        //currentAmmoTMP.text = playerCurrentAmmo + " / " + playerMaxAmmo;    

        // Update Coins
        coinsTMP.text = CoinManager.Instance.Coins.ToString(); 

        // Update Boss Health
        DemonQuestBossHealth.fillAmount = Mathf.Lerp(DemonQuestBossHealth.fillAmount, DemonQuestBossCurrentHealth / DemonQuestBossMaxHealth, 10f * Time.deltaTime);
        InTheAbyssBossHealth.fillAmount = Mathf.Lerp(InTheAbyssBossHealth.fillAmount, InTheAbyssBossCurrentHealth / InTheAbyssBossMaxHealth, 10f * Time.deltaTime);
        NskBossHealth.fillAmount = Mathf.Lerp(NskBossHealth.fillAmount, NskBossCurrentHealth / NskBossMaxHealth, 10f * Time.deltaTime);
	}
	
//========================================================================================================
    public IEnumerator DemonQuestBossFight()
    {
        // Show Boss HealthBar
        StartCoroutine(MyLibrary.FadeCanvasGroup(DemonQuestBossHealthBarPanel.GetComponent<CanvasGroup>(), 1f, 0f, () =>
        {
            DemonQuestBossHealthBarPanel.SetActive(true);
            StartCoroutine(MyLibrary.FadeCanvasGroup(DemonQuestBossHealthBarPanel.GetComponent<CanvasGroup>(), 1f, 1f));
			
        }));
		yield return null;
    }

    public void OnDemonQuestBossDead()
    {
        StartCoroutine(MyLibrary.FadeCanvasGroup(DemonQuestBossHealthBarPanel.GetComponent<CanvasGroup>(), 1f, 0f, () =>
        {
            DemonQuestBossHealthBarPanel.SetActive(false);
        }));
    }
	
//========================================================================================================
	public IEnumerator InTheAbyssBossFight()
    {
        // Show Boss HealthBar
        StartCoroutine(MyLibrary.FadeCanvasGroup(InTheAbyssBossHealthBarPanel.GetComponent<CanvasGroup>(), 1f, 0f, () =>
        {
            InTheAbyssBossHealthBarPanel.SetActive(true);
            StartCoroutine(MyLibrary.FadeCanvasGroup(InTheAbyssBossHealthBarPanel.GetComponent<CanvasGroup>(), 1f, 1f));
        }));
		yield return null;
    }

    public void OnInTheAbyssBossDead()
    {
        StartCoroutine(MyLibrary.FadeCanvasGroup(InTheAbyssBossHealthBarPanel.GetComponent<CanvasGroup>(), 1f, 0f, () =>
        {
            InTheAbyssBossHealthBarPanel.SetActive(false);
        }));
    }
	
//========================================================================================================
	public IEnumerator NskBossFight()
    {
        // Show Boss HealthBar
        StartCoroutine(MyLibrary.FadeCanvasGroup(NskBossHealthBarPanel.GetComponent<CanvasGroup>(), 1f, 0f, () =>
        {
            NskBossHealthBarPanel.SetActive(true);
            StartCoroutine(MyLibrary.FadeCanvasGroup(NskBossHealthBarPanel.GetComponent<CanvasGroup>(), 1f, 1f));
        }));
		yield return null;
    }

    public void OnNskBossDead()
    {
        StartCoroutine(MyLibrary.FadeCanvasGroup(NskBossHealthBarPanel.GetComponent<CanvasGroup>(), 1f, 0f, () =>
        {
            NskBossHealthBarPanel.SetActive(false);
        }));
    }
//========================================================================================================
    
    private void OnEventResponse(GameEvent.EventType obj)
    {
        switch (obj)
        {
            case GameEvent.EventType.BossFight:
                //StartCoroutine(BossFight());
                break;
        }
    }
    
    private void OnEnable()
    {
        GameEvent.OnEventFired += OnEventResponse;
        //Health.OnBossDead += OnBossDead;
    }
    
    private void OnDisable()
    {
        GameEvent.OnEventFired -= OnEventResponse;
        //Health.OnBossDead -= OnBossDead;
    }
}