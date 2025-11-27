using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour
{

    public List<GameObject> objectTextures;
    public List<Material> Shader_materials;
    public List<GameObject> objectsWithShaders;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ToggleTextures();
        ToggleShaders();
    }

    public void ToggleTextures()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            objectTextures[0].GetComponent<Renderer>().material = Shader_materials[1];
            objectTextures[1].GetComponent<Renderer>().material = Shader_materials[2];
            objectTextures[2].GetComponent<Renderer>().material = Shader_materials[3];
            objectTextures[3].GetComponent<Renderer>().material = Shader_materials[4];
            objectTextures[4].GetComponent<Renderer>().material = Shader_materials[5];
            objectTextures[5].GetComponent<Renderer>().material = Shader_materials[6];
            objectTextures[6].GetComponent<Renderer>().material = Shader_materials[7];
            objectTextures[7].GetComponent<Renderer>().material = Shader_materials[8];
            objectTextures[8].GetComponent<Renderer>().material = Shader_materials[12];
            objectTextures[9].GetComponent<Renderer>().material = Shader_materials[11];
            objectTextures[10].GetComponent<Renderer>().material = Shader_materials[11];
            objectTextures[11].GetComponent<Renderer>().material = Shader_materials[11];
            objectTextures[12].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[13].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[14].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[15].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[16].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[17].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[18].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[19].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[20].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[21].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[22].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[23].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[24].GetComponent<Renderer>().material = Shader_materials[10];
            objectTextures[25].GetComponent<Renderer>().material = Shader_materials[9];
            objectTextures[26].GetComponent<Renderer>().material = Shader_materials[13];
        }
    }

    public void ToggleShaders()
    {

        for (int i = 0; i < objectsWithShaders.Count; i++)
        {
            // no lighting
            objectsWithShaders[i].GetComponent<Renderer>().material = Shader_materials[0];
        }

        // turn off all material and replace them with a basic one
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("TEST");
            for (int k = 0; k < objectTextures.Count; k++)
            {
                objectTextures[k].GetComponent<Renderer>().material = Shader_materials[0];
            }

        }
    }
}