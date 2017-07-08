using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class ObjectArrowControls : MonoBehaviour
    {
        GameObject FocalPointRay;
        GameObject ParallelRay;
        GameObject OpticalCenterRay;
        AnimatedLineRenderer FocalPointRayALR;
        AnimatedLineRenderer ParallelRayALR;
        AnimatedLineRenderer OpticalCenterRayALR;
        AnimatedLineRenderer VFocalPointRayALR;
        AnimatedLineRenderer VParallelRayALR;
        AnimatedLineRenderer VOpticalCenterRayALR;
        private bool isSelected;
        private Vector3 origin;
        private void Start()
        {
            FocalPointRay = GameObject.Find("FocalPointRay");
            ParallelRay = GameObject.Find("ParallelRay");
            OpticalCenterRay = GameObject.Find("OpticalCenterRay");
            FocalPointRayALR = FocalPointRay.GetComponent<AnimatedLineRenderer>();
            ParallelRayALR = ParallelRay.GetComponent<AnimatedLineRenderer>();
            OpticalCenterRayALR = OpticalCenterRay.GetComponent<AnimatedLineRenderer>();
            VFocalPointRayALR = GameObject.Find("VirtualFocalPointRay").GetComponent<AnimatedLineRenderer>();
            VParallelRayALR = GameObject.Find("VirtualParallelRay").GetComponent<AnimatedLineRenderer>();
            VOpticalCenterRayALR = GameObject.Find("VirtualOpticalCenterRay").GetComponent<AnimatedLineRenderer>();
            origin = Camera.main.gameObject.transform.position;
            isSelected = false;
        }
        void Update()
        {
            if (Input.touchCount >= 1)
            {
                checkSelected();

            }
            if (isSelected)
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                transform.position = new Vector3(point.x, origin.y + 2, 0);


            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                ResetALRs();
                transform.position -= new Vector3(0.5F, 0, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                ResetALRs();
                transform.position += new Vector3(0.5F, 0, 0);
            }
           

        }

        public void ResetALRs()
        {
            FocalPointRayALR.Reset();
            ParallelRayALR.Reset();
            OpticalCenterRayALR.Reset();
            VFocalPointRayALR.Reset();
            VParallelRayALR.Reset();
            VOpticalCenterRayALR.Reset();
        }

        private void checkSelected()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider && Input.touchCount >= 1)
            {
                isSelected = true;

            }
        }
    }
}
