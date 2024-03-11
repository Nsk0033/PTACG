using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
	public string sceneName;
	
	
    public void StartGame()
	{
		SceneManager.LoadScene(sceneName);
	}
	
	public void OpenSetting()
	{
		Debug.Log("Setting Menu");
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
}
