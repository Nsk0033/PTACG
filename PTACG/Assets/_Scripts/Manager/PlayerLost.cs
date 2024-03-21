using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLost : MonoBehaviour
{
    private Character character;
    private Health health;
    [SerializeField] private GameObject LoseMenu;
    private void Start()
    {
        character = GetComponent<Character>();
        health = GetComponent<Health>();
        LoseMenu.SetActive(false);

    }

    private void FixedUpdate()
    {
        playerDie();
    }

    private void playerDie()
    {
        if (character != null && character.CharacterType == Character.CharacterTypes.Player)
        {
            if (health.CurrentHealth <= 0) 
            {
                LoseMenu.SetActive(true);
                Time.timeScale = LoseMenu.active ? 0 : 1;
            }
        }
        else
            Debug.Log("player not found");
    }
}
