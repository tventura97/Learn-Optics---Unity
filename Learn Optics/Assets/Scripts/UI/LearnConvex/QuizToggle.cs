using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer
{
    public class QuizToggle : MonoBehaviour
    {
        Toggle quizToggle;
        GameObject ObjectArrow;
        GameObject GenerateQuizButton;
        GameObject PlayScene;


        void Start()
        {
            quizToggle = GetComponent<Toggle>();
            quizToggle.isOn = false;
            PlayScene = GameObject.Find("PlayScene");
            GenerateQuizButton = GameObject.Find("GenerateSceneButton");
        }


        public void OnToggle()
        {
            //Maybe just call the scene panel script here

            PlayScene.GetComponent<TogglePlayScene>().OnToggle();
            GenerateQuizButton.SetActive(quizToggle.isOn);

        }
    }
}
