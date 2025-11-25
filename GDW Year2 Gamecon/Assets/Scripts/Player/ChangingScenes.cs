using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangingScenes : MonoBehaviour
{

    public string Scene;

    public void ChangeSceneTo()
    {

        SceneManager.LoadScene(Scene);

    }


}
