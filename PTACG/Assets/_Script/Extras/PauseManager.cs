using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
	[SerializeField] private GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // You can change the key as needed
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1; // Pause or resume time based on the isPaused flag
		pauseMenu.SetActive(isPaused);
    }
	
	public void GoToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
