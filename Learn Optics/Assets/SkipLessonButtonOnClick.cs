using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DigitalRuby.AnimatedLineRenderer
{
    public class SkipLessonButtonOnClick : MonoBehaviour
    {
        GameObject PromptPanel;
        GameObject GenerateQuizButton;

        public void OnClick()
        {
            PromptPanel = GameObject.Find("PromptPanel");
            GenerateQuizButton = GameObject.Find("GenerateQuizButton");

            PromptPanel.SetActive(false);
            GenerateQuizButton.GetComponent<GenerateQuizScript>().SetPlayScene(false);
            //For some reason I can't destroy this so just https://imgflip.com/s/meme/Put-It-Somewhere-Else-Patrick.jpg
            Vector3 SomewhereElse = new Vector3(1000000000000000, 1000000000000, 0);
            transform.position = SomewhereElse;
        }

    }
}