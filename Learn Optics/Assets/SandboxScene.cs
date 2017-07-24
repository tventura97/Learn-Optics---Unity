using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SandboxScene : MonoBehaviour {

    public void OnClick()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
