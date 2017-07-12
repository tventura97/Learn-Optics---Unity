using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer
{
    public class PlayConvexLearningScene : MonoBehaviour
    {

        public GameObject AnimatedLightEmitter;
        public GameObject ProgrammableALR;
        GameObject promptPanelText;
        GameObject ObjectArrow;
        GameObject ImageArrow;
        GameObject Root;
        GameObject FocalPointMarkerHolder;
        GameObject ParallelRay;
        GameObject OpticalCenterRay;
        GameObject FocalPointRay;
        GameObject OpticalElement;
        Animator MainCamera;
        Animator PrismHolder;
        Text panelText;
        Button GenerateRaysButton;
        Button ResetRaysButton;
        string[] texts;
        int promptNumber;
        int counter;
        Vector3 FocalPoint;

        void Start()
        {
            Root = GameObject.Find("Root");
            GenerateRaysButton = GameObject.Find("GenerateRays").GetComponent<Button>();
            ResetRaysButton = GameObject.Find("Reset").GetComponent<Button>();
            MainCamera = GameObject.Find("Main Camera").GetComponent<Animator>();
            PrismHolder = GameObject.Find("PrismHolder").GetComponent<Animator>();
            ObjectArrow = GameObject.Find("ObjectArrow");
            ImageArrow = GameObject.Find("ImageArrow");
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            FocalPointMarkerHolder = GameObject.Find("FocalPointMarkerHolder");
            ObjectArrow.SetActive(false);
            ImageArrow.SetActive(false);
            FocalPointMarkerHolder.SetActive(false);
            FocalPoint = new Vector3(OpticalElement.transform.position.x + 12, OpticalElement.transform.position.y);
            //Look I know this is a dumb way to initialize it but I'm lazy right now
            texts = new string[] { "This is a convex, or converging, lens.", "It is called a converging lens because rays of light that pass through the lens converge at the lens' focal point.",
                "To understand how light refracts through a convex lens,", "Imagine that the lens is really just a series of prisms stacked on top of each other.",
                "When light passes through a prism, it always bends towards the base of the prism.", "These rays of light form images where they intersect",
                "This method of representing image formation is known as ray-tracing.", "To repeat this process, first draw a ray that propagates from the object to the lens parallel to the optical axis",
                "This ray will pass through the focal point of the lens", "Next, draw a ray that passes through the optical center of the lens. This will be undeviated by the lens",
                "Finally, draw a ray that passes through the left focal point of the lens", "This ray will emerge from the lens parallel to the optical axis",
                "The image forms at the intersection of the three rays, and is inverted."};


            promptPanelText = GameObject.Find("PromptPanelText");
            print(GameObject.Find("PromptPanelText").GetComponent<Text>());
            panelText = promptPanelText.GetComponent<Text>();
            counter = 1;
            panelText.text = texts[0];



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
                    Instantiate(AnimatedLightEmitter, new Vector3(Root.transform.position.x - 80, Root.transform.position.y + 6, 0), Quaternion.identity, Root.transform);
                    Instantiate(AnimatedLightEmitter, new Vector3(Root.transform.position.x - 80, Root.transform.position.y + 9, 0), Quaternion.identity, Root.transform);
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
                    ObjectArrow.SetActive(true);

                    //Disable object distance text for the purpose of the tutorial
                    ObjectArrow.transform.GetChild(1).gameObject.SetActive(false);
                    break;

                case 6:
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Continue";
                    ImageArrow.SetActive(true);
                    GenerateRaysButton.onClick.Invoke();
                    print(counter);
                    break;

                case 7:
                    ResetRaysButton.onClick.Invoke();
                    ImageArrow.SetActive(false);
                    FocalPointMarkerHolder.SetActive(true);
                    ParallelRay = Instantiate(ProgrammableALR, ObjectArrow.transform.position, Quaternion.identity, Root.transform);
                    ParallelRay.GetComponent<SetPoints>().InitializeALR();
                    ParallelRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F), 0);
                    ParallelRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F), 2);
                    break;

                case 8:
                    ParallelRay.GetComponent<SetPoints>().SetLinePoint(1000 * (FocalPoint - new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F)), 100);
                    break;

                case 9:
                    OpticalCenterRay = Instantiate(ProgrammableALR, ObjectArrow.transform.position, Quaternion.identity, Root.transform);
                    OpticalCenterRay.GetComponent<SetPoints>().InitializeALR();
                    OpticalCenterRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F), 0);
                    OpticalCenterRay.GetComponent<SetPoints>().SetLinePoint(1000 * (new Vector3(OpticalElement.transform.position.x, OpticalElement.transform.position.y) 
                                                                              - new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F)), 100);
                    break;

                case 10:
                    FocalPointRay = Instantiate(ProgrammableALR, ObjectArrow.transform.position, Quaternion.identity, Root.transform);
                    FocalPointRay.GetComponent<SetPoints>().InitializeALR();
                    FocalPointRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F), 0);
                
                    //This -57.3F is calculated from a different class. Since this is a demonstrative tutorial that has no user interaction, I'm just gonna go ahead and leave that value 
                    //hard coded here.
                    FocalPointRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, -57.3F), 2);
                    break;
                case 11:
                    FocalPointRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(100, -57.3F, 0), 2);
                    break;

                case 12:
                    ImageArrow.SetActive(true);
                    break;

                case 13:
                    GameObject[] ProgrammableALRS = GameObject.FindGameObjectsWithTag("ProgrammableALR");
                    for (int i = 0; i < ProgrammableALRS.Length; i++)
                    {
                        Destroy(ProgrammableALRS[i]);
                    }
                    break;


            }

            if (counter < texts.Length)
            {
                panelText.text = texts[counter];
            }
            counter++;


        }
    }
}