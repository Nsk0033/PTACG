using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
	public string sceneName;
    // Start is called before the first frame update
    
	public void SwitchScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    // OnTriggerEnter2D method called when a Collider2D enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the Collider2D that entered the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Switch to the specified scene
            SwitchScene();
        }
    }
    
}
