using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonQuestBossFight : MonoBehaviour
{
    private bool hasTriggered = false;
	private bool gateOpened = false;
	[SerializeField] private GameObject boss;
	[SerializeField] private GameObject canva;
	[SerializeField] private GameObject wall;
	[SerializeField] private GameObject wall1;
	[SerializeField] private GameObject bgm;
	[SerializeField] private GameObject potions;
	public bool hasPlayerEnter;
	
	
	
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
			hasPlayerEnter = true;
			wall.SetActive(true);
			wall1.SetActive(true);
			
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
                uiManager.StartCoroutine(uiManager.DemonQuestBossFight());
            }
            else
            {
                Debug.LogWarning("Lvl4UIManager component not found in the scene.");
            }
			
			if(boss.activeSelf)
			{
				bgm.SetActive(true);
				SoundManager.Instance.StopMusic();
			}
			
        }
	
    }
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			hasPlayerEnter = false;
			if (canva != null)
			{
				canva.SetActive(false);
			}
			SoundManager.Instance.PlayMusic();
			if (bgm != null)
				bgm.SetActive(false);
			else
				return;
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
		potions.SetActive(true);
		SoundManager.Instance.PlayMusic();
		if (bgm != null)
				bgm.SetActive(false);
		canva.SetActive(false);
		wall.SetActive(false);
		wall1.SetActive(false);
		hasTriggered = true; // Mark the trigger as activated
		// GameObject has been destroyed
		Debug.Log("GameObject has been destroyed!");
	}
	
	public void CameraShakeStart()
	{
		// Assuming bgm is the GameObject containing the AudioCameraShake component
		AudioCameraShake bgmShake = bgm.GetComponent<AudioCameraShake>();

		// Check if the component is found
		if (bgmShake != null)
		{
			// Enable the component
			bgmShake.enabled = true;
		}
		else
		{
			// Log a warning if the component is not found
			Debug.LogWarning("AudioCameraShake component not found on the GameObject.");
		}
	}
	
	public void CameraShakeEnd()
	{
		// Assuming bgm is the GameObject containing the AudioCameraShake component
		AudioCameraShake bgmShake = bgm.GetComponent<AudioCameraShake>();

		// Check if the component is found
		if (bgmShake != null)
		{
			// Enable the component
			bgmShake.enabled = false;
		}
		else
		{
			// Log a warning if the component is not found
			Debug.LogWarning("AudioCameraShake component not found on the GameObject.");
		}
	}
}
