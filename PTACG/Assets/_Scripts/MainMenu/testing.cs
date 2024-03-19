using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    private bool UIcheck;
    // Start is called before the first frame update
    private void Start()
    {
        UIcheck = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PauseMenu.active)
            {
                PauseMenu.SetActive(true);
            }
            else if (PauseMenu.active) 
            {
                PauseMenu.SetActive(false);
            }
        }
    }

}
