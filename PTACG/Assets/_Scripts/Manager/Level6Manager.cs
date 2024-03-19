using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Start()
    {
        _TimerUI.text = null;
        health = _Boss.GetComponent<Health>();
        UnlockableWall.SetActive(true);
        MazeEntrance.SetActive(true);
    }

    private void Update()
    {
        UnlockableWallEnabled();
        EscapeFromDungeon();
    }

    private void UnlockableWallEnabled()
    {
        bool bossShieldBroken = health.IsShieldBroken;
        if (bossShieldBroken)
        {
            UnlockableWall.SetActive(false);
        }
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
                //then lost the game
            }
        }
    }
}
