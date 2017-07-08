using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class AnimatedLRScript : MonoBehaviour
    {
        private float Delay;
        public bool fire;
        private RaycastHit hit;
        AnimatedLineRenderer LineRenderer;
        GameObject Root;
        // Use this for initialization
        void Start()
        {
            Root = GameObject.Find("Root");
            LineRenderer = GetComponent<AnimatedLineRenderer>();
            Delay = 0.75F;
            if (Physics.Raycast(transform.position, transform.right, out hit))
            {
                print("Click");
                LineRenderer.Enqueue(transform.position);
                LineRenderer.Enqueue(hit.point);
                LineRenderer.Enqueue((new Vector3(Root.transform.position.x + 27.6F, Root.transform.position.y, 0)),5);
                LineRenderer.Enqueue((new Vector3(10000, 0, 0)), 100);

            }
        }
    }


}
