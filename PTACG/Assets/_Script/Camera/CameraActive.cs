using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraActive : MonoBehaviour
{
    [SerializeField] private CameraSwitcher cameraList;
    [SerializeField] private CinemachineVirtualCamera activeCamera;
    [SerializeField] private GameObject MapIcon;
	
	private CinemachineBrain cinemachineBrain;

	private void Start()
    {
        // Get the CinemachineBrain component in the scene
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
    }

    void OnTriggerStay2D(Collider2D other)
	{
        if (other.CompareTag("Player"))
        {
            // Call the ActivateCamera method to activate the specified camera
            cameraList.ActivateCamera(activeCamera);
        }
    }
	
	private void Update()
    {
        if (IsCameraActive(activeCamera))
        {
            MapIcon.SetActive(true);
            Debug.Log("Icon True");
        }
        else
        {
            MapIcon.SetActive(false);
        }
    }

    // Method to check if the CinemachineVirtualCamera is actively in use by the CinemachineBrain
    private bool IsCameraActive(CinemachineVirtualCamera virtualCamera)
    {
        if (cinemachineBrain == null)
            return false;

        // Check if the virtual camera's GameObject is active in the scene hierarchy
        return virtualCamera.VirtualCameraGameObject.activeInHierarchy;
    }
}
