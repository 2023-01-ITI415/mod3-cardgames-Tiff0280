using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public void LoadScene1()
    {
        SceneManager.LoadScene("__Prospector_Scene_0");   //NOTE a different function for each scene
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene("NewProspector");
    }


}

