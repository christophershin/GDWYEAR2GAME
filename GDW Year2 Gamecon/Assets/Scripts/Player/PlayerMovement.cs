using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 moveDirection;

    private Alteruna.Avatar _avatar;

    public string LoseScreen;
    public string WinScreen;

    void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();

        if (!_avatar.IsMe)
            return;

        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!_avatar.IsMe)
            return;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boundary"))
        {
            Debug.Log("lose");
            SceneManager.LoadScene(LoseScreen);


        }else if (other.gameObject.CompareTag("Goal"))
        {
            Debug.Log("win");
            SceneManager.LoadScene(WinScreen);
        }else if(other.gameObject.CompareTag("Parriable"))
        {
            Debug.Log("lose");
            SceneManager.LoadScene(LoseScreen);
        }
    }
}