using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class VOPticalCenterRayALR : MonoBehaviour
    {
        private AnimatedLineRenderer animatedLineRenderer;
        private GameObject OpticalElement;
        private GameObject ObjectArrow;
        private bool VirtualImage;
        private float FocalLength;
        private float LRScalingFactor;

        void Start()
        {
            animatedLineRenderer = GetComponent<AnimatedLineRenderer>();
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            ObjectArrow = GameObject.Find("ObjectArrow");
            FocalLength = 12;
            VirtualImage = false;
            //This just extends the length of the linerenderer. LineRenderer draws a line from point a to point b. The functions used here only get the trajectory of the light beams.
            //We need to extend the length of it or it won't be visible.
            LRScalingFactor = 10000000;
        }


        void Update()
        {
            if (Mathf.Abs(ObjectArrow.transform.position.x - OpticalElement.transform.position.x) < FocalLength)
            {
                VirtualImage = true;
            }
            else
            {
                VirtualImage = false;
            }
        }
        public void onClick()
        {
            if (VirtualImage)
            {
                animatedLineRenderer.Reset();
                animatedLineRenderer.Enqueue(transform.position);
                animatedLineRenderer.Enqueue(1000 * -(OpticalElement.transform.position - transform.position), 500);
            }

        }
    }
}