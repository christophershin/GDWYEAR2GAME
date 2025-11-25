using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 moveDirection;

    public GameObject PressE;

    public string WinScene;
    public string LoseScene;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;

        }
        
    }

    void Start()
    {
        PressE.SetActive(false);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Crown"))
        {
            PressE.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                SceneWin();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Crown"))
        {
            PressE.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            SceneLose();
        }
    }

    public void SceneWin()
    {
        SceneManager.LoadScene(WinScene);
    }
    public void SceneLose()
    {
        SceneManager.LoadScene(LoseScene);
    }
}