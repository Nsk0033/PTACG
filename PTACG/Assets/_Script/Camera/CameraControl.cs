using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	[SerializeField] private GameObject _virtualCamera1;
    [SerializeField] private GameObject _virtualCamera2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the object entering the trigger is the player
        {
            // Check if the first virtual camera is active
            if (_virtualCamera1.activeSelf)
            {
                // If active, disable it
                _virtualCamera1.SetActive(false);

                // Enable the second virtual camera
                _virtualCamera2.SetActive(true);
            }
            else
            {
                // If not active, disable the second virtual camera
                _virtualCamera2.SetActive(false);

                // Enable the first virtual camera
                _virtualCamera1.SetActive(true);
            }
        }
    }
}
