using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;

    public void ActivateCamera(CinemachineVirtualCamera activeVirtualCamera)
    {
        foreach (CinemachineVirtualCamera virtualCamera in virtualCameras)
        {
            // Activate the specified virtual camera and deactivate all other virtual cameras
            virtualCamera.gameObject.SetActive(virtualCamera == activeVirtualCamera);
        }
    }
}
