using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NskBossFight : MonoBehaviour
{
    private bool hasTriggered = false;
	private bool gateOpened = false;
	[SerializeField] private GameObject boss;
	[SerializeField] private GameObject canva;
	[SerializeField] private GameObject wall;
	[SerializeField] private GameObject wall1;
	[SerializeField] private GameObject wall2;
	
	
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
			wall.SetActive(true);
			wall1.SetActive(true);
			wall2.SetActive(true);
			
            ResetBossHealth resetBossHealth = boss.GetComponent<ResetBossHealth>();
			if (resetBossHealth != null)
			{
				resetBossHealth.ResetHealth();
				Debug.Log("ResetHealth");
			}
			else
			{
				Debug.LogWarning("ResetBossHealth component not found.");
			}
			
			Lvl4UIManager uiManager = GameObject.FindObjectOfType<Lvl4UIManager>();
            if (uiManager != null)
            {
                uiManager.StartCoroutine(uiManager.NskBossFight());
            }
            else
            {
                Debug.LogWarning("Lvl4UIManager component not found in the scene.");
            }
        }
		
		
    }
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (canva != null)
			{
				canva.SetActive(false);
			}
		}
	}
	
	private void Update()
    {
        // Check if the GameObject to monitor has been destroyed
        if (boss == null && !gateOpened)
        {
            OpenGate();
            gateOpened = true; // Set the flag to true after opening the gate
        }
        if (!boss.activeSelf && !gateOpened)
        {
            OpenGate();
            gateOpened = true; // Set the flag to true after opening the gate
        }
    }
	
	private void OpenGate()
	{
		canva.SetActive(false);
		wall.SetActive(false);
		wall1.SetActive(false);
		wall2.SetActive(false);
		hasTriggered = true; // Mark the trigger as activated
		// GameObject has been destroyed
		Debug.Log("GameObject has been destroyed!");
	}
	
	
}