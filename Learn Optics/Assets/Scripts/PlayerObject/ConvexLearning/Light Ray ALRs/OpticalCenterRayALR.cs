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

        private void Start()
        {
            animatedLineRenderer = GetComponent<AnimatedLineRenderer>();
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            FocalLength = 12;
        }

        public void onClick()
        {
            animatedLineRenderer.Reset();

            animatedLineRenderer.Enqueue(transform.position);
            animatedLineRenderer.Enqueue(OpticalElement.transform.position, 0.5F);
            animatedLineRenderer.Enqueue(1000 * (OpticalElement.transform.position - transform.position), 500);



        }

        private void DebugLines()
        {
            Debug.DrawRay(transform.position, 1000 * (OpticalElement.transform.position - transform.position));
        }
    }
}