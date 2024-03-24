using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BossDie : MonoBehaviour
{
    [SerializeField]private Transform portal;
    [SerializeField] private Transform coins;
    [SerializeField]private Transform player;
    [SerializeField]private TextMeshProUGUI textMeshPro;

    [Header("Timer Setting")]
    [SerializeField] private float _Timer;
    [SerializeField] private TextMeshProUGUI _TimerUI;

    private void Start()
    {
        _TimerUI.text = null;
    }
    // Update is called once per frame
    void Update()
    {
        if (player.gameObject.activeSelf == false)
        {
            StopAllCoroutines();
            textMeshPro.text = "Press P to resurrect";
            _TimerUI.text = null;
            _Timer = 30;
        }

        else
        {
            StartCoroutine(Countdown30());
            textMeshPro.text = "Survive for 30 seconds";

            _TimerUI.text = "Run!" + "\n" + _Timer.ToString();
            _Timer -= Time.deltaTime;
            if(_Timer <= 0)
            {
                _TimerUI.text = "You have succeeded";
            }
        }
    }

    private IEnumerator Countdown30()
    {
        yield return new WaitForSeconds(30);
        GameObject.Destroy(this.gameObject);
        portal.gameObject.SetActive(true);
        coins.gameObject.SetActive(true);
    }
}
