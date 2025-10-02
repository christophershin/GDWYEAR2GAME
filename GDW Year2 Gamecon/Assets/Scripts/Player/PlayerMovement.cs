using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 moveDirection;

    PhotonView view;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (view.IsMine)
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
        }
    }
}