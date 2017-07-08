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
        private GameObject Object;
        private bool VirtualImage;
        private float FocalLength;

        void Start()
        {
            animatedLineRenderer = GetComponent<AnimatedLineRenderer>();
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            Object = GameObject.Find("ObjectArrow");
            FocalLength = 12;
            VirtualImage = false;
        }


        void Update()
        {
            if (Mathf.Abs(Object.transform.position.x - OpticalElement.transform.position.x) < FocalLength)
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