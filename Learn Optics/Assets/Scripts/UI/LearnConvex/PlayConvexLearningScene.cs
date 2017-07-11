using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer
{
    public class PlayConvexLearningScene : MonoBehaviour
    {

        public GameObject AnimatedLightEmitter;
        GameObject promptPanelText;
        GameObject ObjectArrow;
        GameObject LightEmitter;
        GameObject Root;
        Animator MainCamera;
        Animator PrismHolder;
        Text panelText;
        string[] texts;
        int promptNumber;
        int counter;
        // Use this for initialization
        void Start()
        {
            Root = GameObject.Find("Root");
            MainCamera = GameObject.Find("Main Camera").GetComponent<Animator>();
            PrismHolder = GameObject.Find("PrismHolder").GetComponent<Animator>();
            ObjectArrow = GameObject.Find("ObjectArrow");
            ObjectArrow.SetActive(false);
            //Look I know this is a dumb way to initialize it but I'm lazy right now
            texts = new string[] { "This is a convex, or converging, lens.", "It is called a converging lens because rays of light that pass through the lens converge at the lens' focal point.",
            "To understand how light refracts through a convex lens,", "Imagine that the lens is really just a series of prisms stacked on top of each other.",
            "When light passes through a prism, it always bends towards the base of the prism.", "These rays of light form images where they intersect"};


            promptPanelText = GameObject.Find("PromptPanelText");
            print(GameObject.Find("PromptPanelText").GetComponent<Text>());
            panelText = promptPanelText.GetComponent<Text>();
            counter = 1;
            panelText.text = texts[0];
            promptNumber = texts.Length;



        }

        // Update is called once per frame
        public void onClick()
        {

            switch (counter)
            {
                case 1:
                    //Spawn animated light emitters to demonstrate concept of converging at focal point
                    for (int i = -8; i <= 8; i += 4)
                    {
                        Instantiate(AnimatedLightEmitter, new Vector3(Root.transform.position.x - 80, Root.transform.position.y + i, 0), Quaternion.identity,
                            GameObject.FindGameObjectWithTag("OpticalElement").transform);
                    }
                    break;

                case 2:
                    //Destroy animated light emitters

                    break;

                case 3:
                    GameObject[] InitialAnimatedLightEmitters = GameObject.FindGameObjectsWithTag("AnimatedLightEmitter");
                    for (int i = 0; i < InitialAnimatedLightEmitters.Length; i++)
                    {
                        Destroy(InitialAnimatedLightEmitters[i]);
                    }
                    //Pan to top of lens to demonstrate prism concept
                    MainCamera.SetBool("Pan_TopofLens", true);
                    PrismHolder.SetBool("triggerPrism", true);
                    break;

                case 4:
                    //Spawn two animated light emitters to demonstrate light bending through prisms.
                    Instantiate(AnimatedLightEmitter, new Vector3(Root.transform.position.x - 80, Root.transform.position.y + 6, 0), Quaternion.identity, GameObject.Find("Root").transform);
                    Instantiate(AnimatedLightEmitter, new Vector3(Root.transform.position.x - 80, Root.transform.position.y + 9, 0), Quaternion.identity, GameObject.Find("Root").transform);
                    break;

                case 5:
                    GameObject[] AnimatedLightEmitters = GameObject.FindGameObjectsWithTag("AnimatedLightEmitter");
                    for (int i = 0; i < AnimatedLightEmitters.Length; i++)
                    {
                        Destroy(AnimatedLightEmitters[i]);
                    }
                    MainCamera.SetBool("Pan_TopofLens", false);
                    PrismHolder.SetBool("triggerPrism", false);
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Generate";
                    break;

                case 6:
                    ObjectArrow.SetActive(true);
                    break;



            }

            if (counter < promptNumber)
            {
                panelText.text = texts[counter];
            }
            counter++;


        }
    }
}