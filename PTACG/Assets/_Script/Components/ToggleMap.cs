using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMap : CharacterComponents
{
	[SerializeField] private GameObject Minimap;
	[SerializeField] private GameObject Generalmap;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            OnOffMinimap();
        }
		if (Input.GetKeyDown(KeyCode.M))
        {
            OnOffGeneralmap();
        }
    }
	
	private void OnOffMinimap()
	{
		if (Minimap.activeSelf) // Check if Minimap GameObject is currently active
		{
			Minimap.SetActive(false); // Deactivate the Minimap GameObject
		}
		else
		{
			Minimap.SetActive(true); // Activate the Minimap GameObject
		}
	}
	
	private void OnOffGeneralmap()
	{
		if (Generalmap.activeSelf) // Check if Minimap GameObject is currently active
		{
			Generalmap.SetActive(false); // Deactivate the Minimap GameObject
		}
		else
		{
			Generalmap.SetActive(true); // Activate the Minimap GameObject
		}
	}
}
