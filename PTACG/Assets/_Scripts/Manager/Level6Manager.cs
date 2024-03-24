using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class Level6Manager : MonoBehaviour
{
    //get boss component
    [SerializeField] private GameObject _Boss;
    private Health health;

    [Header("Timer Setting")]
    [SerializeField] private float _Timer;
    [SerializeField] private TextMeshProUGUI _TimerUI;

    [Header("UnlockableWall Setting")]
    [SerializeField] private GameObject UnlockableWall;
    [SerializeField] private GameObject MazeEntrance;
    [SerializeField] private GameObject MainEntrance;

    [Header("Crystal Setting")]
    [SerializeField] private GameObject BossShield;

    [Header("Win and Lose Menu")]
    [SerializeField] private GameObject WinMenu;
    [SerializeField] private GameObject LoseMenu;

    //private BoxCollider2D win;

    private void Start()
    {
        _TimerUI.text = null;
        health = _Boss.GetComponent<Health>();
        UnlockableWall.SetActive(true);
        MazeEntrance.SetActive(true);
        MainEntrance.SetActive(false);
        //win = GetComponent<BoxCollider2D>();
        WinMenu.SetActive(false);
        LoseMenu.SetActive(false);
    }

    private void Update()
    {
        UnlockableWallEnabled();
        EscapeFromDungeon();
        LockMainEntrance(UnlockableWallEnabled());
        CrystalDestroy();
    }

    private bool UnlockableWallEnabled()
    {
        bool bossShieldBroken = health.IsShieldBroken;
        if (bossShieldBroken)
        {
            UnlockableWall.SetActive(false);
            return true;
        }
        else
            return false;
    }

    private void LockMainEntrance(bool UnlockableWallEnabled)
    {
        if (UnlockableWallEnabled)
        {
            MainEntrance.SetActive(true);
        }
        else
            return;
    }

    private void EscapeFromDungeon()
    {
        bool bossDie = health.CurrentHealth <= 0;
        if (bossDie) 
        {
            MazeEntrance.SetActive(false);

            _TimerUI.text = "Run!" + "\n" + _Timer.ToString();
            _Timer -= Time.deltaTime;
            if (_Timer <= 0)
            {
                _Timer = 0;

                //SceneManager.LoadScene(Lose);
                LoseMenu.SetActive(true);
                Time.timeScale = LoseMenu.active ? 0 : 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("someone cum");
        if (other.CompareTag("Player")) 
        {
            //Debug.Log("player cum");
            //SceneManager.LoadScene(Win);
            WinMenu.SetActive(true);
            Time.timeScale = WinMenu.active ? 0 : 1;
        }
    }

    private void CrystalDestroy()
    {
        Debug.Log("health IsShieldBroken" + health.IsShieldBroken);
        if (health.IsShieldBroken)
        {
            GameObject[] crystalsFound = GameObject.FindGameObjectsWithTag("Crystal");

            SpriteRenderer _spriteRenderer = BossShield.GetComponent<SpriteRenderer>();

            if (crystalsFound.Length == 3)
            {
                _spriteRenderer.color = new Color(0.96f, 1f, 0.68f);
            }
            else if (crystalsFound.Length == 2)
            {
                _spriteRenderer.color = new Color(1f, 0.64f, 0.37f);
            }
            else if (crystalsFound.Length == 1)
            {
                _spriteRenderer.color = new Color(1f, 0.09f, 0f);
            }
            else if (crystalsFound.Length == 0)
            {
                //BossShield.SetActive(false);
                Destroy(BossShield);
            }
            else
                return;
            //Debug.Log("crystalsFound.Length: " + crystalsFound.Length);
        }
    }
}
