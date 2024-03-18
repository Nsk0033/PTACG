using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonQuestBossFight : MonoBehaviour
{
    private bool hasTriggered = false;
	[SerializeField] private GameObject boss;
	[SerializeField] private GameObject canva;
	[SerializeField] private GameObject wall;
	
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            Lvl4UIManager uiManager = GameObject.FindObjectOfType<Lvl4UIManager>();
            if (uiManager != null)
            {
                uiManager.StartCoroutine(uiManager.DemonQuestBossFight());
            }
            else
            {
                Debug.LogWarning("Lvl4UIManager component not found in the scene.");
            }
            hasTriggered = true; // Mark the trigger as activated
        }
    }
	
	private void Update()
    {
        // Check if the GameObject to monitor has been destroyed
        if (boss == null)
        {
			canva.SetActive(false);
			wall.SetActive(false);
            // GameObject has been destroyed
            Debug.Log("GameObject has been destroyed!");
        }
		if (!boss.activeSelf)
        {
			canva.SetActive(false);
			wall.SetActive(false);
            // GameObject has been destroyed
            Debug.Log("GameObject has been destroyed!");
        }
    }
	
	
}
