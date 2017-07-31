using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.AnimatedLineRenderer
{
    public class ImageInfoPanelScript : MonoBehaviour
    {

        GameObject ImageArrow, ObjectArrow, OpticalElement, C;
        TextMesh Orientation, Magnification, Type;
        bool isUpright, isConcave, isConcaveReflective, isConvexReflective;
        float FocalLength, CentOfCurv;

        private void Start()
        {

            isUpright = true;
        }
        void Update()
        {
            //References need to be dynamically updated because these objects can be changed by the user at runtime
            Orientation = GameObject.Find("ImageOrientation").GetComponent<TextMesh>();
            Magnification = GameObject.Find("ImageMagnification").GetComponent<TextMesh>();
            Type = GameObject.Find("ImageType").GetComponent<TextMesh>();
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

            SetPosition();
        }

        private void SetPosition()
        {
            if (isConvexReflective || isConcaveReflective && Mathf.Abs(ObjectArrow.transform.position.x - OpticalElement.transform.position.x) < FocalLength || isConcave)
            {
                isUpright = true;
            }

            else
            {
                isUpright = false;
            }


            //if upright, materialize under the image arrow. if not upright, materialize above the image arrow.
            if (isUpright)
            {
                transform.position = new Vector3(ImageArrow.transform.position.x, OpticalElement.transform.position.y - GetOffset());
            }
            else
            {
                transform.position = new Vector3(ImageArrow.transform.position.x, OpticalElement.transform.position.y + GetOffset());
            }
        }
        private float GetOffset()
        {
            if (isUpright)
            {
                return 7.5F;
            }
            else
            {
                return 7.5F;
            }

        }
    }
}