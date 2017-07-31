using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer
{
    public class HintButtonScript : MonoBehaviour
    {

        private Animator PopUpPromptAnimator;
        private Animator PopUpPromptTextAnimator;
        private Text PopUpPromptText;
        private int counter, LRIndex, CurrentRayIndex;
        private bool isUICoroutineExecuting;
        private bool isConcaveReflective;
        private bool isConvexReflective;
        private bool isConcave;
        private float MessageDuration;

        void Start()
        {
            PopUpPromptAnimator = GameObject.Find("PopUpPrompt").GetComponent<Animator>();
            PopUpPromptTextAnimator = GameObject.Find("PopUpPromptText").GetComponent<Animator>();
            PopUpPromptText = GameObject.Find("PopUpPromptText").GetComponent<Text>();
            //3 seconds is a good time duration to display the prompt
            MessageDuration = 3;
        }

        private void Update()
        {
            isConcaveReflective = GameObject.FindGameObjectWithTag("OpticalElement").GetComponent<Properties_Optical>().isConcaveReflective;
            isConvexReflective = GameObject.FindGameObjectWithTag("OpticalElement").GetComponent<Properties_Optical>().isConvexReflective;
            isConcave = GameObject.FindGameObjectWithTag("OpticalElement").GetComponent<Properties_Optical>().isConcave;

        }
        public void OnClick()
        {
            counter = GameObject.Find("GenerateQuizButton").GetComponent<GenerateQuizScript>().GetCounter();
            LRIndex = GameObject.Find("GenerateQuizButton").GetComponent<GenerateQuizScript>().GetLRIndex();
            CurrentRayIndex = GameObject.Find("GenerateQuizButton").GetComponent<GenerateQuizScript>().GetCurrentRayIndex();
            switch (CurrentRayIndex)
            {
                //Parallel Ray
                case 0:
                    //Remember that LRIndex starts at 1
                    switch (LRIndex)
                    {
                        //Ray that hits optical element
                        case 1:
                            DisplayPrompt("Draw a ray from the object, to the optical element, parallel to the optical axis.", MessageDuration);
                            break;
                        //Ray after interacting with optical element
                        case 2:
                            //Convex lens 
                            if (!isConcaveReflective && !isConvexReflective && !isConcave)
                            {
                                DisplayPrompt("Draw a ray to the focal point of the lens.", MessageDuration);
                            }
                            //Concave lens
                            else if (isConcave)
                            {
                                DisplayPrompt("Draw a ray away from the left focal point of the lens.", MessageDuration);
                            }
                            //Concave Mirror
                            else if (isConcaveReflective)
                            {
                                DisplayPrompt("Draw a ray reflecting towards the focal point of the mirror.", MessageDuration);
                            }

                            //Convex Mirror
                            else if (isConvexReflective)
                            {
                                DisplayPrompt("Draw a ray reflecting away from the right focal point of the mirror.", MessageDuration);
                            }
                            break;

                    }
                    break;

                //Optical Center Ray for lenses, Focal Point Ray for mirrors 
                case 1:
                    switch (LRIndex)
                    {
                        case 1:
                            //If mirror
                            if (isConcaveReflective || isConvexReflective)
                            {
                                //if Concave mirror in front of focal point
                                if (isConcaveReflective && CheckPositionOfObject())
                                {
                                    DisplayPrompt("Draw a ray from the object to the optical center of the mirror.", MessageDuration);
                                }
                                else if (isConcaveReflective)
                                {
                                    DisplayPrompt("Draw a ray to the mirror, through the left focal point", MessageDuration);
                                }
                                //if convex mirror
                                else if (isConvexReflective)
                                {
                                    DisplayPrompt("Draw a ray from the object, to the mirror, towards the right focal point of the mirror.", MessageDuration);
                                }
                            }
                            //if lens
                            else
                            {
                                DisplayPrompt("Draw a ray from the object to the optical center of the lens.", MessageDuration);
                            }
                            break;

                        case 2:
                            if (isConcaveReflective && CheckPositionOfObject())
                            {
                                //Do nothing
                            }
                            else if (isConcaveReflective || isConvexReflective)
                            {
                                DisplayPrompt("Draw a ray reflected from the mirror, parallel to the optical axis.", MessageDuration);
                            }

                            //if lens, do nothing
                            else
                            {
                                //Do nothing
                            }
                            break;
                    }
                    break;

                //Focal Point Ray for lenses
                case 2:
                    //Only lenses here, so don't worry about dealing with mirrors
                    switch (LRIndex)
                    {
                        case 1:
                            if (isConcave)
                            {
                                DisplayPrompt("Draw a ray from the object, to the lens, towards the right focal point.", MessageDuration);
                            }
                            else
                            {
                                DisplayPrompt("Draw a ray from the object, to the lens, through the left focal point.", MessageDuration);
                            }
                            break;

                        case 2:
                            //Both lenses behave the same way in this part of ray-tracing
                            DisplayPrompt("Draw a ray away from the lens, parallel to the optical axis.", MessageDuration);

                            break;
                    }
                    break;
            }
        }


        private void DisplayPrompt(string text, float duration)
        {
            PopUpPromptAnimator.SetBool("DisplayPrompt", true);
            PopUpPromptTextAnimator.SetBool("DisplayPrompt", true);
            PopUpPromptText.fontSize = 10;
            PopUpPromptText.text = text;
            StartCoroutine(DestroyMessagePrompt(duration));

        }

        IEnumerator DestroyMessagePrompt(float time)
        {
            if (isUICoroutineExecuting)
                yield break;

            isUICoroutineExecuting = true;

            yield return new WaitForSeconds(time);

            PopUpPromptAnimator.SetBool("DisplayPrompt", false);
            PopUpPromptTextAnimator.SetBool("DisplayPrompt", false);



            isUICoroutineExecuting = false;
        }

        //If object is in front of focalpoint return true
        private bool CheckPositionOfObject()
        {

            if (Mathf.Abs(GameObject.Find("ObjectArrow").transform.position.x - GameObject.FindGameObjectWithTag("OpticalElement").transform.position.x) < GameObject.Find("GenerateQuizButton").GetComponent<GenerateQuizScript>().FocalLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

