using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer
{


    public class PlaySceneToggleScript : MonoBehaviour
    {
        GameObject ScenePanel;
        GameObject ObjectArrow;
        GameObject ImageArrow;
        GameObject ImageDistanceText;
        Toggle PlaySceneToggle;
        Animator CameraAnimator;

        void Start()
        {
            ScenePanel = GameObject.Find("PromptPanel");
            PlaySceneToggle = GetComponent<Toggle>();
            ObjectArrow = GameObject.FindGameObjectWithTag("ObjectArrow");
            ImageArrow = GameObject.Find("ImageArrow");
            CameraAnimator = GameObject.Find("Main Camera").GetComponent<Animator>();
            print("Objects Successfully Initialized");
        }

        private void Update()
        {

        }

        public void OnToggle()
        {
            GameObject.Find("GenerateQuizButton").GetComponent<GenerateQuizScript>().SetPlayScene(PlaySceneToggle.isOn);
            ScenePanel.GetComponent<ConcaveLearningScript>().SetDefaults();
            ScenePanel.SetActive(!ScenePanel.activeSelf);
            //If the toggle is disabled, turn on the Image/Object Arrows
            ObjectArrow.SetActive(!PlaySceneToggle.isOn);
            ImageArrow.SetActive(!PlaySceneToggle.isOn);
            CameraAnimator.SetBool("Pan_TopofLens", false);
            CameraAnimator.SetBool("Pan_FocalPoint", false);

        }
    }
}
