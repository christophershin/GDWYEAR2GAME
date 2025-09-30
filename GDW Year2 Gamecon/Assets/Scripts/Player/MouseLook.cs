using Alteruna;
using Unity.VisualScripting;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public class MouseLook : AttributesSync
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // Assign your player's transform in the Inspector

    private float xRotation = 0f;
    [SerializeField] private Alteruna.Avatar _avatar;

    void Start()
    {
        if (!_avatar.IsMe)
        {
            return;
        }


        Cursor.lockState = CursorLockMode.Locked; // Lock and hide the cursor
    }

    void Update()
    {
        if (!_avatar.IsMe)
        {
            return;
        }


        BroadcastRemoteMethod("CameraLook");

    }


    [SynchronizableMethod]
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