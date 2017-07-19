using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadConcaveScene : MonoBehaviour {


    public void OnClick()
    {
        SceneManager.LoadSceneAsync("LearnConcave");
    }
}
