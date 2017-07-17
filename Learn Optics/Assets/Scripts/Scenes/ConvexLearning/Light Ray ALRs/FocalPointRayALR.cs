using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class FocalPointRayALR : MonoBehaviour
    {
        private AnimatedLineRenderer animatedLineRenderer;
        private float FocalLength;
        private float ObjectDistance;
        private float ImageDistance;
        private float Magnification;
        private RaycastHit hit;
        private GameObject OpticalElement;
        private Vector3 FocalPointLeft;
        public bool VirtualImage;
        private float LRScalingFactor;

        private void Start()
        {
            animatedLineRenderer = GetComponent<AnimatedLineRenderer>();
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            FocalLength = 12;
            //This just extends the length of the linerenderer. LineRenderer draws a line from point a to point b. The functions used here only get the trajectory of the light beams.
            //We need to extend the length of it or it won't be visible.
            LRScalingFactor = 10000000;
        }
        void Update()
        {
            transform.eulerAngles = new Vector3(0, 0, AngleBetween(Vector3.right, FocalPointLeft - transform.position));

        }

        public void onClick()
        {
            animatedLineRenderer.Reset();
            FocalPointLeft = new Vector3(OpticalElement.transform.position.x - FocalLength, OpticalElement.transform.position.y, OpticalElement.transform.position.z);

                animatedLineRenderer.Enqueue(transform.position);
                animatedLineRenderer.Enqueue(CalculateFinalPosition(), 0.5F);

                animatedLineRenderer.Enqueue(new Vector3 (1000, CalculateFinalPosition().y, CalculateFinalPosition().z), 25);

            if (VirtualImage)
            {

            }

        }

        private void DebugLines()
        {
            Debug.DrawLine(transform.position, CalculateFinalPosition());
            Debug.DrawRay(CalculateFinalPosition(), 1000 * Vector3.right);
            
        }

        private float AngleBetween (Vector3 From, Vector3 To)
        {
            float sign = Mathf.Sign(Vector3.Cross(From, To).z);
            float angle = Vector3.Angle(From, To);

            return angle * sign;
        }

        //This is calculated using the World Space
        private Vector3 CalculateFinalPosition()
        {
            float FinalX = OpticalElement.transform.position.x;
            float FinalY = OpticalElement.transform.position.y + Mathf.Abs(OpticalElement.transform.position.x - transform.position.x) * Mathf.Tan(Mathf.Deg2Rad * AngleBetween(Vector3.right, FocalPointLeft - transform.position)) + Mathf.Abs(transform.position.y - OpticalElement.transform.position.y);
            float FinalZ = OpticalElement.transform.position.z;

            return new Vector3(FinalX, FinalY, FinalZ);
        }
    }
}