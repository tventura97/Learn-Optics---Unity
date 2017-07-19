using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DigitalRuby.AnimatedLineRenderer
{
    public class ConcaveLearningScript : MonoBehaviour
    {


        public GameObject AnimatedLightEmitter;
        public GameObject ProgrammableALR;
        public GameObject ProgrammableLR;
        public Material Laser;
        public Material GreenLaser;
        public Material ParticleLaser;
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
        Toggle PlaySceneToggle;
        Vector3 FocalPoint;
        Vector3 FocalPointLeft;
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
            InitializeObjects();
            texts = new string[] {"Welcome to the concave lens lesson.", "This is a concave, or diverging, lens.", "It's called a diverging lens because light rays that pass through the lens diverge from its focal point. ",
                    "To better understand how light refracts through a concave lens,", "Picture the lens as a series of prisms.", "Light always refracts towards the base of a prism.",
                    "These rays of light form virtual images.", "These images are called virtual because light beams diverge from them rather than converge at them.", "Your brain " +
                    "assumes that the light rays originate from a point in front of the actual object.", "You will see an image at the point it thinks the light rays would have emanated from.", "This method of image formation is known as ray-tracing.",
                    "To repeat this process, first draw a ray that is parallel to the optical axis.", "This will diverge from the left focal point.", "Next, draw a ray that passes through the optical center. This will emerge undeviated.",
                    "Finally, draw a ray that approaches the right focal point of the lens.", "This will emerge parallel to the optical axis.", "TRACE RAYS BACKWARDS, INSERT MORE APPROPRIATE DIALOGUE HERE", "" };

            panelText.text = texts[0];
            SetDefaults();
            counter = 0;
            FocalLength = 12;
            FocalPointLeft = new Vector3(OpticalElement.transform.position.x - FocalLength, OpticalElement.transform.position.y);
            FocalPoint = new Vector3(OpticalElement.transform.position.x + FocalLength, OpticalElement.transform.position.y);
            //This just extends the length of the linerenderer. LineRenderer draws a line from point a to point b. The functions used here only get the trajectory of the light beams.
            //We need to extend the length of it or it won't be visible.
            LRScalingFactor = 10000000;

            //Disable animator until proper animations are created. Doesn't matter anyway. Animations are extraneous
            ObjectArrow.GetComponent<Animator>().enabled = false;
            ImageArrow.GetComponent<Animator>().enabled = false;
            //Start 
            OnClick();
        }

        public void OnClick()
        {
            print(counter);
            switch (counter)
            {
                case 0:
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Start";
                    break;
                case 1:
                    //Disable all arrows for purpose of demonstration
                    ObjectArrow.SetActive(false);
                    ImageArrow.SetActive(false);
                    ImageDistanceText.SetActive(false);
                    FocalPointMarkerHolder.SetActive(true);
                    FocalLengthBar.SetActive(false);
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Continue";
                    break;
                case 2:
                    //Instantiate ALRs to demonstrate how light rays refract through concave lens

                    for (int i = -9; i <= 9; i += 3)
                    {
                        CurrentRay = Instantiate(ProgrammableALR, new Vector3(OpticalElement.transform.position.x - 50, OpticalElement.transform.position.y + i), Quaternion.identity);
                        CurrentRay.GetComponent<SetPoints>().InitializeALR();
                        CurrentRay.GetComponent<SetPoints>().SetLinePoint(CurrentRay.transform.position, 0);
                        CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y), 1);
                        CurrentRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y) - FocalPointLeft), LRScalingFactor / 10);
                    }

                    break;

                case 3:
                    //Do nothing, this just advances dialogue.
                    break;

                case 4:
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    MainCamera.SetBool("Pan_TopofLens", true);
                    PrismHolder.SetBool("triggerPrism", true);

                    break;
                case 5:
                    //Instantiate ALRs to show how light rays refract through concave lens prisms.

                    for (int i = 6; i <= 9; i += 3)
                    {
                        CurrentRay = Instantiate(ProgrammableALR, new Vector3(OpticalElement.transform.position.x - 50, OpticalElement.transform.position.y + i), Quaternion.identity);
                        CurrentRay.GetComponent<SetPoints>().InitializeALR();
                        CurrentRay.GetComponent<SetPoints>().SetLinePoint(CurrentRay.transform.position, 0);
                        CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y), 1);
                        CurrentRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y) - FocalPointLeft), LRScalingFactor / 10);
                    }
                    break;

                case 6:
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    MainCamera.SetBool("Pan_TopofLens", false);
                    PrismHolder.SetBool("triggerPrism", false);
                    ObjectArrow.SetActive(true);
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Generate";
                    break;

                case 7:
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Continue";
                    //Parallel Ray
                    CurrentRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(CurrentRay.transform.position, 0);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y), 1);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y) - FocalPointLeft), LRScalingFactor / 10);

                    //Optical Center Ray
                    CurrentRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(CurrentRay.transform.position, 0);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (OpticalElement.transform.position - CurrentRay.transform.position), LRScalingFactor / 2);

                    //Focal Point Ray
                    CurrentRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(CurrentRay.transform.position, 0);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, GetFinalY()), 1);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint((new Vector3(LRScalingFactor, GetFinalY())), LRScalingFactor / 100);
                    break;

                case 8:
                    //Reverse direction of Parallel Ray
                    CurrentRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y), 0);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (FocalPointLeft - new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y)), LRScalingFactor / 10);
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);


                    //Reverse direction of FocalPoint Ray
                    CurrentRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, GetFinalY()), 1);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(-1000, GetFinalY()), 10);

                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Generate";
      
                    break;

                case 9:
                    //Form image
                    ImageArrow.SetActive(true);
                    //Disable the animator, it prevents us from altering the position of the ImageArrow.
                    ImageArrow.GetComponent<Animator>().enabled = false;
                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                    print(ImageArrow.GetComponent<ImageArrowGeneration>().GetPosition());
                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Continue";
                    break;

                case 10:
                    //Advance dialogue
                    break;

                case 11:
                    //Move ImageArrow out of view of scene
                    ImageArrow.transform.position = new Vector3(0, 0, 0);
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    //Parallel Ray 1st half
                    CurrentRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(CurrentRay.transform.position, 0);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y), 1);
                    break;

                case 12:
                    //Parallel Ray 2nd half
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y) - FocalPointLeft), LRScalingFactor / 10);
                    break;

                case 13:
                    //Optical Center Ray
                    CurrentRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(CurrentRay.transform.position, 0);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (OpticalElement.transform.position - CurrentRay.transform.position), LRScalingFactor / 2);
                    break;

                case 14:
                    //Focal Point Ray 1st half
                    CurrentRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(CurrentRay.transform.position, 0);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, GetFinalY()), 1);
                    break;

                case 15:
                    //Focal Point Ray 2nd half
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint((new Vector3(LRScalingFactor, GetFinalY())), LRScalingFactor / 100);
                    break;

                case 16:
                    //Reverse direction of Focal Point and Parallel Rays

                    //Reverse direction of Parallel Ray
                    CurrentRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y), 0);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (FocalPointLeft - new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y)), LRScalingFactor / 10);
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);


                    //Reverse direction of FocalPoint Ray
                    CurrentRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                    CurrentRay.GetComponent<SetPoints>().InitializeALR();
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                    CurrentRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                    CurrentRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, GetFinalY()), 1);
                    CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(-1000, GetFinalY()), 10);
                    break;

                case 17:
                    //Image forms
                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                    break;

                    
                case 18:
                    EquationPanelAnimator.SetBool("toggleMenu", true);
                    transform.position -= new Vector3(0, 650, 0);
                    break;

                case 19:
                    FocalLengthBar.SetActive(true);
                    //Get text of focal length bar and set it to focal length of object (-12)
                    FocalLengthBar.transform.GetChild(0).GetComponent<Text>().text = "-12m";
                    break;

                case 20:
                    //Input focal length in InputField
                    FocalLengthIF.text = "-12";
                    break;

                case 21:
                    //Input Object distance in InputField
                    ObjectDistanceIF.text = Mathf.Abs(ObjectArrow.transform.position.x - OpticalElement.transform.position.x).ToString("F1");
                    break;

                case 22:
                    //Calculate Image distance
                    ThinLensButton.onClick.Invoke();
                    break;

                case 23:
                    //Input Object distance in Magnification InputField
                    mObjectDistanceIF.text = ObjectDistanceIF.text;
                    break;

                case 24:
                    //Input Image distance in Magnification InputField
                    mImageDistanceIF.text = ImageDistanceIF.text;
                    break;

                case 25:
                    //Calculate Lateral Magnification
                    MagnificationButton.onClick.Invoke();
                    break;

                case 26:
                    //Begin example
                    //Set play scene bool to false so that the quiz button thinks the scene isn't playing
                    GameObject.Find("GenerateQuizButton").GetComponent<GenerateQuizScript>().SetPlayScene(false);
                    GenerateQuizButton.onClick.Invoke();
             
                    break;

            

            }

            if (counter < texts.Length)
            {
                panelText.text = texts[counter];
            }
            else if (counter >= texts.Length)
            {
                panelText.text = "case " + counter.ToString();
            }
            counter++;

        }
        private void InitializeObjects()
        {
            Root = GameObject.Find("Root");
            GenerateRaysButton = GameObject.Find("GenerateRaysButton").GetComponent<Button>();
            GenerateQuizButton = GameObject.Find("GenerateQuizButton").GetComponent<Button>();
            MainCamera = GameObject.Find("Main Camera").GetComponent<Animator>();
            PrismHolder = GameObject.Find("PrismHolder").GetComponent<Animator>();
            ObjectArrow = GameObject.Find("ObjectArrow");
            ImageArrow = GameObject.Find("ImageArrow");
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
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
        private float AngleBetween(Vector3 From, Vector3 To)
        {
            float sign = Mathf.Sign(Vector3.Cross(From, To).z);
            float angle = Vector3.Angle(From, To);

            return angle * sign;
        }

        private float GetFinalY()
        {
            return CurrentRay.transform.position.y + Mathf.Abs(OpticalElement.transform.position.x - CurrentRay.transform.position.x) / Mathf.Tan(Mathf.Deg2Rad * AngleBetween((FocalPoint - CurrentRay.transform.position), Vector3.down));
        }
    }
}