using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class SetPoints : MonoBehaviour
    {
        AnimatedLineRenderer RayRenderer;
        void Start()
        {

        }

        public void InitializeALR()
        {
            RayRenderer = GetComponent<AnimatedLineRenderer>();

        }
        public void SetLinePoints(Vector3 From, Vector3 To, float duration)
        {
            //Initialize the AnimatedLightRenderer here. When this script is called, it probably doesn't call Start(), so this function thinks that RayRenderer isn't initialized
            RayRenderer.Enqueue(From);
            RayRenderer.Enqueue(To, duration);


        }
        public void SetLinePoint(Vector3 Point, float duration)
        {
            RayRenderer.Enqueue(Point, duration);


        }
    }
}