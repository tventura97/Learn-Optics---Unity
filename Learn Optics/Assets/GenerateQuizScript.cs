using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class starts the interactive ray-tracing activity. Note that the order of rays is ALWAYS parallel ray -> optical center ray -> focal point ray

namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class GenerateQuizScript : MonoBehaviour
    {
        public GameObject ConcaveMirror;
        public GameObject ConvexMirror;
        private GameObject ObjectArrow;
        private GameObject OpticalElement;
        private GameObject ImageArrow;
        private GameObject ImageDistanceText;
        private GameObject CurrentRay;
        private GameObject CurrentALRRay;
        private GameObject Root;
        private GameObject FocalPointMarkerHolder;
        private GameObject[] CurrentRays;
        private Vector3 HitOne;
        private Vector3 HitTwo;
        private Toggle PlaySceneToggle;
        private Animator EquationPanelAnimator;
        private Animator PopUpPromptAnimator;
        private Animator PopUpPromptTextAnimator;
        private Animator ImageInfoPanelAnimator;
        private float ErrorTolerance;
        private float TrajectoryErrorTolerance;
        private float LRScalingFactor;
        private bool isCoroutineExecuting;
        private bool isUICoroutineExecuting;
        private bool isLRInitializing;
        public bool isMirrorSet;
        private bool isConcaveReflective;
        private bool isConvexReflective;
        private bool isUserRendering;
        private int MirrorSelection;
        private Text ButtonText;
        private Text PopUpPromptText;
        public GameObject ProgrammableLR;
        public GameObject ProgrammableALR;
        public Vector3 FocalPoint;
        public Vector3 FocalPointLeft;
        public float OpticalElementType;
        public float FocalLength;
        public bool PlaySceneToggleState;
        public bool isInteractable;
        public bool isConcave;
        public bool isReflective;
        public int counter;
        public int CurrentRayIndex;
        public int LRIndex;
        int debugcounter;


        void Start()
        {
            InitializeObjects();
            //We select the focal length to be 12 because it is a decent distance from the lens that allows for a wide range of image formation without having to increase
            //Orthographic size of the camera by too much (Image/Object Arrows would become too small to interact with if the camera gets too large)

            //How close the user can be to the correction position of the endpoint of the Light ray. Default is 5%
            ErrorTolerance = 0.015F;
            TrajectoryErrorTolerance = 0.005F;
            //This just extends the length of the linerenderer. LineRenderer draws a line from point a to point b. The functions used here only get the trajectory of the light beams.
            //We need to extend the length of it or it won't be visible.
            LRScalingFactor = 10000000;
            counter = 0;
            isInteractable = false;
            isLRInitializing = false;
            isMirrorSet = false;
            debugcounter = 0;


        }

        private void Update()
        {
            InitializeObjects();
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            FocalPointMarkerHolder = GameObject.Find("FocalPointMarkerHolder");
            FocalLength = Mathf.Abs(GameObject.Find("F1").transform.position.x - OpticalElement.transform.position.x);
            FocalPoint = new Vector3(OpticalElement.transform.position.x + FocalLength, OpticalElement.transform.position.y);
            FocalPointLeft = new Vector3(OpticalElement.transform.position.x - FocalLength, OpticalElement.transform.position.y);
            isConcaveReflective = OpticalElement.GetComponent<Properties_Optical>().isConcaveReflective;
            isConvexReflective = OpticalElement.GetComponent<Properties_Optical>().isConvexReflective;
            ButtonText.enabled = true;
            ButtonText.text = "Generate Quiz";
        }
        private void InitializeObjects()
        {
            ObjectArrow = GameObject.Find("ObjectArrow");
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            ImageArrow = GameObject.Find("ImageArrow");
            ImageDistanceText = GameObject.Find("ImageDistanceText");
            Root = GameObject.Find("Root");
            EquationPanelAnimator = GameObject.Find("EquationPanel").GetComponent<Animator>();
            ButtonText = transform.GetChild(0).GetComponent<Text>();
            PopUpPromptAnimator = GameObject.Find("PopUpPrompt").GetComponent<Animator>();
            PopUpPromptTextAnimator = GameObject.Find("PopUpPromptText").GetComponent<Animator>();
            PopUpPromptText = GameObject.Find("PopUpPromptText").GetComponent<Text>();
            ImageInfoPanelAnimator = GameObject.Find("ImageInformationPanel").GetComponent<Animator>();
        }
        private void FixedUpdate()
        {
            if (isInteractable)
            {
                Interact();
            }
        }


        public void OnClick()
        {
            //Closes info panel
            ImageInfoPanelAnimator.SetBool("DisplayInfo", false);
            counter = 0;
            CurrentRayIndex = 0;
            LRIndex = 1;
            //Put this here. It moves around otherwise

            //If the scene is not playing, then do everything else. Otherwise, no functionality.
            if (!PlaySceneToggleState)
            {
                //This will destroy any and all forms of line renderers currently present in the scene.
                FocalPointMarkerHolder.GetComponent<Animator>().enabled = false;
                FocalPointMarkerHolder.transform.position = Root.transform.position;
                DestroyAllLineRenderers();
                ObjectArrow.SetActive(true);
                ImageArrow.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
                ObjectArrow.transform.GetComponent<ObjectArrowControls>().ResetALRs();
                //If the SetMirror() Function has not been called, assign a random value to the MirrorSelection.
                if (isReflective)
                {
                    Destroy(GameObject.FindGameObjectWithTag("OpticalElement"));
                    if (!isMirrorSet)
                    {
                        MirrorSelection = Random.Range(0, 2);
                    }
                    switch (MirrorSelection)
                    {
                        case 0:
                            Instantiate(ConvexMirror, Root.transform.position, Quaternion.Euler(0, -90, 90), Root.transform);
                            break;
                        case 1:
                            Instantiate(ConcaveMirror, Root.transform.position, Quaternion.Euler(0, 90, 90), Root.transform);
                            break;

                    }
                }

                if (isConcave && !isConcaveReflective && !isReflective)
                {

                    ObjectArrow.transform.position = new Vector3(Random.Range(OpticalElement.transform.position.x - 30, OpticalElement.transform.position.x - 5),
                        OpticalElement.transform.position.y + 4.84F, OpticalElement.transform.position.z);

                }
                else if (isReflective)
                {
                    //50% chance of it being in front the focal point or from Lens position - 30 to lens position - 22. This is because past 22, going through the focal point doesn't
                    //result in a collision with the optical element
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            ObjectArrow.transform.position = new Vector3(Random.Range(OpticalElement.transform.position.x - 30, OpticalElement.transform.position.x - 24),
    OpticalElement.transform.position.y + 4.84F, OpticalElement.transform.position.z);
                            break;
                        case 1:
                            ObjectArrow.transform.position = new Vector3(Random.Range(OpticalElement.transform.position.x - 11, OpticalElement.transform.position.x - 5),
    OpticalElement.transform.position.y + 4.84F, OpticalElement.transform.position.z);
                            break;
                    }
                }
                else
                {
                    ObjectArrow.transform.position = new Vector3(Random.Range(OpticalElement.transform.position.x - 30, OpticalElement.transform.position.x - 18.5F),
                    OpticalElement.transform.position.y + 2, OpticalElement.transform.position.z);
                }


                InitializeLineRenderers();
                isInteractable = true;

            }
        }

        private void InitializeLineRenderers()
        {
            CurrentRay = Instantiate(ProgrammableLR, ObjectArrow.transform.position, Quaternion.identity, Root.transform);
            if (isReflective)
            {
                if (isConcaveReflective)
                {
                    if (Mathf.Abs(ObjectArrow.transform.position.x - OpticalElement.transform.position.x) < FocalLength)
                    {
                        CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, (OpticalElement.transform.position - new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F)))), Root.transform);

                    }
                    else
                    {
                        CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, (FocalPointLeft - new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F)))), Root.transform);

                    }
                }
                else
                {
                    CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, (FocalPoint - new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F)))), Root.transform);
                }
            }
            else if (!isReflective)
            {
                CurrentALRRay = Instantiate(ProgrammableALR, ObjectArrow.transform.position, Quaternion.identity, Root.transform);
            }

            CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
            CurrentRay.GetComponent<SetLRPoints>().InitializeLR();

            if (isConcave || isConcaveReflective || isConvexReflective)
            {
                CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), 0);
                CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(0, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F));
            }

            else if (!isConcave && !isConcaveReflective && !isConvexReflective)
            {
                CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F), 0);
                CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(0, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F));
            }
            LRIndex = 1;

        }

        public void Interact()
        {
            if (isInteractable)
            {
                Vector3 point;

                if (true)
                {
                    if (Input.touchCount < 1)
                    {
                        if (isConcave || isConcaveReflective || isConvexReflective)
                        {
                            point = new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F);
                        }

                        else
                        {
                            point = new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F);
                        }


                    }

                    else
                    {
                        point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    }
                }
                else
                {
                    //This is for using the editor to test things
                    point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                }
                switch (counter)
                {
                    case 0:

                        //Start interactive ray tracing tutorial, beginning with parallel ray.
                        //This snippet of code ensures that this is only called once
                        if (!isLRInitializing)
                        {
                            isLRInitializing = true;
                            //If user has already touched the screen, display the message for a short time. Otherwise, display for 5 seconds
                            if (Input.touchCount > 0)
                            {
                                DisplayPrompt("Touch the screen to draw a ray", 0);

                            }
                            else
                            {
                                DisplayPrompt("Touch the screen to draw a ray", 3);

                            }
                            if (ImageArrow.GetComponent<Animator>() != null)
                            {
                                ImageArrow.GetComponent<Animator>().enabled = false;
                            }
                            ImageArrow.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
                            transform.GetChild(0).gameObject.SetActive(false);
                            if (ImageDistanceText != null)
                            {
                                ImageDistanceText.SetActive(false);
                            }
                            CurrentRayIndex = 0;
                        }
                        GenerateLR(point, CurrentRayIndex);
                        break;

                    case 1:
                        if (!isLRInitializing)
                        {
                            isLRInitializing = true;
                            InitializeLineRenderers();
                            CurrentRayIndex = 1;
                        }
                        GenerateLR(point, CurrentRayIndex);
                        break;

                    case 2:
                        if (!isLRInitializing)
                        {
                            isLRInitializing = true;
                            InitializeLineRenderers();
                            CurrentRayIndex = 2;
                        }
                        GenerateLR(point, CurrentRayIndex);
                        break;

                    case 3:
                        print("case 3");
                        ImageInfoPanelAnimator.SetBool("DisplayInfo", true);
                        if (isConcave && !isReflective)
                        {
                            //Reverse Directions of FocalPoint Ray and Parallel Ray automatically
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
                            CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, CalculateFinalConcavePosition().y), 1);
                            CurrentRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(-1000, CalculateFinalConcavePosition().y), 10);

                            ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                            ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                            transform.GetChild(0).gameObject.SetActive(true);
                        }
                        counter++;
                        break;

                    case 4:
                        //Sometimes the line renderers get set multiple times resulting in visual glitches. This destroys them so that only the animated line renderers are visible
                        GameObject[] ProgrammableLineRenderers = GameObject.FindGameObjectsWithTag("ProgrammableLR");
                        for (int i = 0; i < ProgrammableLineRenderers.Length; i++)
                        {
                            //Destroy(ProgrammableLineRenderers[i]);
                        }
                        isInteractable = false;
                        break;

                }
                //limits the number of times the line renderer can have its position set. If this number is too high, this if statement is too high, the next few lines of code try to improperly
                //position the line renderer, so then we have stray rays of light everywhere
                int LineRendLimit;

                if (isReflective)
                {
                    LineRendLimit = 2;
                }
                else
                {
                    LineRendLimit = 3;
                }
                if (counter < LineRendLimit)
                {
                    if (CurrentRay != null)
                    {
                        CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, point);
                    }
                }

            }

        }

        private void GenerateLR(Vector3 point, int CurrentRayIndex)
        {
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

                                if (isConcave || isReflective)
                                {

                                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F), 0.5F);
                                    CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F));


                                }
                                else if (!isConcave)
                                {
                                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F), 0.5F);
                                    CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F));
                                }

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
                                if (isConcave)
                                {
                                    if (isReflective)
                                    {
                                        if (isConcaveReflective)
                                        {
                                            CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(-LRScalingFactor * (new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F) - FocalPointLeft), LRScalingFactor / 10);
                                            CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, -LRScalingFactor * (FocalPoint - new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F)));
                                        }
                                        else
                                        {
                                            CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * ((new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F)) - FocalPoint), LRScalingFactor / 10);
                                            CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, LRScalingFactor * ((new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F)) - FocalPoint));
                                        }
                                    }

                                    else
                                    {
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F) - FocalPointLeft), LRScalingFactor / 10);
                                        CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, LRScalingFactor * (FocalPoint - new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F)));
                                    }
                                }
                                else
                                {
                                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (FocalPoint - new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F)), LRScalingFactor / 10);
                                    CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, LRScalingFactor * (FocalPoint - new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F)));
                                }
                                StartCoroutine(ExecuteAfterTime(0.5F));
                                counter++;
                                isLRInitializing = false;
                            }

                            break;
                    }
                    break;
                //Optical Center Ray
                case 1:
                    StartCoroutine(ExecuteAfterTime(0.5F));
                    switch (LRIndex)
                    {
                        case 1:
                            if (CheckPointLocation(point, 0))
                            {
                                CurrentRay.GetComponent<SetLRPoints>().SetNumLRPoints(3);

                                CurrentRay.GetComponent<SetLRPoints>().SetVisible(false);
                                if (!isReflective)
                                {
                                    if (isConcave)
                                    {
                                        CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, LRScalingFactor * (OpticalElement.transform.position - new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F)));
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (OpticalElement.transform.position - new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F)), LRScalingFactor / 10);
                                    }
                                    else
                                    {
                                        CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, LRScalingFactor * (OpticalElement.transform.position - new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F)));
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (OpticalElement.transform.position - new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F)), LRScalingFactor / 10);
                                    }
                                }
                                else
                                {
                                    CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, CurrentALRRay.GetComponent<SetPoints>().GetHitPoint());
                                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.GetComponent<SetPoints>().GetHitPoint(), 0.5F);
                                }
                                LRIndex++;
                                StartCoroutine(ExecuteAfterTime(0.5F));

                            }
                            break;

                        case 2:
                            if (isReflective)
                            {
                                HitOne = new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F);
                                if (CheckPointLocation(point, 1) || (Mathf.Abs(ObjectArrow.transform.position.x - OpticalElement.transform.position.x) < FocalLength && !isConvexReflective))
                                {
                                    CurrentRay.GetComponent<SetLRPoints>().SetVisible(false);
                                    HitTwo = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();

                                    if (Mathf.Abs(ObjectArrow.transform.position.x - OpticalElement.transform.position.x) < FocalLength && !isConvexReflective)
                                    {
                                        CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection());
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection(), LRScalingFactor / 100);
                                        Vector3 ReflectedDirection = CurrentALRRay.GetComponent<SetPoints>().GetReflectedDirection();
                                        //Reverse parallel ray
                                        CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                                        CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                                        CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                                        CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(HitOne, 0);
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * (HitOne - FocalPointLeft), LRScalingFactor / 10);
                                        CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                                        CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);

                                        //Reverse FocalPoint Ray
                                        CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.identity);
                                        CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
                                        CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartWidth = 0.1F;
                                        CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndWidth = 0.1F;
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(HitTwo.x - 0.25F, HitTwo.y), 0);
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(LRScalingFactor * -new Vector3(ReflectedDirection.x, ReflectedDirection.y + 10), LRScalingFactor / 1000);
                                        CurrentALRRay.GetComponent<AnimatedLineRenderer>().StartColor = new Color(0, 255, 0);
                                        CurrentALRRay.GetComponent<AnimatedLineRenderer>().EndColor = new Color(0, 255, 0);
                                    }
                                    else
                                    {
                                        CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, new Vector3(-1000, CurrentALRRay.GetComponent<SetPoints>().GetHitPoint().y));
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(-1000, CurrentALRRay.GetComponent<SetPoints>().GetHitPoint().y), 10);
                                    }
                                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                                    ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();

                                    if (isConvexReflective)
                                    {
                                        //Reverse Ray Direction if convex mirror
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
                                    }
                                    counter = 3;
                                    isLRInitializing = false;
                                }

                            }
                            else
                            {
                                counter++;
                                isLRInitializing = false;
                            }
                            break;
                    }
                    break;

                //Focal Point Ray
                case 2:
                    StartCoroutine(ExecuteAfterTime(0.5F));
                    if (!isReflective)
                    {
                        switch (LRIndex)
                        {
                            case 1:
                                if (CheckPointLocation(point, 0))
                                {
                                    CurrentRay.GetComponent<SetLRPoints>().SetVisible(false);
                                    CurrentRay.GetComponent<SetLRPoints>().SetNumLRPoints(3);
                                    if (isConcave)
                                    {
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CalculateFinalConcavePosition(), 0.5F);
                                        CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, CalculateFinalConcavePosition());
                                    }
                                    else
                                    {
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(CalculateFinalPosition(), 0.5F);
                                        CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, CalculateFinalPosition());
                                    }

                                    StartCoroutine(ExecuteAfterTime(0.5F));
                                    LRIndex++;
                                }
                                break;

                            case 2:
                                if (CheckPointLocation(point, 1))
                                {
                                    CurrentRay.GetComponent<SetLRPoints>().SetVisible(false);
                                    if (isConcave)
                                    {
                                        CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, new Vector3(1000, CalculateFinalConcavePosition().y, CalculateFinalConcavePosition().z));
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(1000, CalculateFinalConcavePosition().y, CalculateFinalConcavePosition().z), 25);
                                    }
                                    else
                                    {
                                        CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, new Vector3(1000, CalculateFinalPosition().y, CalculateFinalPosition().z));
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(1000, CalculateFinalPosition().y, CalculateFinalPosition().z), 25);
                                    }
                                    counter++;
                                    isLRInitializing = false;
                                    //This is the last ray to be drawn. Once this is drawn correctly (the function ensures it), the image will be generated.
                                    if (!isConcave)
                                    {
                                        transform.GetChild(0).gameObject.SetActive(true);
                                        ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                                        ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                                        print("position set");
                                    }

                                }
                                break;
                        }
                    }
                    else
                    {
                    }
                    break;
            }

        }
        bool CheckPointLocation(Vector3 point, int index)
        {
            Vector3[] CorrectPosition = CalculatePositions();
            bool XCorrect, YCorrect;
            //If the values are within ErrorTolerance % of each other, it is considered correct. This is important since, on a phone screen, space is not optimal, so you need some allowances.
            //Note that if the F is not in front of the 1.00, Unity will cast it to an int.

            //Check point and trajectory. If either of them are correct (provided that we are past a certain point on the x axis), then return true for XCorrect and YCorrect
            //We know that trajectory only matters with the second round of line renderers (what happens to the ray after it interacts with the optical element),
            //It will still check the point for the first round of line renderers (Since they should all be drawn to hit the optical element)

            if (Mathf.Abs(point.x / CorrectPosition[index].x) < 1.00F + ErrorTolerance && Mathf.Abs(point.x / CorrectPosition[index].x) > 1.00F - ErrorTolerance || TrajectoryCheck(point, CorrectPosition))
            {
                XCorrect = true;
            }
            else
            {
                XCorrect = false;
            }
            if (Mathf.Abs(point.y / CorrectPosition[index].y) < 1.00F + ErrorTolerance && Mathf.Abs(point.y / CorrectPosition[index].y) > 1.00F - ErrorTolerance || TrajectoryCheck(point, CorrectPosition))
            {
                YCorrect = true;
            }
            else
            {
                YCorrect = false;
            }

            if (XCorrect && YCorrect)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        IEnumerator ExecuteAfterTime(float time)
        {
            if (isCoroutineExecuting)
                yield break;

            isCoroutineExecuting = true;

            yield return new WaitForSeconds(time);

            for (int i = 0; i < 3; i++)
            {
                CurrentRays = GameObject.FindGameObjectsWithTag("ProgrammableLR");
                CurrentRay = CurrentRays[CurrentRays.Length - 1];
            }

            CurrentRay.GetComponent<SetLRPoints>().SetVisible(true);

            isCoroutineExecuting = false;
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
        Vector3[] CalculatePositions()
        {
            Vector3[] points = new Vector3[2];

            switch (CurrentRayIndex)
            {
                //Parallel Ray
                case 0:
                    if (!isReflective)
                    {
                        if (isConcave)
                        {
                            points[0] = new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F);
                            points[1] = new Vector3(OpticalElement.transform.position.x + 12, ObjectArrow.transform.position.y + 3.3F + FocalLength * Mathf.Tan(Mathf.Deg2Rad * AngleBetween(Vector3.right, new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F) - FocalPointLeft)));
                        }
                        else
                        {
                            points[0] = new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F);
                            points[1] = new Vector3(OpticalElement.transform.position.x + 12, OpticalElement.transform.position.y);
                        }
                    }

                    else
                    {
                        if (isConcaveReflective)
                        {
                            if (OpticalElement != null)
                            {
                                points[0] = new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F);
                                points[1] = FocalPointLeft;
                            }
                        }
                        else
                        {
                            if (OpticalElement != null)
                            {
                                points[0] = new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F);
                                points[1] = CalculateFinalConvexPosition();
                            }
                        }
                    }
                    break;

                //Optical Center Ray or Focal Point Ray for Mirrors
                case 1:
                    if (isReflective)
                    {
                        CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();

                        if (isConcaveReflective)
                        {
                            if (Mathf.Abs(ObjectArrow.transform.position.x - OpticalElement.transform.position.x) < FocalLength)
                            {
                                points[0] = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();
                                points[1] = points[0];
                            }
                            else
                            {
                                points[0] = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();
                                points[1] = new Vector3(FocalPointLeft.x, CurrentALRRay.GetComponent<SetPoints>().GetHitPoint().y);
                            }
                        }
                        else
                        {
                            points[0] = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();
                            points[1] = new Vector3(FocalPointLeft.x, CurrentALRRay.GetComponent<SetPoints>().GetHitPoint().y);
                        }
                    }

                    else
                    {
                        if (isConcave)
                        {
                            points[0] = OpticalElement.transform.position;
                            points[1] = 10 * (new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F) - OpticalElement.transform.position);
                        }
                        else
                        {
                            points[0] = OpticalElement.transform.position;
                            points[1] = points[0];
                        }

                    }
                    break;

                //Focal Point Ray (Lenses only)
                case 2:
                    if (isConcave)
                    {
                        points[0] = CalculateFinalConcavePosition();
                        points[1] = new Vector3(FocalPoint.x, CalculateFinalConcavePosition().y, CalculateFinalPosition().z);
                    }
                    else
                    {
                        points[0] = CalculateFinalPosition();
                        points[1] = new Vector3(FocalPoint.x, CalculateFinalPosition().y, CalculateFinalPosition().z);
                    }



                    break;



            }
            return points;
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

        private Vector3 CalculateFinalConcavePosition()
        {
            float FinalX = OpticalElement.transform.position.x;
            float FinalY;
            if (CurrentRay != null)
            {
                FinalY = ObjectArrow.transform.position.y + 3.3F + Mathf.Abs(OpticalElement.transform.position.x - CurrentRay.transform.position.x) /
                        Mathf.Tan(Mathf.Deg2Rad * AngleBetween((FocalPoint - new Vector3(ObjectArrow.transform.position.x, (ObjectArrow.transform.position.y + 3.3F))), Vector3.down));
            }
            else
            {
                FinalY = 0;
            }
            return new Vector3(FinalX, FinalY);

        }



        private float AngleBetween(Vector3 From, Vector3 To)
        {
            float sign = Mathf.Sign(Vector3.Cross(From, To).z);
            float angle = Vector3.Angle(From, To);

            return angle * sign;
        }

        public void DestroyAllLineRenderers()
        {
            GameObject[] LineRenderers = GameObject.FindGameObjectsWithTag("LightEmitter");
            GameObject[] AnimatedLineRenderers = GameObject.FindGameObjectsWithTag("AnimatedLightEmitter");
            GameObject[] PALRS = GameObject.FindGameObjectsWithTag("ProgrammableALR");
            GameObject[] PLRS = GameObject.FindGameObjectsWithTag("ProgrammableLR");
            GameObject[] RealRays = GameObject.FindGameObjectsWithTag("RealRay");
            GameObject[] VirtualRays = GameObject.FindGameObjectsWithTag("VirtualRay");

            for (int i = 0; i < LineRenderers.Length; i++)
            {
                Destroy(LineRenderers[i]);
            }

            for (int i = 0; i < AnimatedLineRenderers.Length; i++)
            {
                Destroy(AnimatedLineRenderers[i]);
            }
            for (int i = 0; i < PALRS.Length; i++)
            {
                Destroy(PALRS[i]);
            }
            for (int i = 0; i < PLRS.Length; i++)
            {
                Destroy(PLRS[i]);
            }
            for (int i = 0; i < RealRays.Length; i++)
            {
                Destroy(RealRays[i]);
            }
            for (int i = 0; i < VirtualRays.Length; i++)
            {
                Destroy(VirtualRays[i]);
            }
        }

        public void SetPlayScene(bool isOn)
        {
            PlaySceneToggleState = isOn;
        }

        public void SetMirror(int Mirror)
        {
            //0 for convex mirror, 1 for concave mirror

            MirrorSelection = Mirror;
            isMirrorSet = true;
        }

        public Vector3 CalculateFinalConvexPosition()
        {
            float x, y;
            x = FocalPointLeft.x;
            y = Mathf.Abs(GameObject.Find("F1").transform.position.x - GameObject.Find("F2").transform.position.x) / Mathf.Tan(Mathf.Deg2Rad * AngleBetween(Vector3.down, new Vector3(ObjectArrow.transform.position.y + 3.3F, OpticalElement.transform.position.x) - FocalPoint));
            return new Vector3(x, -y);

        }

        public bool TrajectoryCheck(Vector3 point, Vector3[] CorrectPosition)
        {
            bool XCorrect = false;
            bool YCorrect = false;
            Vector2 UserTrajectory = new Vector2(point.x, point.y) - new Vector2(CorrectPosition[0].x, CorrectPosition[0].y);
            Vector2 CalculatedTrajectory = new Vector2(CorrectPosition[1].x, CorrectPosition[1].y) - new Vector2(CorrectPosition[0].x, CorrectPosition[0].y);


            //Don't forget to normalize the vectors and make sure that the length of the user line renderer is long enough that they don't accidentally get the correct answer just by tapping in the right spot near the lens/mirror
            if ((Vector2.Dot(UserTrajectory.normalized, CalculatedTrajectory.normalized) > 1 - TrajectoryErrorTolerance && Vector2.Dot(UserTrajectory.normalized, CalculatedTrajectory.normalized) < 1 + TrajectoryErrorTolerance))
            {
                if (UserTrajectory.magnitude > 20)
                {
                    XCorrect = true;
                    YCorrect = true;
                }
                else
                {
                    XCorrect = false;
                    YCorrect = false;
                }
            }
            else
            {
                XCorrect = false;
                YCorrect = false;
            }


            if (XCorrect == true && YCorrect == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DisplayPrompt(string text, float duration)
        {
            PopUpPromptAnimator.SetBool("DisplayPrompt", true);
            PopUpPromptTextAnimator.SetBool("DisplayPrompt", true);
            PopUpPromptText.text = text;
            StartCoroutine(DestroyMessagePrompt(duration));

        }
        public int GetCounter()
        {
            return counter;
        }

        public int GetLRIndex()
        {
            return LRIndex;
        }

        public int GetCurrentRayIndex()
        {
            return CurrentRayIndex;
        }
    }

}