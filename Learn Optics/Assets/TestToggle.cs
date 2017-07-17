using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class TestToggle : MonoBehaviour
    {

        GameObject ScenePanel;
        GameObject ObjectArrow;
        GameObject ImageArrow;
        GameObject ImageDistanceText;
        Toggle PlaySceneToggle;

        void Start()
        {
            ScenePanel = GameObject.Find("PromptPanel");
            PlaySceneToggle = GetComponent<Toggle>();
            ObjectArrow = GameObject.FindGameObjectWithTag("ObjectArrow");
            ImageArrow = GameObject.Find("ImageArrow");

        }

        private void Update()
        {

        }

        public void OnToggle()
        {
            ScenePanel.GetComponent<PlayConvexLearningScene>().SetDefaults();
            ScenePanel.SetActive(!ScenePanel.activeSelf);
            //If the toggle is disabled, turn on the Image/Object Arrows
            ObjectArrow.SetActive(!PlaySceneToggle.isOn);
            ImageArrow.SetActive(!PlaySceneToggle.isOn);
        }
    }
}