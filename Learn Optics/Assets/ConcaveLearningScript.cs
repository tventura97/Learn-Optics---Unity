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
                    "To better understand how light refracts through a concave lens,", "4", "5", "6", "7", "8"};
            panelText.text = texts[0];
            SetDefaults();
            counter = 0;
            FocalLength = 12;
            FocalPointLeft = new Vector3(OpticalElement.transform.position.x - FocalLength, OpticalElement.transform.position.y);

            //This just extends the length of the linerenderer. LineRenderer draws a line from point a to point b. The functions used here only get the trajectory of the light beams.
            //We need to extend the length of it or it won't be visible.
            LRScalingFactor = 10000000;

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
                    for (int i = -8; i <= 8; i+=4)
                    {
                        CurrentRay = Instantiate(ProgrammableALR, new Vector3(OpticalElement.transform.position.x - 100, OpticalElement.transform.position.y + i), Quaternion.identity);
                        CurrentRay.GetComponent<SetPoints>().InitializeALR();
                        CurrentRay.GetComponent<SetPoints>().SetLinePoint(CurrentRay.transform.position, 0);
                        CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y), 2);
                        CurrentRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (new Vector3(OpticalElement.transform.position.x, CurrentRay.transform.position.y) - FocalPointLeft), LRScalingFactor/10);
                    }
                    break;

                case 3:
                    break;
                case 4:
                    GenerateQuizButton.GetComponent<GenerateQuizScript>().DestroyAllLineRenderers();
                    break;
            }

            if (counter < texts.Length)
            {
                panelText.text = texts[counter];
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
    }
}