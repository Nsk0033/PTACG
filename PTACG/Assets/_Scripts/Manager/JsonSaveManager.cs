using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class JsonSaveManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            OnSaveBtnClick();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            OnLoadBtnClick();
        }
    }

    public void OnSaveBtnClick() 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerData playerData = new PlayerData();
            playerData.currentPosition = player.transform.position;
            playerData.currentHealth = player.GetComponent<Health>().CurrentHealth;
            playerData.currentHealth = player.GetComponent<Health>().CurrentShield;
            playerData.currentMoney = CoinManager.Instance.Coins.ToString();
            //playerData.currentScene = 

            playerData.isBowUpgraded = player.GetComponent<CharacterWeapon>().IsBowUpgraded;
            playerData.isSwordUpgraded = player.GetComponent<CharacterWeapon>().IsSwordUpgraded;
            playerData.isStaffOwned = player.GetComponent<CharacterWeapon>().IsStaffOwned;
            playerData.isYamatoOwned = player.GetComponent<CharacterWeapon>().IsYamatoOwned;

            string jsonWrite = JsonUtility.ToJson(playerData);
            Debug.Log("json write = " + jsonWrite);

            File.WriteAllText(Application.dataPath + "/saveFile.json", jsonWrite);
        }
    }

    public void OnLoadBtnClick() 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (File.Exists(Application.dataPath + "/saveFile.json"))
        {
            string jsonRead = File.ReadAllText(Application.dataPath + "/saveFile.json");
            Debug.Log("json read = " + jsonRead);
            
            PlayerData playerLoaded = JsonUtility.FromJson<PlayerData>(jsonRead);
            player.transform.localPosition = playerLoaded.currentPosition;
            player.GetComponent<Health>().CurrentHealth = playerLoaded.currentHealth;
            player.GetComponent<Health>().CurrentShield = playerLoaded.currentShield;
            //current scene
            CoinManager.Instance.Coins = int.Parse(playerLoaded.currentMoney);

            player.GetComponent<CharacterWeapon>().IsBowUpgraded = playerLoaded.isBowUpgraded;
            player.GetComponent<CharacterWeapon>().IsSwordUpgraded = playerLoaded.isSwordUpgraded;
            player.GetComponent<CharacterWeapon>().IsStaffOwned = playerLoaded.isStaffOwned;
            player.GetComponent<CharacterWeapon>().IsYamatoOwned = playerLoaded.isYamatoOwned;
        }
    }

    private class PlayerData 
    {
        //Player information
        public Vector2 currentPosition;
        public float currentHealth;
        public float currentShield;
        public string currentMoney;
        public string currentScene;

        //weapon information
        public bool isBowUpgraded;
        public bool isSwordUpgraded;
        public bool isStaffOwned;
        public bool isYamatoOwned;

        //boss information

    }

}
