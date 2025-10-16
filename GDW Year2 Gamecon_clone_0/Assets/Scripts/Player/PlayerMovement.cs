using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 moveDirection;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;

        }

       //UpdatePositionServerRPC();
        
    }

    void Start()
    {

        controller = GetComponent<CharacterController>();
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
    }

    /*[ServerRpc(RequireOwnership = false)]
    private void UpdatePositionServerRPC()
    {
        int SpawnCount = GameObject.Find("World").GetComponent<GameManager>().spawnList.Count;

        int randNum = UnityEngine.Random.Range(0, 1);

        transform.position = GameObject.Find("World").GetComponent<GameManager>().spawnList[randNum].transform.position;
    }*/
}