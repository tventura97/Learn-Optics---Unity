using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class GenerateQuiz : MonoBehaviour
    {
        GameObject ObjectArrow;
        GameObject OpticalElement;
        GameObject ImageArrow;
        GameObject ImageDistanceText;
        GameObject CurrentRay;
        GameObject CurrentALRRay;
        GameObject Root;
        public GameObject ProgrammableLR;
        public GameObject ProgrammableALR;
        public Vector3 FocalPoint;
        public Vector3 FocalPointLeft;
        public float FocalLength;
        public bool isInteractable;
        public int counter;
        public int CurrentRayIndex;
        public int LRIndex;
        private float ErrorTolerance;
        private bool isCoroutineExecuting;
        private bool isLRInitializing;
        



        void Start()
        {
            InitializeObjects();
            //We select the focal length to be 12 because it is a decent distance from the lens that allows for a wide range of image formation without having to increase
            //Orthographic size of the camera by too much (Image/Object Arrows would become too small to interact with if the camera gets too large)
            FocalLength = 12;
            FocalPoint = new Vector3 (OpticalElement.transform.position.x + FocalLength, OpticalElement.transform.position.y);
            FocalPointLeft = new Vector3(OpticalElement.transform.position.x - FocalLength, OpticalElement.transform.position.y);
            //How close the user can be to the correction position of the endpoint of the Light ray. Default is 5%
            ErrorTolerance = 0.015F;
            counter = 0;
            isLRInitializing = false;
        }

        private void InitializeObjects()
        {
            ObjectArrow = GameObject.Find("ObjectArrow");
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            ImageArrow = GameObject.Find("ImageArrow");
            ImageDistanceText = GameObject.Find("ImageDistanceText");
            Root = GameObject.Find("Root");
        }
        private void Update()
        {
            if (isInteractable)
            {
                Interact();
            }
        }

        public void OnClick()
        {
            ObjectArrow.SetActive(true);
            ImageArrow.GetComponent<Animator>().enabled = false;
            ImageArrow.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            ObjectArrow.transform.GetComponent<ObjectArrowControls>().ResetALRs();
            ObjectArrow.transform.position = new Vector3(Random.Range(OpticalElement.transform.position.x - 30, OpticalElement.transform.position.x - 18.5F),
            OpticalElement.transform.position.y + 2, OpticalElement.transform.position.z);
            counter = 0;
            isInteractable = true;
        }

        public void Interact()
        {
            if (isInteractable)
            {
                Vector3 point;
                if (Input.touchCount < 1)
                {
                    point = new Vector3(ObjectArrow.transform.position.x, ObjectArrow.transform.position.y + 1.32F);
                }
                else
                {
                    point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                switch (counter)
                {

                    case 0:
                        if (!isLRInitializing)
                        {
                            isLRInitializing = true;
                            //Start interactive ray tracing tutorial
                            ImageArrow.GetComponent<Animator>().enabled = false;
                            ImageArrow.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
                            transform.GetChild(0).gameObject.SetActive(false);
                            ImageDistanceText.SetActive(false);
                            InitializeLineRenderers();
                            CurrentRayIndex = 0;
                            GenerateLR(point, CurrentRayIndex);
                        }
                        break;

                    case 1:
                        InitializeLineRenderers();
                        CurrentRayIndex = 1;
                        GenerateLR(point, CurrentRayIndex);
                        break;

                    case 2:
                        InitializeLineRenderers();
                        CurrentRayIndex = 2;
                        GenerateLR(point, CurrentRayIndex);
                        break;

                    case 3:
                        //Image forms
                        transform.GetChild(0).gameObject.SetActive(true);
                        ImageArrow.GetComponent<ImageArrowGeneration>().SetPosition();
                        GenerateLR(point, CurrentRayIndex);
                        break;

                }
                CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, point);
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
                                CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, 1000 * (FocalPoint - new Vector3(OpticalElement.transform.position.x, ObjectArrow.transform.position.y + 1.32F))); StartCoroutine(ExecuteAfterTime(0.5F));
                                isInteractable = false;
                                counter++;
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
                        isInteractable = false;
                        StartCoroutine(ExecuteAfterTime(0.5F));
                        counter++;
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
                                CurrentRay.GetComponent<SetLRPoints>().SetLineRendPoints(LRIndex, new Vector3(1000, CalculateFinalPosition().y, CalculateFinalPosition().z));
                                CurrentALRRay.GetComponent<SetPoints>().SetLinePoint(new Vector3(1000, CalculateFinalPosition().y, CalculateFinalPosition().z), 25);
                                isInteractable = false;
                                StartCoroutine(ExecuteAfterTime(0.5F));
                                counter++;
                            }
                            break;
                    }
                    break;
            }
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
        IEnumerator ExecuteAfterTime(float time)
        {
            if (isCoroutineExecuting)
                yield break;

            isCoroutineExecuting = true;

            yield return new WaitForSeconds(time);

            CurrentRay.GetComponent<SetLRPoints>().SetVisible(true);

            isCoroutineExecuting = false;
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

        //This is calculated using the World Space
        private Vector3 CalculateFinalPosition()
        {
            float FinalX = OpticalElement.transform.position.x;
            float FinalY = CurrentRay.transform.position.y + 1.32F + Mathf.Abs(OpticalElement.transform.position.x - CurrentRay.transform.position.x)
                * Mathf.Tan(Mathf.Deg2Rad * AngleBetween(Vector3.right, FocalPointLeft - new Vector3(CurrentRay.transform.position.x, CurrentRay.transform.position.y + 1.32F)));
            float FinalZ = OpticalElement.transform.position.z;

            return new Vector3(FinalX, FinalY, FinalZ);
        }

        private float AngleBetween(Vector3 From, Vector3 To)
        {
            float sign = Mathf.Sign(Vector3.Cross(From, To).z);
            float angle = Vector3.Angle(From, To);

            return angle * sign;
        }
    }

}
