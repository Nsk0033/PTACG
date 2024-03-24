using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Character playableCharacter;
    [SerializeField] private Transform spawnPosition;

    public Transform Boss { get; set; }
    public Transform Player { get; set; }

    private float timer;
    protected override void Awake()
    {
        if (Boss != null)
        {
            Boss = GameObject.Find("Boss").gameObject.transform;

        }
        else
        {
            return;
        }
        Player = playableCharacter.transform;
        Camera2D.Instance.Target = playableCharacter.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ReviveCharacter();
        }
    }

    private void ReviveCharacter()
    {
        if (playableCharacter.GetComponent<Health>().CurrentHealth <= 0)
        {
            playableCharacter.GetComponent<Health>().Revive();
            playableCharacter.transform.position = spawnPosition.position;
        }
    }
}