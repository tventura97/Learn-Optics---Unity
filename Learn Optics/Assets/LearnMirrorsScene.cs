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
        Toggle PlaySceneToggle;
        Vector3 FocalPoint;
        Vector3 FocalPointLeft;
        Vector3 HitOne;
        Vector3 HitTwo;
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
            texts = new string[] { "Welcome to the mirrors lesson.", "A mirror is an object that reflects light, and there are many different kinds.", "All mirrors follow the law of " +
                "reflection. That is, the angle of reflection is equal to the angle incident." , "This is a plane mirror.", "Plane mirrors create virtual, upright images on the opposite side of the object",
                "The image is identical to the object, and is the same distance from the mirror as the object.", "This is a concave mirror. It is capable of forming many different types of images.", "If we are past the " +
                "center of curvature, C, concave mirrors form inverted, real images.", "If we are between the focal point, F, and C, the image formed is real, inverted, and larger than the object.", "If we are in front of F, no image is formed", "Let's try an example."};
            InitializeObjects();
            SetDefaults();
            counter = 0;
            FocalLength = 15;
            FocalPointLeft = new Vector3(OpticalElement.transform.position.x - FocalLength, OpticalElement.transform.position.y);
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
            PlaySceneToggle = GameObject.Find("PlaySceneToggle").GetComponent<Toggle>();
            panelText = GameObject.Find("PromptPanelText").GetComponent<Text>();
            ResetInputFields = GameObject.Find("ResetButton").GetComponent<Button>();
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
                    ObjectArrow.SetActive(false);
                    ImageArrow.SetActive(false);
                    FocalLengthBar.SetActive(false);
                    ImageDistanceText.SetActive(false);
                    FocalPointMarkerHolder.GetComponent<Animator>().enabled = false;
                    FocalPointMarkerHolder.transform.position = new Vector3(0, 0, 0);
                    break; 
                case 1:
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
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (FocalPointLeft - CurrentALRRay.GetComponent<SetPoints>().GetHitPoint()), LRScalingFactor / 10);

                    //Ray that points at optical center
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, OpticalElement.transform.position - CurrentALRRay.transform.position)));
                    CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.transform.position, 0);
                    CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();

                    //Store hit point
                    HitTwo = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(OpticalElement.transform.position, 0.5F);
                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection(), LRScalingFactor/100);
                    break;

                case 10:
                    //Advance Dialogue
                    break;

                case 11:
                    GameObject.Find("GenerateQuizButton").GetComponent<GenerateQuizScript>().SetPlayScene(false);
                    GenerateQuizButton.onClick.Invoke();
                    break;

                case 12:
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
