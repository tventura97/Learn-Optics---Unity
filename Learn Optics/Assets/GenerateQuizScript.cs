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
        private GameObject ObjectArrow;
        private GameObject OpticalElement;
        private GameObject ImageArrow;
        private GameObject ImageDistanceText;
        private GameObject CurrentRay;
        private GameObject CurrentALRRay;
        private GameObject Root;
        private Toggle PlaySceneToggle;
        private Animator EquationPanelAnimator;
        private float ErrorTolerance;
        private float LRScalingFactor;
        private bool isCoroutineExecuting;
        private bool isLRInitializing;
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
            //This just extends the length of the linerenderer. LineRenderer draws a line from point a to point b. The functions used here only get the trajectory of the light beams.
            //We need to extend the length of it or it won't be visible.
            LRScalingFactor = 10000000;
            counter = 0;
            isInteractable = false;
            isLRInitializing = false;
            PlaySceneToggleState = PlaySceneToggle.isOn;
            debugcounter = 0;

        }

        private void Update()
        {
            InitializeObjects();
            FocalLength = Mathf.Abs(GameObject.Find("F1").transform.position.x - OpticalElement.transform.position.x);
            FocalPoint = new Vector3(OpticalElement.transform.position.x + FocalLength, OpticalElement.transform.position.y);
            FocalPointLeft = new Vector3(OpticalElement.transform.position.x - FocalLength, OpticalElement.transform.position.y);
        }
        private void InitializeObjects()
        {
            ObjectArrow = GameObject.Find("ObjectArrow");
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            ImageArrow = GameObject.Find("ImageArrow");
            ImageDistanceText = GameObject.Find("ImageDistanceText");
            Root = GameObject.Find("Root");
            PlaySceneToggle = GameObject.Find("PlaySceneToggle").GetComponent<Toggle>();
            EquationPanelAnimator = GameObject.Find("EquationPanel").GetComponent<Animator>();
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
            //If the scene is not playing, then do everything else. Otherwise, no functionality.
            if (!PlaySceneToggleState)
            {
                //This will destroy any and all forms of line renderers currently present in the scene.

                //Learning Scripts can bypass the PlaySceneToggleRequirement.

                DestroyAllLineRenderers();
                ObjectArrow.SetActive(true);
                ImageArrow.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
                ObjectArrow.transform.GetComponent<ObjectArrowControls>().ResetALRs();
                if (isConcave)
                {
                    if (isReflective)
                    {
                        ObjectArrow.transform.position = new Vector3(Random.Range(OpticalElement.transform.position.x - 30, OpticalElement.transform.position.x - 25),
                        OpticalElement.transform.position.y + 4.84F, OpticalElement.transform.position.z);
                    }
                    else
                    {
                        ObjectArrow.transform.position = new Vector3(Random.Range(OpticalElement.transform.position.x - 30, OpticalElement.transform.position.x - 5),
                        OpticalElement.transform.position.y + 4.84F, OpticalElement.transform.position.z);
                    }
                }
                else
                {
                    ObjectArrow.transform.position = new Vector3(Random.Range(OpticalElement.transform.position.x - 30, OpticalElement.transform.position.x - 18.5F),
                    OpticalElement.transform.position.y + 2, OpticalElement.transform.position.z);
                }

                counter = 0;
                LRIndex = 1;
                InitializeLineRenderers();
                isInteractable = true;

            }
        }

        private void InitializeLineRenderers()
        {
            CurrentRay = Instantiate(ProgrammableLR, ObjectArrow.transform.position, Quaternion.identity, Root.transform);
            if (isReflective)
            {
                CurrentALRRay = Instantiate(ProgrammableALR, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), Quaternion.Euler(0, 0, AngleBetween(Vector3.right, (FocalPointLeft - new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F)))), Root.transform);
            }
            else if (!isReflective)
            {
                CurrentALRRay = Instantiate(ProgrammableALR, ObjectArrow.transform.position, Quaternion.identity, Root.transform);
            }
            CurrentALRRay.GetComponent<SetPoints>().InitializeALR();
            CurrentRay.GetComponent<SetLRPoints>().InitializeLR();

            if (isConcave)
            {
                CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F), 0);
                CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(0, new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F));
            }

            else
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
                point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                /*
                if (Input.touchCount < 1)
                {
                    point = new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 3.3F);
                }
                else
                {
                    point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
                */
                switch (counter)
                {
                    case 0:
                        //Start interactive ray tracing tutorial, beginning with parallel ray.

                        //This snippet of code ensures that this is only called once
                        if (!isLRInitializing)
                        {
                            isLRInitializing = true;
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

                            isInteractable = false;
                            transform.GetChild(0).gameObject.SetActive(true);
                            ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                            ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                        }
                        break;

                }
                if (counter < 4)
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
                                        CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(-LRScalingFactor * (new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F) - FocalPointLeft), LRScalingFactor / 10);
                                        CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, -LRScalingFactor * (FocalPoint - new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F)));
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
                                counter++;
                                LRIndex++;
                                StartCoroutine(ExecuteAfterTime(0.5F));
                             
                            }
                            break;

                        case 2:
                            if (isReflective)
                            {
                                if (CheckPointLocation(point, 1))
                                {
                                    CurrentRay.GetComponent<SetLRPoints>().SetVisible(false);
                                    CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, new Vector3 (-1000, CurrentALRRay.GetComponent<SetPoints>().GetHitPoint().y));
                                    CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(-1000, CurrentALRRay.GetComponent<SetPoints>().GetHitPoint().y), 100);
                                    ImageArrow.GetComponent<ImageArrowGeneration>().CalculatePosition();
                                    ImageArrow.transform.position = ImageArrow.GetComponent<ImageArrowGeneration>().GetPosition();
                                    isInteractable = false;
                                    isLRInitializing = false;
                                    StartCoroutine(ExecuteAfterTime(0.5F));
                                    counter++;
                                }

                            }
                            break;
                    }
                    break;

                //Focal Point Ray
                case 2:
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
                                    ImageArrow.transform.position = ImageArrow.GetComponent<ImageArrowGeneration>().GetPosition();
                                    isInteractable = false;
                                }


                            }
                            break;
                    }
                    break;
            }

        }
        bool CheckPointLocation(Vector3 point, int index)
        {
            print("Checking Point Location " + debugcounter);
            debugcounter++;

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
        IEnumerator ExecuteAfterTime(float time)
        {
            if (isCoroutineExecuting)
                yield break;

            isCoroutineExecuting = true;

            yield return new WaitForSeconds(time);

            if (CurrentRay != null)
            {
                CurrentRay.GetComponent<SetLRPoints>().SetVisible(true);
            }
            isCoroutineExecuting = false;
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
                        points[0] = new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 3.3F);
                        points[1] = FocalPointLeft;
                    }
                    break;

                //Optical Center Ray
                case 1:
                    if (isReflective)
                    {
                        CurrentALRRay.GetComponent<SetPoints>().CalculateReflectedDirection();
                        points[0] = CurrentALRRay.GetComponent<SetPoints>().GetHitPoint();
                        points[1] = new Vector3(FocalPointLeft.x, CurrentALRRay.GetComponent<SetPoints>().GetHitPoint().y);
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

                //Focal Point Ray
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

    }

}