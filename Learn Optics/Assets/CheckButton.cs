using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer
{
    public class CheckButton : MonoBehaviour
    {

        GameObject QuizToggle;
        GameObject ImageArrow;
        void Start()
        {
            QuizToggle = GameObject.Find("QuizToggle");
            ImageArrow = GameObject.Find("ImageArrow");
        }

        void Update()
        {
            if (!QuizToggle.GetComponent<Toggle>().isOn)
            {
                enabled = false;
            }
        }

        public void OnClick()
        {
            if (((GameObject.Find("Root").transform.position.x - ImageArrow.GetComponent<ImageArrowGeneration>().ImageDistance)/ImageArrow.transform.position.x > 0.9) &&
                ((GameObject.Find("Root").transform.position.x - ImageArrow.GetComponent<ImageArrowGeneration>().ImageDistance)/ ImageArrow.transform.position.x) < 1.1)
                {
                ImageArrow.GetComponent<ImageArrowGeneration>().ResetALR();
                print("correct");
                }
            else
            {
                print("incorrect");
            }
        }
    }
}
