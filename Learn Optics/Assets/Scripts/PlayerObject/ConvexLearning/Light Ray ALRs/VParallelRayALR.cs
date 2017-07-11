using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class VParallelRayALR : MonoBehaviour
    {

        private AnimatedLineRenderer animatedLineRenderer;
        private float FocalLength;
        private float ObjectDistance;
        private float ImageDistance;
        private float Magnification;
        private Vector3 FocalPoint;
        private RaycastHit hit;
        private GameObject OpticalElement;
        public bool VirtualImage;

        private void Start()
        {
            animatedLineRenderer = GetComponent<AnimatedLineRenderer>();
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            FocalLength = 12;
            FocalPoint = new Vector3(OpticalElement.transform.position.x + FocalLength, OpticalElement.transform.position.y, OpticalElement.transform.position.z);
            VirtualImage = false;

        }

        private void Update()
        {
            if (Mathf.Abs(transform.position.x - OpticalElement.transform.position.x) < FocalLength)
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
                animatedLineRenderer.Enqueue(CalculateFinalPosition(), 0.5F);
                animatedLineRenderer.Enqueue(1000 * (CalculateFinalPosition() - FocalPoint), 100);
            }
        }

        private void DebugLines()
        {
            Debug.DrawRay(transform.position, new Vector3(OpticalElement.transform.position.x, hit.point.y, hit.point.z) - transform.position);
            Debug.DrawRay(new Vector3(OpticalElement.transform.position.x, hit.point.y, hit.point.z), 100 * (FocalPoint - new Vector3(OpticalElement.transform.position.x, hit.point.y, hit.point.z)));
        }

        private Vector3 CalculateFinalPosition()
        {
            float FinalX = OpticalElement.transform.position.x;
            float FinalY = transform.position.y;
            float FinalZ = OpticalElement.transform.position.z;
            return new Vector3(FinalX, FinalY, FinalZ);
        }
    }
}
