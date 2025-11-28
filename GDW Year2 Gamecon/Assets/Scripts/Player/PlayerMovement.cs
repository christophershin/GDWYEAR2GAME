using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 moveDirection;

    public GameObject pressE;

    public string LoseScreen;
    public string WinScreen;


    public List<GameObject> objectsWithShaders;
    public List<GameObject> objectTextures;
    public List<Material> Shader_materials;
    public GameObject CanvasImage;
    public GameObject cameraColorGrading;

    void Start()
    {
        pressE.SetActive(false);
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

        if (Input.GetKey(KeyCode.LeftShift))
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Crown"))
        {
            pressE.SetActive(true);
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
            pressE.SetActive(false);
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
        SceneManager.LoadScene(WinScreen);
    }
    public void SceneLose()
    {
        SceneManager.LoadScene(LoseScreen);
    }



    public void ToggleShaders()
    {

        //// turn off all material and replace them with a basic one
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    for (int i = 0; i < objectsWithShaders.Count; i++)
        //    {
        //        // no lighting
        //        objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[0];
        //        // turn the camera with color grading off
                
        //    }

        //    cameraColorGrading.SetActive(false);

        //}

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            for (int i = 0; i < objectsWithShaders.Count; i++)
            {
                // no lighting
                objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[0];
                // turn the camera with color grading off

            }

            cameraColorGrading.SetActive(false);

        }

        // WarmLUT
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            cameraColorGrading.SetActive(true);

            CanvasImage.GetComponent<RawImage>().material = Shader_materials[1];

        }

        // CoolLUT
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {

            // turn camera on
            cameraColorGrading.SetActive(true);

            CanvasImage.GetComponent<RawImage>().material = Shader_materials[2];

        }

        // CustomLUT
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {

            // turn camera on
            cameraColorGrading.SetActive(true);

            CanvasImage.GetComponent<RawImage>().material = Shader_materials[3];
        }
    }
}