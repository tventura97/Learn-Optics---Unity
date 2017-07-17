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
        GameObject OpticalElement;
        void Start()
        {
            QuizToggle = GameObject.Find("QuizToggle");
            ImageArrow = GameObject.Find("ImageArrow");
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
        }

        public void OnClick()
        {
            if (Mathf.Abs((OpticalElement.transform.position.x - ImageArrow.transform.position.x) / ImageArrow.GetComponent<ImageArrowGeneration>().ImageDistance) > 0.98F
                && Mathf.Abs((OpticalElement.transform.position.x - ImageArrow.transform.position.x) / ImageArrow.GetComponent<ImageArrowGeneration>().ImageDistance) < 1.02F) 
            {
                print("correct");
            }
            else 
            {
                print("incorrect");
            }
        }
    }
}
