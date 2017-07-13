using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer

/*This class is a disaster. It's supposed to be the tutorial scene for convex lenses, and I started out with spaghetti code and then it kind of just spiraled out of control.
 * This image: https://pics.me.me/when-your-spaghetti-code-goes-out-of-control-made-on-18799313.png pretty much sums up this class. Anyway, a lot of fundamental methods to be used
 * in the next tutorial classes and scenes SHOULD be much better. 
 */
{
    public class PlayConvexLearningScene : MonoBehaviour
    {

        public GameObject AnimatedLightEmitter;
        public GameObject ProgrammableALR;
        public GameObject ProgrammableLR;
        GameObject promptPanelText;
        GameObject ObjectArrow;
        GameObject ImageArrow;
        GameObject Root;
        GameObject FocalPointMarkerHolder;
        GameObject ParallelRay;
        GameObject OpticalCenterRay;
        GameObject FocalPointRay;
        GameObject OpticalElement;
        GameObject CurrentRay;
        GameObject CurrentALRRay;
        GameObject ImageDistanceText;
        Animator MainCamera;
        Animator PrismHolder;
        Animator EquationPanelAnimator;
        Text panelText;
        Button GenerateRaysButton;
        Button ResetRaysButton;
        Vector3 FocalPoint;
        Vector3 FocalPointLeft;
        string[] texts;
        float ErrorTolerance;
        bool LRInteract;
        bool isCoroutineExecuting;
        int LRIndex;
        int CurrentRayIndex;
        int promptNumber;
        int counter;

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
            EquationPanelAnimator = GameObject.Find("EquationPanel").GetComponent<Animator>();
            ImageDistanceText = GameObject.Find("ImageDistanceText");
            ObjectArrow.SetActive(false);
            ImageArrow.SetActive(false);
            ImageDistanceText.SetActive(false);
            FocalPointMarkerHolder.SetActive(true);
            FocalPoint = new Vector3(OpticalElement.transform.position.x + 12, OpticalElement.transform.position.y);
            FocalPointLeft = new Vector3(OpticalElement.transform.position.x - 12, OpticalElement.transform.position.y);
            LRInteract = false;
            LRIndex = 0;

            //How close the user can be to the correction position of the endpoint of the Light ray. Default is 5%
            ErrorTolerance = 0.015F;

            //Look I know this is a dumb way to initialize it but I'm lazy right now
            texts = new string[] { "This is a convex, or converging, lens.", "It is called a converging lens because rays of light that pass through the lens converge at the lens' focal point.",
                "To understand how light refracts through a convex lens,", "Imagine that the lens is really just a series of prisms stacked on top of each other.",
                "When light passes through a prism, it always bends towards the base of the prism.", "These rays of light form images where they intersect",
                "This method of representing image formation is known as ray-tracing.", "To repeat this process, first draw a ray that propagates from the object, to the lens, parallel to the optical axis",
                "This ray will pass through the focal point of the lens", "Next, draw a ray that passes through the optical center of the lens. This will be undeviated by the lens",
                "Finally, draw a ray that passes through the left focal point of the lens", "This ray will emerge from the lens, parallel to the optical axis",
                "The image forms at the intersection of the three rays, and is inverted.", "To find the image location, the Thin Lens Equation can be used", "continue", "15", "16", "17"};


            promptPanelText = GameObject.Find("PromptPanelText");
            panelText = promptPanelText.GetComponent<Text>();
            counter = 1;
            panelText.text = texts[0];


        }

        private void Update()
        {
            Interact();
            print(Input.touchCount);
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

                    //Disable object distance text for the purpose of this portion of the tutorial
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
                    print(13);
                    ObjectArrow.SetActive(true);
                    ObjectArrow.GetComponent<ObjectArrowControls>().UseControls(false);
                    GameObject[] ProgrammableALRS = GameObject.FindGameObjectsWithTag("ProgrammableALR");
                    for (int i = 0; i < ProgrammableALRS.Length; i++)
                    {
                        Destroy(ProgrammableALRS[i]);
                    }
                    EquationPanelAnimator.SetBool("toggleMenu", true);
                    ImageDistanceText.SetActive(true);
                    ObjectArrow.transform.GetChild(1).gameObject.SetActive(true);
                    //Reposition prompt panel since the equation panel will block it when it comes down
                    transform.position -= new Vector3(0, 650, 0);

                    break;

                case 14:
                    EquationPanelAnimator.SetBool("toggleMenu", false);
                    transform.position += new Vector3(0, 650, 0);
                    break;

                case 15:
                    InitializeLineRenderers();
                    CurrentRayIndex = 0;
                    LRInteract = true;
                    break;

                case 16:
                    InitializeLineRenderers();
                    LRInteract = true;
                    CurrentRayIndex = 1;
                    break;

                case 17:
                    InitializeLineRenderers();
                    LRInteract = true;
                    CurrentRayIndex = 2;
                    break;

                case 18:
                    ImageArrow.SetActive(true);
                    break;




            }

            if (counter < texts.Length)
            {
                panelText.text = texts[counter];
            }
            counter++;


        }

        bool CheckPointLocation(Vector3 point, int index)
        {

            Vector3[] CorrectPosition = CalculatePositions();
            //If the values are within ErrorTolerance % of each other, it is considered correct. This is important since, on a phone screen, space is not optimal, so you need some allowances.
            //Note that if the F is not in front of the 1.00, Unity will cast it to an int.
            bool XCorrect = Mathf.Abs(point.x / CorrectPosition[index].x) < 1.00F + ErrorTolerance && Mathf.Abs(point.x / CorrectPosition[index].x) > 1.00F - ErrorTolerance;
            bool YCorrect = Mathf.Abs(point.y / CorrectPosition[index].y) < 1.00F + ErrorTolerance && Mathf.Abs(point.y / CorrectPosition[index].y) > 1.00F - ErrorTolerance;
            if (XCorrect && YCorrect)
            {
                return true;
            }
            else
            {
                return false;
            }





        }
        Vector3[] CalculatePositions()
        {
            Vector3[] points = new Vector3[2];

            switch (CurrentRayIndex)
            {
                //Parallel Ray
                case 0:
                    points[0] = new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F);
                    points[1] = new Vector3(OpticalElement.transform.position.x + 12, OpticalElement.transform.position.y);
                    break;

                //Optical Center Ray
                case 1:
                    points[0] = OpticalElement.transform.position;
                    points[1] = points[0];
                    break;

                //Focal Point Ray
                case 2:
                    points[0] = CalculateFinalPosition();
                    points[1] = new Vector3(FocalPoint.x, CalculateFinalPosition().y, CalculateFinalPosition().z);
                    break;



            }
            return points;
        }
        void Interact()
        {
            if (LRInteract)
            {
                Vector3 point;
                if (Input.touchCount < 1)
                {
                    point = new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F);
                }
                else 
                {
                    point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
                CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, point);

                //LRIndex starts at 1 since the position of the first vertex of the LR is at the object arrow
                switch (CurrentRayIndex)
                {
                    //Parallel Ray
                    case 0:
                        switch (LRIndex)
                        {
                            case 1:
                                if (CheckPointLocation(point, 0))
                                {
                                    CurrentRay.GetComponent<SetLRPoints>().SetVisible(false);
                                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F), 0.5F);
                                    CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F));
                                    LRIndex++;
                                    //This just delays making the LR visible so that we can see the animation of the animated line renderer
                                    StartCoroutine(ExecuteAfterTime(0.5F));
                                    CurrentRay.GetComponent<SetLRPoints>().SetNumLRPoints(3);


                                }
                                break;


                            case 2:
                                if (CheckPointLocation(point, 1))
                                {
                                    CurrentRay.GetComponent<SetLRPoints>().SetVisible(false);
                                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(1000 * (FocalPoint - new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F)), 100);
                                    CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, 1000 * (FocalPoint - new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F)));
                                    LRInteract = false;
                                    StartCoroutine(ExecuteAfterTime(0.5F));
                                    onClick();
                                }
                                break;
                        }
                        break;
                    //Optical Center Ray
                    case 1:
                        CurrentRay.GetComponent<SetLRPoints>().SetNumLRPoints(2);
                        if (CheckPointLocation(point, 0))
                        {
                            CurrentRay.GetComponent<SetLRPoints>().SetVisible(false);
                            CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, 1000 * (OpticalElement.transform.position - new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F)));
                            CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(1000 * (OpticalElement.transform.position - new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F)), 100);
                            LRInteract = false;
                            StartCoroutine(ExecuteAfterTime(0.5F));
                            onClick();
                        }
                        break;

                    //Focal Point Ray
                    case 2:
                        switch (LRIndex)
                        {
                            case 1:
                                if (CheckPointLocation(point, 0))
                                {
                                    print(CalculateFinalPosition());
                                    CurrentRay.GetComponent<SetLRPoints>().SetVisible(false);
                                    CurrentRay.GetComponent<SetLRPoints>().SetNumLRPoints(3);
                                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CalculateFinalPosition(), 0.5F);
                                    CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, CalculateFinalPosition());
                                    LRIndex++;
                                    StartCoroutine(ExecuteAfterTime(0.5F));
                                }
                                break;

                            case 2:
                                if (CheckPointLocation(point, 1))
                                {
                                    CurrentRay.GetComponent<SetLRPoints>().SetVisible(false);
                                    CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, new Vector3 (1000, CalculateFinalPosition().y, CalculateFinalPosition().z));
                                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(1000, CalculateFinalPosition().y, CalculateFinalPosition().z), 25);
                                    LRInteract = false;
                                    StartCoroutine(ExecuteAfterTime(0.5F));
                                    onClick();
                                }
                                break;
                        }
                        break;
                }


            }
        }

        IEnumerator ExecuteAfterTime(float time)
        {
            if (isCoroutineExecuting)
                yield break;

            isCoroutineExecuting = true;

            yield return new WaitForSeconds(time);

            CurrentRay.GetComponent<SetLRPoints>().SetVisible(true);

            isCoroutineExecuting = false;
        }
        private float AngleBetween(Vector3 From, Vector3 To)
        {
            float sign = Mathf.Sign(Vector3.Cross(From, To).z);
            float angle = Vector3.Angle(From, To);

            return angle * sign;
        }

        //This is calculated using the World Space
        private Vector3 CalculateFinalPosition()
        {
            float FinalX = OpticalElement.transform.position.x;
            float FinalY = CurrentRay.transform.position.y + 1.32F + Mathf.Abs(OpticalElement.transform.position.x - CurrentRay.transform.position.x)
                * Mathf.Tan(Mathf.Deg2Rad * AngleBetween(Vector3.right, FocalPointLeft - new Vector3(CurrentRay.transform.position.x, CurrentRay.transform.position.y + 1.32F))); 
            float FinalZ = OpticalElement.transform.position.z;

            return new Vector3(FinalX, FinalY, FinalZ);
        }

        private void InitializeLineRenderers()
        {
            CurrentRay = Instantiate(ProgrammableLR, ObjectArrow.transform.position, Quaternion.identity, Root.transform);
            CurrentALRRay = Instantiate(ProgrammableALR, ObjectArrow.transform.position, Quaternion.identity, Root.transform);
            CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
            CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F), 0);
            CurrentRay.GetComponent<SetLRPoints>().InitializeLR();
            CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(0, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F));
            LRIndex = 1;

        }
    }
}



