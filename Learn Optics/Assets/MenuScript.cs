using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{



    public void LoadConvexScene()
    {
        SceneManager.LoadSceneAsync("LearnConvex");
        Screen.orientation = ScreenOrientation.Landscape;
    }

    public void LoadConcaveScene()
    {
        SceneManager.LoadSceneAsync("LearnConcave");
        Screen.orientation = ScreenOrientation.Landscape;

    }

    public void LoadMirrorScene()
    {
        SceneManager.LoadSceneAsync("LearnMirrors");
        Screen.orientation = ScreenOrientation.Landscape;

    }

    public void LoadSandboxScene()
    {
        SceneManager.LoadSceneAsync("Activity");
        Screen.orientation = ScreenOrientation.Landscape;

    }

}
