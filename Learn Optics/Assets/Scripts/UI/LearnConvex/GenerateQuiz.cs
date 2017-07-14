using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DigitalRuby.AnimatedLineRenderer
{
    public class GenerateQuiz : MonoBehaviour
    {

        GameObject ObjectArrow;
        GameObject OpticalElement;

        void Start()
        {
            ObjectArrow = GameObject.Find("ObjectArrow");
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
        }

        public void OnClick()
        {
            ObjectArrow = GameObject.Find("ObjectArrow");
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            if (true)//GameObject.Find("QuizToggle").GetComponent<Toggle>().isOn)
            {
                ObjectArrow.transform.GetComponent<ObjectArrowControls>().ResetALRs();
                ObjectArrow.transform.position = new Vector3(Random.Range(OpticalElement.transform.position.x - 30, OpticalElement.transform.position.x - 18.5F),
                OpticalElement.transform.position.y + 2, OpticalElement.transform.position.z);
            }
        }
    }
}
