using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class SetPoints : MonoBehaviour
    {
        GameObject Root;
        AnimatedLineRenderer RayRenderer;
        LineRenderer LineRenderer;
        public RaycastHit OpticalElementHit;
        public bool isReflective;
        public bool isConcaveReflective;
        public bool isConvexReflective;
        private float LRScalingFactor;
        public Vector3 HitPoint;
        Vector3 ReflectedRayDirection;

        private void Start()
        {
            //This just extends the length of the linerenderer. LineRenderer draws a line from point a to point b. The functions used here only get the trajectory of the light beams.
            //We need to extend the length of it or it won't be visible.
        }
        private void Update()
        {

            isReflective = GameObject.FindGameObjectWithTag("OpticalElement").GetComponent<Properties_Optical>().isReflective;
            isConcaveReflective = GameObject.FindGameObjectWithTag("OpticalElement").GetComponent<Properties_Optical>().isConcaveReflective;
            isConvexReflective = GameObject.FindGameObjectWithTag("OpticalElement").GetComponent<Properties_Optical>().isConvexReflective;

        }
        public void InitializeALR()
        {
            //Initialize the AnimatedLightRenderer here. When this script is called, it probably doesn't call Start(), so this function thinks that RayRenderer isn't initialized
            Root = GameObject.Find("Root");
            RayRenderer = GetComponent<AnimatedLineRenderer>();
            LRScalingFactor = 10000000;

        }

        public void SetLinePoint(Vector3 Point, float duration)
        {
            RayRenderer.Enqueue(Point, duration);

        }


        public void CalculateReflectedDirection()
        {
            if (Physics.Raycast(transform.position, transform.right, out OpticalElementHit))
            {
                if (!isConcaveReflective && !isConvexReflective)
                {
                    HitPoint = new Vector3(Root.transform.position.x, OpticalElementHit.point.y + (Mathf.Abs(Root.transform.position.x - OpticalElementHit.point.x) * Mathf.Tan(Mathf.Deg2Rad * AngleBetween(Vector3.right, OpticalElementHit.point - transform.position
                        ))));
                }
                else
                {
                    HitPoint = OpticalElementHit.point;
                }
                ReflectedRayDirection = new Vector3(0, 0, 0);
                Vector3 traj_in = OpticalElementHit.point - transform.position; //Vector from light origin to point hit
                float angle_incident = AngleBetween(traj_in, -OpticalElementHit.normal);
                float angle_reflected = angle_incident;
                ReflectedRayDirection = LRScalingFactor * new Vector3(-Mathf.Cos(Mathf.Deg2Rad * angle_reflected), -Mathf.Sin(Mathf.Deg2Rad * angle_reflected));
            }             
        }

        public Vector3 GetReflectedDirection()
        {
            return ReflectedRayDirection;
        }
        
        public Vector3 GetHitPoint()
        {
            return HitPoint;
        }

        //Enqueues a point in the ALR at the hit point.
        public void SetPointAtHit(float duration)
        {
            if (HitPoint != new Vector3(0, 0, 0))
            {
                SetLinePoint(HitPoint, duration);
            }
            else
            {
                SetLinePoint(new Vector3(1000, transform.position.y), duration);
            }
        }
        private float AngleBetween(Vector3 From, Vector3 To)
        {
            float sign = Mathf.Sign(Vector3.Cross(From, To).z);
            float angle = Vector3.Angle(From, To);

            return angle * sign;
        }
    }
}