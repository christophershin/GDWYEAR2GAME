using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 moveDirection;

    

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            cameraTransform.gameObject.SetActive(false);
            return;

        }

        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {

        // Player Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (controller.isGrounded)
        {
            moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
            moveDirection *= moveSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }

        // Apply Gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the CharacterController
        controller.Move(moveDirection * Time.deltaTime);

        RotateCamera();
    }
    void RotateCamera()
    {

        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

    }

}