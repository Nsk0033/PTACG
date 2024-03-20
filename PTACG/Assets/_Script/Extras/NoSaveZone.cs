using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSaveZone : MonoBehaviour
{
    [SerializeField] private GameObject noSavePanel;
	[SerializeField] private GameObject boss;

    

    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Activate the "NoSavePanel" GameObject
            if (noSavePanel != null && boss != null && boss.activeSelf)
			{
				noSavePanel.SetActive(true);
			}
			else
				noSavePanel.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Deactivate the "NoSavePanel" GameObject when the player exits the trigger area
            if (noSavePanel != null)
            {
                noSavePanel.SetActive(false);
            }
        }
    }
	
	
}
