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
            if (GameObject.Find("QuizToggle").GetComponent<Toggle>().isOn)
            {
                ObjectArrow.transform.GetComponent<ObjectArrowControls>().ResetALRs();
                ObjectArrow.transform.position = new Vector3(Random.Range(OpticalElement.transform.position.x - 30, OpticalElement.transform.position.x - 4),
                OpticalElement.transform.position.y + 2, OpticalElement.transform.position.z);
            }
        }
    }
}
