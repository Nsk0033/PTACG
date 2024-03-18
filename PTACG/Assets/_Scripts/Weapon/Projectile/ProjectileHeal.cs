using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHeal : MonoBehaviour
{
    public Character ProjectileOwner { get; set; }
    private Health playerHealth;
	[SerializeField] private int healAmount = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player GameObject by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        // Get the Health component from the player GameObject
        if (playerObject != null)
        {
            playerHealth = playerObject.GetComponent<Health>();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GainHealth(healAmount);
        }
    }

    void GainHealth(int amount)
    {
        if (playerHealth != null)
        {
            playerHealth.GainHealth(amount);
			Debug.Log("Heal Player");
        }
    }
}
