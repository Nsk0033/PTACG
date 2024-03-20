using System;
using System.Collections;
using System.Collections.Generic;
//using System.Reflection;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class Vendor : MonoBehaviour
{
    [Header("Panels")]
	[SerializeField] private GameObject popUpPanel;
	[SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject adPanel;

    [Header("Ads")]
    [SerializeField] private GameObject watchAdButton;
    [SerializeField] private GameObject ad1;
    [SerializeField] private GameObject ad2;

    [Header("Items")]
    //[SerializeField] private VendorItem weaponItem;
    [SerializeField] private VendorItem healthItem;
    [SerializeField] private VendorItem shieldItem;

    [Header("Conversation Panel")]
    [SerializeField] private GameObject VendorConversationPanel;
    


    public bool canOpenShop;
    private bool ad1played = false;
    private bool ad2played = false;
    private CharacterWeapon characterWeapon;
    private VideoPlayer videoPlayer;

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
		//gacha script here
		Debug.Log("Gacha Start!");
	}

    public void WatchAd()
    {
        shopPanel.SetActive(false);
        adPanel.SetActive(true);
        ad1.SetActive(true);
        videoPlayer = ad1.GetComponent<VideoPlayer>();
        videoPlayer.Play();

        StartCoroutine(AnotherAd());
        StartCoroutine(ResetShopPanel());
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