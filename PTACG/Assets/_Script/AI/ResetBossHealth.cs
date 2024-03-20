using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBossHealth : MonoBehaviour
{
    private Health bossHealth;

    private void Awake()
    {
        // Get the Lvl4BossHealth component attached to the same GameObject
        bossHealth = GetComponent<Health>();
    }

    // Public method to reset the boss's health
    public void ResetHealth()
    {
        // Check if the boss health component is available
        if (bossHealth != null)
        {
            // Reset the boss's current health to its maximum health
            bossHealth.CurrentHealth = bossHealth.maxHealth;
			
			if(Lvl4UIManager.Instance != null)
			{
				Lvl4UIManager.Instance.UpdateBossHealth(gameObject.tag, bossHealth.CurrentHealth, bossHealth.maxHealth);
			}
			else if(UIManager.Instance != null)
			{
				UIManager.Instance.UpdateBossHealth(bossHealth.CurrentHealth, bossHealth.maxHealth);
			}
			
        }
        else
        {
            Debug.LogWarning("Lvl4BossHealth component not found on the boss GameObject.");
        }
    }
}
