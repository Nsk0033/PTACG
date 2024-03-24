using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Reflection;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class Vendor : MonoBehaviour
{
    [Header("Panels")]
	[SerializeField] private GameObject popUpPanel;
	[SerializeField] private GameObject shopPanel;
	[SerializeField] private GameObject gachaWinPanel;
    

    [Header("Ads")]
    [SerializeField] private GameObject adPanel;
    [SerializeField] private GameObject watchAdButton;
    [SerializeField] private GameObject ad1;
    [SerializeField] private GameObject ad2;

    [Header("Items")]
    //[SerializeField] private VendorItem weaponItem;
    [SerializeField] private VendorItem healthItem;
    [SerializeField] private VendorItem shieldItem;

    [Header("Settings")]
    [SerializeField] [Range(0,100)] private float chanceToDrop = 5f;
    [SerializeField] private Transform placeToDrop;
	[SerializeField] private int gachaCounter = 0;
	[SerializeField] private GameObject player;
	
	[Header("Rewards")]
    [SerializeField] private GameObject[] rewards;
    
	private bool weaponOwned;
    public bool canOpenShop;
    private CharacterWeapon characterWeapon;
    private VideoPlayer videoPlayer;
    SoundManager soundManager;

    private void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
    private void Update()
    {
        if (canOpenShop)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                shopPanel.SetActive(true);
                watchAdButton.SetActive(true);
                popUpPanel.SetActive(false);
            }
        }

        if (shopPanel.activeInHierarchy)
        {
            //BuyItems();
        }
    }
	
	public void BuyHealthPotion()
	{
		healthItem.healthItem.AddHealth(characterWeapon.GetComponent<Character>());
        ProductBought(healthItem.Cost);
	}
	
	public void BuySheildPotion()
	{
		shieldItem.shieldItem.AddShield(characterWeapon.GetComponent<Character>());
        ProductBought(shieldItem.Cost);
	}
	
	public void BuyGacha()
	{
		ProductBought(20);
		gachaCounter++;
		if (gachaCounter == 5 && !weaponOwned)
		{
			gachaWinPanel.SetActive(true);
			Invoke("CloseGachaTab",3f);
        }
		else
		{
			float probability = Random.Range(0, 100);
			if (probability > chanceToDrop)
			{
				Instantiate(SelectReward(), placeToDrop.position, Quaternion.identity);
			}
			else if (probability <= chanceToDrop)
			{
				if(!weaponOwned)
				{
					weaponOwned = true;
					CharacterWeapon characterWeapon = player.GetComponent<CharacterWeapon>();
					if (characterWeapon != null)
					{
						characterWeapon.SetIsStaffOwned();
					}
					else
					{
						Debug.LogWarning("CharacterWeapon component not found on the player object.");
					}
					gachaWinPanel.SetActive(true);
					Invoke("CloseGachaTab",3f);
				}
				else
				{
					Instantiate(SelectReward(), placeToDrop.position, Quaternion.identity);
				}
			}
		}
		
	}
	
	private void CloseGachaTab()
	{
		gachaWinPanel.SetActive(false);
	}
	
	private GameObject SelectReward()
	{
		int randomRewardIndex = Random.Range(0, rewards.Length);
		return rewards[randomRewardIndex];
	}

    /*private void BuyItems()
    {

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (CoinManager.Instance.Coins >= shieldItem.Cost)
            {
                //shieldItem.shieldItem.AddShield(characterWeapon.GetComponent<Character>());
                ProductBought(shieldItem.Cost);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (CoinManager.Instance.Coins >= healthItem.Cost)
            {
                //healthItem.healthItem.AddHealth(characterWeapon.GetComponent<Character>());
                ProductBought(healthItem.Cost);
            }
        }
    }*/
    public void WatchAd()
    {
        shopPanel.SetActive(false);
        adPanel.SetActive(true);
        ad1.SetActive(true);
        videoPlayer = ad1.GetComponent<VideoPlayer>();
        videoPlayer.Play();
        soundManager.StopMusic();

        StartCoroutine(AnotherAd());
        StartCoroutine(ResetShopPanel());
        CoinManager.Instance.AddCoins(20);
    }

    IEnumerator AnotherAd()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(15.0f);
        ad1.SetActive(false);
        ad2.SetActive(true);
        videoPlayer = ad2.GetComponent<VideoPlayer>();
        videoPlayer.Play();
    }

    IEnumerator ResetShopPanel()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(30.0f);
        ad2.SetActive(false);
        shopPanel.SetActive(true);
        watchAdButton.SetActive(false);
        soundManager.PlayMusic();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        { 
            characterWeapon = other.GetComponent<CharacterWeapon>();
            canOpenShop = true;
            popUpPanel.SetActive(true);
        }
	}

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            characterWeapon = null;
            canOpenShop = false;
            popUpPanel.SetActive(false); 
            shopPanel.SetActive(false);           
        }
	}

    private void ProductBought(int amount)
    {
        CoinManager.Instance.RemoveCoins(amount);
    }
}