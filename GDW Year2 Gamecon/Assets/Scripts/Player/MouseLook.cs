using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;


public class MouseLook : NetworkBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // Assign your player's transform in the Inspector

    private float xRotation = 0f;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {

            gameObject.SetActive(false);
            return;

        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {



        CameraLook();
        


    }


    void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        // Vertical camera rotation (looking up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp to prevent over-rotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        

        // Horizontal player rotation (looking left/right)
        playerBody.Rotate(Vector3.up * mouseX);

    }
}