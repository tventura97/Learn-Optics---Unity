using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class AnimatedLRScript : MonoBehaviour
    {
        private float FocalPoint;
        private Vector3 FocalPointVector;
        public bool fire;
        private RaycastHit hit;
        AnimatedLineRenderer LineRenderer;
        GameObject OpticalElement;
        // Use this for initialization
        void Start()
        {
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            LineRenderer = GetComponent<AnimatedLineRenderer>();
            FocalPoint = 12;
            FocalPointVector = new Vector3(OpticalElement.transform.position.x + FocalPoint, OpticalElement.transform.position.y, 0);
            if (Physics.Raycast(transform.position, transform.right, out hit))
            {
                print("Click");
                LineRenderer.Enqueue(transform.position);
                LineRenderer.Enqueue(hit.point);
                LineRenderer.Enqueue(FocalPointVector, 0.5F);
                LineRenderer.Enqueue(1000 * (FocalPointVector - hit.point), 50);

            }
        }
    }


}
