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


    public string LoseScreen;
    public string WinScreen;


    public List<GameObject> objectsWithShaders;
    public List<GameObject> objectTextures;
    public List<Material> Shader_materials;
    public GameObject CanvasImage;
    public GameObject cameraColorGrading;

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
        ToggleTextures();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Boundary"))
    //    {
    //        Debug.Log("lose");
    //        SceneManager.LoadScene(LoseScreen);


    //    }
    //    else if (other.gameObject.CompareTag("Goal"))
    //    {
    //        Debug.Log("win");
    //        SceneManager.LoadScene(WinScreen);
    //    }
    //    else if (other.gameObject.CompareTag("Parriable"))
    //    {
    //        Debug.Log("lose");
    //        SceneManager.LoadScene(LoseScreen);
    //    }
    //}


    public void ToggleTextures()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            objectTextures[0].GetComponent<Renderer>().material = Shader_materials[11];
            objectTextures[1].GetComponent<Renderer>().material = Shader_materials[11];
            objectTextures[2].GetComponent<Renderer>().material = Shader_materials[11];
            objectTextures[3].GetComponent<Renderer>().material = Shader_materials[12];
            objectTextures[4].GetComponent<Renderer>().material = Shader_materials[14];
            objectTextures[5].GetComponent<Renderer>().material = Shader_materials[14];
            objectTextures[6].GetComponent<Renderer>().material = Shader_materials[14];
            objectTextures[7].GetComponent<Renderer>().material = Shader_materials[14];
            objectTextures[8].GetComponent<Renderer>().material = Shader_materials[13];

        }
    }



    public void ToggleShaders()
    {

        // turn off all material and replace them with a basic one
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            for (int i = 0; i < objectsWithShaders.Count; i++)
            {
                // no lighting
                objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[0];
                // turn the camera with color grading off
                
            }

            cameraColorGrading.SetActive(false);

            //for (int k = 0; k < objectTextures.Count; k++)
            //{
            //    objectTextures[k].GetComponent<Renderer>().material = Shader_materials[0];
            //}

        }

        //// simple diffuse lighting
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    for (int i = 0; i < objectsWithShaders.Count; i++)
        //    {
        //        objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[1];

        //    }
        //}

        //// diffuse lighting with ambient
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    for (int i = 0; i < objectsWithShaders.Count; i++)
        //    {

        //        objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[2];
        //    }
        //}

        //// simple specular
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    for (int i = 0; i < objectsWithShaders.Count; i++)
        //    {

        //        objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[3];
        //    }
        //}

        //// custom additional effects
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    for (int i = 0; i < objectsWithShaders.Count; i++)
        //    {

        //        objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[4];
        //    }
        //}

        //// Rim Lighting
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    for (int i = 0; i < objectsWithShaders.Count; i++)
        //    {
        //        objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[5];
        //    }
        //}

        //// Bump Mapping
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    for (int i = 0; i < objectsWithShaders.Count; i++)
        //    {
        //        // custom additional effects
        //        objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[6];
        //    }
        //}

        //// Toon Shader
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    for (int i = 0; i < objectsWithShaders.Count; i++)
        //    {
        //        // custom additional effects
        //        objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[7];
        //    }
        //}

        // color grading with LUT warm
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            cameraColorGrading.SetActive(true);

            CanvasImage.GetComponent<RawImage>().material = Shader_materials[1];

        }

        // color grading with LUT cold
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {

            // turn camera on
            cameraColorGrading.SetActive(true);

            CanvasImage.GetComponent<RawImage>().material = Shader_materials[2];

        }

        // color grading with LUT custom
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {

            // turn camera on
            cameraColorGrading.SetActive(true);

            CanvasImage.GetComponent<RawImage>().material = Shader_materials[3];
        }

        // color grading with LUT custom
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {

            // turn camera on
            cameraColorGrading.SetActive(true);

            CanvasImage.GetComponent<RawImage>().material = Shader_materials[4];
        }

    }
}