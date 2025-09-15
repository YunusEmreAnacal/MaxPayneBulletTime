using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}