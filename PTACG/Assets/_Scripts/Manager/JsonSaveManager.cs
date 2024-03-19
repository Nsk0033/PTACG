using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class JsonSaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //OnSaveBtnClick();
    }

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

            string jsonWrite = JsonUtility.ToJson(playerData);
            Debug.Log("json write = " + jsonWrite);

            File.WriteAllText(Application.dataPath + "/saveFile.json", jsonWrite);
        }
    }

    public void OnLoadBtnClick() 
    {
        GameObject setPlayerPostion = GameObject.FindGameObjectWithTag("Player");

        if (File.Exists(Application.dataPath + "/saveFile.json"))
        {
            string jsonRead = File.ReadAllText(Application.dataPath + "/saveFile.json");
            Debug.Log("json read = " + jsonRead);
            
            PlayerData playerLoaded = JsonUtility.FromJson<PlayerData>(jsonRead);
            setPlayerPostion.transform.localPosition = playerLoaded.currentPosition;
        }
    }

    private class PlayerData 
    {
        //Player information
        public Vector2 currentPosition;
        public int currentHealth;
        public int currentShield;
        public int currentMoney;
        public int currentScene;

        //Player weapon
        //public int currentAmmo;

        //weapon information
        public bool isBowUpgraded;
        public bool isSwordUpgraded;
        public bool isStaffOwned;
        public bool isYamatoOwned;
    }

}
