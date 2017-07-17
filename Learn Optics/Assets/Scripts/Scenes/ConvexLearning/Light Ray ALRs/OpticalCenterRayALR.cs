using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]

    public class OpticalCenterRayALR : MonoBehaviour
    {
        private AnimatedLineRenderer animatedLineRenderer;
        private float FocalLength;
        private float ObjectDistance;
        private float ImageDistance;
        private RaycastHit hit;
        private float Magnification;
        private GameObject OpticalElement;
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

        public void onClick()
        {
            animatedLineRenderer.Reset();

            animatedLineRenderer.Enqueue(transform.position);
            animatedLineRenderer.Enqueue(OpticalElement.transform.position, 0.5F);
            animatedLineRenderer.Enqueue(LRScalingFactor * (OpticalElement.transform.position - transform.position), 1000);



        }

        private void DebugLines()
        {
            Debug.DrawRay(transform.position, 1000 * (OpticalElement.transform.position - transform.position));
        }
    }
}