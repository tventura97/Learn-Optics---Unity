using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer
{
    public class LearnMirrorsScene : MonoBehaviour {

        public GameObject AnimatedLightEmitter;
        public GameObject ProgrammableALR;
        public GameObject ProgrammableLR;
        public GameObject PlaneMirror;
        public GameObject ConcaveMirror;
        public GameObject ConvexMirror;
        public bool isReflective;
        public bool isConcaveReflective;
        public bool isConvexReflective;
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
        GameObject FocalLengthBar;
        RaycastHit OpticalElementHit;
        InputField ObjectDistanceIF;
        InputField ImageDistanceIF;
        InputField FocalLengthIF;
        InputField mObjectDistanceIF;
        InputField mImageDistanceIF;
        InputField MagnificationIF;
        Animator MainCamera;
        Animator PrismHolder;
        Animator EquationPanelAnimator;
        Text panelText;
        Button GenerateRaysButton;
        Button GenerateQuizButton;
        Button ResetRaysButton;
        Button ThinLensButton;
        Button MagnificationButton;
        Button ResetInputFields;
        GameObject SkipLessonButton;
        Toggle PlaySceneToggle;
        Vector3 FocalPoint;
        Vector3 FocalPointLeft;
        Vector3 HitOne;
        Vector3 HitTwo;
        Vector3 ReflectedDirection;
        RaycastHit MirrorHit;
        string[] texts;
        float ErrorTolerance;
        float FocalLength;
        float LRScalingFactor;
        bool LRInteract;
        bool isCoroutineExecuting;
        int LRIndex;
        int CurrentRayIndex;
        int promptNumber;
        int counter;

        void Start()
        {
            texts = new string[] { "Welcome to the mirrors lesson.", "A mirror is an object that reflects light. There are many different kinds of mirrors.", "All mirrors follow the law of " +
                "reflection. That is, the angle of reflection is equal to the angle incident." , "This is a plane mirror.", "Plane mirrors create virtual, upright images on the opposite side of the object",
                "The image is identical to the object, and is the same distance from the mirror as the object.",

                "This is a concave mirror. It is capable of forming many different types of images.", "If we are past the " +
                "center of curvature, C, concave mirrors form inverted, diminished, real images in front of the mirror.", "If we are between the focal point, F, and C, the image formed is real, inverted, magnified and in front of the mirror.", "If we are in front of F, an upright, magnified virtual image forms behind the mirror.",
                "The process of drawing rays to determine image formation is known as ray-tracing.", "First draw a ray from the object parallel to the optical axis. This will be reflected to pass through the focal point of the mirror.", "Next, draw a ray that passes through the focal point of the mirror. " +
                "this will be reflected to be parallel to the optical axis.", "The image forms at the intersection of the two rays.", "If the object is in front of the focal point, the first ray is the same.", "Then, draw a ray that hits the optical center of the mirror. This ray will reflect according to the law of reflection.",
                "Trace these rays backwards. The virtual image forms at the intersection of those rays.","Let's try an example.", "Continue when the image has formed.", "Using the information provided, the image location can be calculated using the mirror equation.",
                "The object distance is given, the focal length is 15.0m. Concave mirrors have positive focal lengths.", "Plug these values into the equation and calculate the image's position.",
                "Now that we know the image position, we can calculate its size, or how much it was magnified by the mirror.", "The lateral magnification is just the negative ratio of the image distance to the object distance.",

                "This is a convex mirror. It's like a concave mirror, but flipped horizontally.", "These mirrors always form upright, diminished, virtual images behind the mirror.", "The ray-tracing process for convex mirrors is very similar to the concave mirror process.",
                "First, draw a ray parallel to the optical axis. It is reflected away from the right focal point.", "Next, draw a ray that approaches the right focal point. It will be reflected to be parallel to the optical axis.", "Trace these rays backwards. An image forms at the intersection.",

                "Let's try an example.", "Press continue when the image forms", "Like with the concave mirror, we can use the mirror equation to calculate image position. Note that convex mirrors have negative focal lengths.", "Since the image is virtual its distance is negative.", "Using these values, calculate its magnification.", "Convex mirrors form diminished images.", "You have reached the end of the lesson."};
            InitializeObjects();
            FocalLength = Mathf.Abs(GameObject.Find("F1").transform.position.x - OpticalElement.transform.position.x);
            FocalPointLeft = new Vector3(OpticalElement.transform.position.x - FocalLength, OpticalElement.transform.position.y);
            FocalPoint = new Vector3(OpticalElement.transform.position.x + FocalLength, OpticalElement.transform.position.y);
            FocalPointMarkerHolder.GetComponent<Animator>().enabled = false;
            FocalPointMarkerHolder.transform.position = Root.transform.position;

            SetDefaults();
            counter = 0;

            LRScalingFactor = 10000;



            OnClick();

        }

        void Update() {

            //This has to constantly be updated because the mirrors will change
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");

        }

        private void InitializeObjects()
        {
            Root = GameObject.Find("Root");
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            GenerateRaysButton = GameObject.Find("GenerateRaysButton").GetComponent<Button>();
            GenerateQuizButton = GameObject.Find("GenerateQuizButton").GetComponent<Button>();
            MainCamera = GameObject.Find("Main Camera").GetComponent<Animator>();
            ObjectArrow = GameObject.Find("ObjectArrow");
            ImageArrow = GameObject.Find("ImageArrow");
            FocalPointMarkerHolder = GameObject.Find("FocalPointMarkerHolder");
            FocalPointMarkerHolder.transform.position = Root.transform.position;
            EquationPanelAnimator = GameObject.Find("EquationPanel").GetComponent<Animator>();
            ImageDistanceText = GameObject.Find("ImageDistanceText");
            ObjectDistanceIF = GameObject.Find("ObjectDistanceInputField").GetComponent<InputField>();
            ImageDistanceIF = GameObject.Find("ImageDistanceInputField").GetComponent<InputField>();
            FocalLengthIF = GameObject.Find("FocalLengthInputField").GetComponent<InputField>();
            mObjectDistanceIF = GameObject.Find("mObjectDistanceIF").GetComponent<InputField>();
            mImageDistanceIF = GameObject.Find("mImageDistanceIF").GetComponent<InputField>();
            MagnificationIF = GameObject.Find("MagnificationIF").GetComponent<InputField>();
            FocalLengthBar = GameObject.Find("FocalLengthBar");
            GenerateQuizButton = GameObject.Find("GenerateQuizButton").GetComponent<Button>();
            ThinLensButton = GameObject.Find("CalculateThinLensButton").GetComponent<Button>();
            MagnificationButton = GameObject.Find("CalculateMagnificationButton").GetComponent<Button>();
            panelText = GameObject.Find("PromptPanelText").GetComponent<Text>();
            ResetInputFields = GameObject.Find("ResetButton").GetComponent<Button>();
            SkipLessonButton = GameObject.Find("SkipLessonButton");
            print("Objects Successfully Initialized");
        }


        public void SetDefaults()
        {
            ObjectArrow.SetActive(true);
            ImageArrow.SetActive(true);
            ImageDistanceText.SetActive(true);
            FocalPointMarkerHolder.SetActive(true);
            FocalLengthBar.SetActive(false);
        }


        public void OnClick()
        {

            //Note that given the size of the objectarrow, the largest angle you can rotate the ALR
            switch (counter)
            {
                //Set defaults for the mirror learning scene.
                case 0:
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Start";
                    GameObject.Find("GenerateQuizButton").GetComponent<GenerateQuizScript>().SetPlayScene(true);
                    break; 
                case 1:
                    SkipLessonButton.SetActive(false);
                    ObjectArrow.SetActive(false);
                    ImageArrow.SetActive(false);
                    FocalLengthBar.SetActive(false);
                    ImageDistanceText.SetActive(false);
                    FocalPointMarkerHolder.GetComponent<Animator>().enabled = false;
                    FocalPointMarkerHolder.transform.position = new Vector3(0, 0, 0);
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Continue";
                    break;

                case 2:
                    //Create rays that radiate from a point
                    for (int i = -30; i <= 30; i += 10)
                    {
                        CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(OpticalElement.transform.position.x - 20, OpticalElement.transform.position.y), Quaternion.Euler(0, 0, i));
                        CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                        CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                        CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection(), 10000000/100);
                    }
                    break;

                case 3:
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    break;

                case 4:
                    ObjectArrow.SetActive(true);
                    ImageArrow.SetActive(true);
                    ImageArrow.transform.position = new Vector3(0, 0, 0);
                    //Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                    //Store hit point
                    HitOne = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection(), 10000000 / 100);

                    //Ray that points at optical center
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, OpticalElement.transform.position - CurrentALRRay.transform.position)));
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();

                    //Store hit point
                    HitTwo = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();


                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection(), 10000000 / 100);
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Generate";
                    break;

                case 5:
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Next Mirror";
                    //Reverse Direction of rays
                    //Reversed Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    //Change color of ray to black
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 0, 0);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 0, 0);

                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(HitOne, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(-CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection(), 10000000 / 100);

                    //Reversed OpticalCenterRay
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0,0, AngleBetween(Vector3.right, OpticalElement.transform.position - CurrentALRRay.transform.position)));
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    //Change color of ray to black
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 0, 0);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 0, 0);

                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(HitTwo, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(-CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection(), 10000000 / 100);

                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                    break;

                case 6:
                    //Create concave mirror, destroy plane mirror
                    Destroy(GameObject.FindGameObjectWithTag("OpticalElement"));
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Continue";
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    CurrentALRRay = Instantiate(ConcaveMirror, Root.transform.position, Quaternion.Euler(new Vector3 (0, 90, 90)));
                    ObjectArrow.SetActive(false);
                    ImageArrow.transform.position = new Vector3(0, 0, 0);
                    FocalPointMarkerHolder.transform.position = OpticalElement.transform.position;

                    for (int i = -15; i <= 15; i+=5)
                    {
                        CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(OpticalElement.transform.position.x - 50, OpticalElement.transform.position.y + i), Quaternion.identity);
                        CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                        CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                        CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (FocalPointLeft - CurrentALRRay.GetComponent<SetPoints>().GetHitPoint()), LRScalingFactor/10);
                    }
                    //Create ALRs
                    break;

                case 7:
                    //Past C Image formation
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    ObjectArrow.SetActive(true);
                    ObjectArrow.transform.position = new Vector3(OpticalElement.transform.position.x - 2*FocalLength - 5, OpticalElement.transform.position.y + 4.84F);
                    //Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                    //Store hit point
                    HitOne = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (FocalPointLeft - CurrentALRRay.GetComponent<SetPoints>().GetHitPoint()), LRScalingFactor / 10);

                    //Focal Point Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, FocalPointLeft - CurrentALRRay.transform.position)));
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();

                    //Store hit point
                    HitTwo = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3 (-1000, HitTwo.y), 10);
                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                    break;

                case 8:
                    //Between F and C Image formation
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    ObjectArrow.SetActive(true);
                    ObjectArrow.transform.position = new Vector3(OpticalElement.transform.position.x - FocalLength - 7, OpticalElement.transform.position.y + 4.84F);
                    //Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                    //Store hit point
                    HitOne = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (FocalPointLeft - CurrentALRRay.GetComponent<SetPoints>().GetHitPoint()), LRScalingFactor / 10);

                    //Focal Point Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, FocalPointLeft - CurrentALRRay.transform.position)));
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();

                    //Store hit point
                    HitTwo = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(-1000, HitTwo.y), 10);
                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                    break;

                case 9:
                    //Past F (Virtual Image)
                    ImageArrow.transform.position = new Vector3(0, 0, 0);
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    ObjectArrow.transform.position = new Vector3(OpticalElement.transform.position.x - 10, OpticalElement.transform.position.y + 4.84F);
                    //Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                    //Store hit point
                    HitOne = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (FocalPointLeft - HitOne), LRScalingFactor / 10);

                    //Ray that points at optical center
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, OpticalElement.transform.position - CurrentALRRay.transform.position)));
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();

                    //Store hit point
                    HitTwo = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(OpticalElement.transform.position, 0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection(), LRScalingFactor/100);

                    //Store this for the reversed ray
                    ReflectedDirection = CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection();

                    //Reverse rays and turn them black to show the formation of the virtual image

                    //Reverse direction of Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(HitOne, 0);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (HitOne - FocalPointLeft), LRScalingFactor / 10);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);

                    //Reverse Optical Center Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(HitTwo.x -0.7F, HitTwo.y) , 0);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * - new Vector3(ReflectedDirection.x, ReflectedDirection.y + 10), LRScalingFactor/1000);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);
                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                    transform.position -= new Vector3(0, 650, 0);
                    break;

                case 10:
                    //Advance Dialogue
                    transform.position += new Vector3(0, 650, 0);
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    ImageArrow.transform.position = new Vector3(0, 0, 0);

                    break;

                    
                case 11:
                    //Step one: Parallel Ray
                    //Move Object arrow somewhere else (Should be between F and C)
                    ObjectArrow.transform.position = new Vector3(OpticalElement.transform.position.x - 25, ObjectArrow.transform.position.y);
                    //Between F and C Image formation
                    ObjectArrow.SetActive(true);
                    ObjectArrow.transform.position = new Vector3(OpticalElement.transform.position.x - FocalLength - 7, OpticalElement.transform.position.y + 4.84F);
                    //Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                    //Store hit point
                    HitOne = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (FocalPointLeft - CurrentALRRay.GetComponent<SetPoints>().GetHitPoint()), LRScalingFactor / 10);
                    break;

                case 12:
                    //Step Two: FocalPoint Ray (Mirrors don't use optical center ray)

                    //Focal Point Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, FocalPointLeft - CurrentALRRay.transform.position)));
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();

                    //Store hit point
                    HitTwo = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(-1000, HitTwo.y), 10);
                    break;

                case 13:
                    //Form Image
                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                    break;

                case 14:
                    //Ray-Tracing in case object is in front of focal point 
                    ImageArrow.transform.position = new Vector3(0, 0, 0);
                    ObjectArrow.transform.position = new Vector3(OpticalElement.transform.position.x - 10, ObjectArrow.transform.position.y);
                    //In front of F (Virtual Image)
                    ImageArrow.transform.position = new Vector3(0, 0, 0);
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    ObjectArrow.transform.position = new Vector3(OpticalElement.transform.position.x - 10, OpticalElement.transform.position.y + 4.84F);
                    //Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                    //Store hit point
                    HitOne = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (FocalPointLeft - HitOne), LRScalingFactor / 10);
                    break;

                case 15:
                    //Ray that points at optical center
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, OpticalElement.transform.position - CurrentALRRay.transform.position)));
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();

                    //Store hit point
                    HitTwo = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(OpticalElement.transform.position, 0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection(), LRScalingFactor / 100);


                    //Store this for the reversed ray
                    ReflectedDirection = CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection();
                    break;

                case 16:
                    //Reverse Rays and form image
                    //Reverse direction of Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(HitOne, 0);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (HitOne - FocalPointLeft), LRScalingFactor / 10);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);

                    //Reverse Optical Center Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(HitTwo.x - 0.7F, HitTwo.y), 0);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * -new Vector3(ReflectedDirection.x, ReflectedDirection.y + 10), LRScalingFactor / 1000);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);
                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                    transform.position -= new Vector3(0, 650, 0);

                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                    break;

                case 17:
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    transform.position += new Vector3(0, 650, 0);
                    break;

                case 18:
                    GameObject.Find("GenerateQuizButton").GetComponent<GenerateQuizScript>().SetPlayScene(false);
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().SetMirror(1);
                    GenerateQuizButton.onClick.Invoke();
                    break;

                case 19:
                    //Advance Dialogue
                    break;

                case 20:
                    FocalLengthBar.SetActive(true);
                    break;

                case 21:
                    //Bring down menu, let them plug in the values
                    EquationPanelAnimator.SetBool("toggleMenu", true);
                    transform.position -= new Vector3(0, 650, 0);
                    break;

                case 22:
                    //Call the calculate button so if they didn't plug in any values (or put the wrong ones in), it'll just pull the values from the scene.
                    FocalLengthBar.GetComponent<Animator>().SetBool("ScaleUpLeftSidee", true);
                    ThinLensButton.onClick.Invoke();
                    break;

                case 23:
                    //Call the magnification button for the same reason as above
                    MagnificationButton.onClick.Invoke();
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Next Mirror";
                    break;

                case 24:

                    EquationPanelAnimator.SetBool("toggleMenu", false);
                    transform.position += new Vector3(0, 650, 0);
                    //Destroy Current Optical Element
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Continue";
                    Destroy(GameObject.FindGameObjectWithTag("OpticalElement"));
                    FocalLengthBar.transform.position = new Vector3(10000, 10000, 10000);
                    //Replace with Convex Mirror
                    Instantiate(ConvexMirror, Root.transform.position, Quaternion.Euler(0, -90, -90), Root.transform);
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    ObjectArrow.SetActive(false);
                    ImageArrow.transform.position = new Vector3(0, 0, 0);
                    for (int i = -15; i <= 15; i += 5)
                    {
                        CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(OpticalElement.transform.position.x - 50, OpticalElement.transform.position.y + i), Quaternion.identity);
                        CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                        CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                        CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * -(FocalPoint - CurrentALRRay.GetComponent<SetPoints>().GetHitPoint()), LRScalingFactor / 10);
                    }
                    break;

                case 25:
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Generate";
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    ObjectArrow.SetActive(true);
                    ObjectArrow.transform.position = new Vector3(OpticalElement.transform.position.x - FocalLength - 7, OpticalElement.transform.position.y + 4.84F);
                    //Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                    //Store hit point
                    HitOne = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * -(FocalPoint - CurrentALRRay.GetComponent<SetPoints>().GetHitPoint()), LRScalingFactor / 10);

                    //Focal Point Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, FocalPoint - CurrentALRRay.transform.position)));
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();

                    //Store hit point
                    HitTwo = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(-1000, HitTwo.y), 10);
                    break;

                case 26:
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Continue";
                    //Reverse rays and turn them black to show the formation of the virtual image
                    //Reverse direction of Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(HitOne, 0);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * -(HitOne - FocalPoint), LRScalingFactor / 10);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);

                    //Reverse Focal Point Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(HitTwo.x - 0.7F, HitTwo.y), 0);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(1000, HitTwo.y),  10);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);
                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                    break;

                case 27:
                    ImageArrow.transform.position = new Vector3(0, 0, 0);
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    //Step one: parallel ray
                    ObjectArrow.transform.position = new Vector3(OpticalElement.transform.position.x - FocalLength - 7, OpticalElement.transform.position.y + 4.84F);
                    //Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                    //Store hit point
                    HitOne = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * -(FocalPoint - CurrentALRRay.GetComponent<SetPoints>().GetHitPoint()), LRScalingFactor / 10);
                    break;

                case 28:
                    //Part two: Focal Point ray

                    //Focal Point Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, FocalPoint - CurrentALRRay.transform.position)));
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();

                    //Store hit point
                    HitTwo = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetPointAtHit(0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(-1000, HitTwo.y), 10);
                    break;

                case 29:
                    //Reverse rays and turn them black to show the formation of the virtual image
                    //Reverse direction of Parallel Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(HitOne, 0);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * -(HitOne - FocalPoint), LRScalingFactor / 10);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);

                    //Reverse Focal Point Ray
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(HitTwo.x - 0.7F, HitTwo.y), 0);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(1000, HitTwo.y), 10);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);
                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                    break;

                case 30:
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().SetMirror(0);
                    GenerateQuizButton.onClick.Invoke();
                    break;

                case 31:
                    ResetInputFields.onClick.Invoke();
                    EquationPanelAnimator.SetBool("toggleMenu", true);
                    transform.position -= new Vector3(0, 650, 0);
                    break;

                case 32:
                    ThinLensButton.onClick.Invoke();
                    break;

                case 33:
                    //Advance dialogue
                    break;

                case 34:
                    MagnificationButton.onClick.Invoke();
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "End";
                    break;

                case 35:
                    EquationPanelAnimator.SetBool("toggleMenu", false);
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().isMirrorSet = false;
                    SkipLessonButton.SetActive(true);
                    SkipLessonButton.GetComponent<Button>().onClick.Invoke();
                    break;
            }

            if (counter < texts.Length)
            {
                panelText.text = texts[counter];
            }
            else
            {
                panelText.text = "case " + counter;
            }
            counter++;
        }

        private float AngleBetween(Vector3 From, Vector3 To)
        {
            float sign = Mathf.Sign(Vector3.Cross(From, To).z);
            float angle = Vector3.Angle(From, To);

            return angle * sign;
        }
    }
}
