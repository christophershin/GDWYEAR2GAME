using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 moveDirection;


    public string LoseScreen;
    public string WinScreen;

    public List<GameObject> objectsWithShaders;
    public List<Material> Shader_materials;

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

        if(Input.GetKey(KeyCode.LeftShift))
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        // Apply Gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the CharacterController
        controller.Move(moveDirection * Time.deltaTime);

        //toggling shaders 
        ToggleShaders();
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

    public void ToggleShaders()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for(int i=0; i<objectsWithShaders.Count; i++)
            {
                objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[0];
                objectsWithShaders[i].GetComponent<ChangeMaterial>().anotherMaterial = Shader_materials[0];
                
                
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            for (int i = 0; i < objectsWithShaders.Count; i++)
            {
                objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[1];
                objectsWithShaders[i].GetComponent<ChangeMaterial>().anotherMaterial = Shader_materials[1];
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            for (int i = 0; i < objectsWithShaders.Count; i++)
            {
                objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[2];
                objectsWithShaders[i].GetComponent<ChangeMaterial>().anotherMaterial = Shader_materials[2];
            }
        }
    }
}