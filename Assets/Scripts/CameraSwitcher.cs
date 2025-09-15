using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera dollyCam; // Dolly üzerindeki vCam
    public Camera mainCamera; // Oyun kamerasý (Main Camera)
    public CinemachineDollyCart dollyCart; // Cart objesi

    void Update()
    {
        // Dolly cart path’in sonuna gelmiþse
        if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
        {
            // Dolly cam kapat
            dollyCam.gameObject.SetActive(false);

            // Main camera aktif et
            mainCamera.enabled = true;
        }
    }
}
