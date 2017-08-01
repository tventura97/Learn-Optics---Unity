﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DigitalRuby.AnimatedLineRenderer
{
    public class ImageMagnificationTextScript : MonoBehaviour {

        GameObject ImageArrow, ObjectArrow, OpticalElement, C;
        TextMesh Orientation, Magnification, Type;
        bool isUpright, isConcave, isConcaveReflective, isConvexReflective;
        float FocalLength, CentOfCurv;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update()
        {
            //References need to be dynamically updated because these objects can be changed by the user at runtime
            Magnification = GetComponent<TextMesh>();
            ImageArrow = GameObject.Find("ImageArrow");
            ObjectArrow = GameObject.Find("ObjectArrow");
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
            isConcave = OpticalElement.GetComponent<Properties_Optical>().isConcave;
            isConcaveReflective = OpticalElement.GetComponent<Properties_Optical>().isConcaveReflective;
            isConvexReflective = OpticalElement.GetComponent<Properties_Optical>().isConvexReflective;
            FocalLength = GameObject.Find("GenerateQuizButton").GetComponent<GenerateQuizScript>().FocalLength;
            try
            {
                C = GameObject.Find("C");
            }
            catch (MissingReferenceException e)
            {
                print(e);
            }
            if (C != null)
            {
                CentOfCurv = Mathf.Abs(C.transform.position.x - OpticalElement.transform.position.x);
            }

            SetTextMeshes();
        }

        private void SetTextMeshes()
        {
            //Magnification is either enlarged or diminished
            //If image arrow is bigger, it's enlarged
            if (ImageArrow.transform.localScale.y/ObjectArrow.transform.localScale.y > 1)
            {
                Magnification.text = "Enlarged";
            }
            else
            {
                Magnification.text = "Diminished";
            }


        }
    }
}