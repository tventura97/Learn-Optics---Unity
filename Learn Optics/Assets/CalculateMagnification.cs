using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateMagnification : MonoBehaviour {

    GameObject OpticalElement;
    GameObject ObjectArrow;
    Button ResetButton;
    InputField mObjectDistanceIF;
    InputField mImageDistanceIF;
    InputField MagnificationIF;
    float ObjectDistance;
    float ImageDistance;
    float Magnification;
    float FocalLength;

	void Start () {
        mObjectDistanceIF = GameObject.Find("mObjectDistanceIF").GetComponent<InputField>();
        mImageDistanceIF = GameObject.Find("mImageDistanceIF").GetComponent<InputField>();
        MagnificationIF = GameObject.Find("MagnificationIF").GetComponent<InputField>();
        ResetButton = GameObject.Find("ResetButton").GetComponent<Button>();
    }

    private void Update()
    {
        ObjectArrow = GameObject.Find("ObjectArrow");
        OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
    }

    public void OnClick ()
    {
        //If all input fields are empty, pull values from scene and calculate
        if (mObjectDistanceIF.text == "" && mImageDistanceIF.text == "" && MagnificationIF.text == "")
        {
            mObjectDistanceIF.text = "";
            mImageDistanceIF.text = "";
            MagnificationIF.text = "";
            ObjectDistance = Mathf.Abs(GameObject.Find("ObjectArrow").transform.position.x - GameObject.FindGameObjectWithTag("OpticalElement").transform.position.x);
            if (OpticalElement.GetComponent<Properties_Optical>().isConvexReflective || (OpticalElement.GetComponent<Properties_Optical>().isConcave && !OpticalElement.GetComponent<Properties_Optical>().isReflective))
            {
                FocalLength = (-Mathf.Abs(OpticalElement.transform.position.x - GameObject.Find("F1").transform.position.x));
            }
            else
            {
                FocalLength = (Mathf.Abs(OpticalElement.transform.position.x - GameObject.Find("F1").transform.position.x));
            }

            float tempvalue = 1 / FocalLength - 1 / ObjectDistance;
            print(tempvalue);
            ImageDistance = 1 / tempvalue;
            Magnification = -ImageDistance / ObjectDistance;
            mObjectDistanceIF.text = ObjectDistance.ToString("F1");
            mImageDistanceIF.text = ImageDistance.ToString("F1");
            MagnificationIF.text = Magnification.ToString("F1");
        }
        else
        {
            if (mObjectDistanceIF.text == "")
            {
                ImageDistance = float.Parse(mImageDistanceIF.text);
                Magnification = float.Parse(MagnificationIF.text);
                ObjectDistance = -ImageDistance / Magnification;
                mObjectDistanceIF.text = ObjectDistance.ToString("F1");
            }
            else if (mImageDistanceIF.text == "")
            {
                Magnification = float.Parse(MagnificationIF.text);
                ObjectDistance = float.Parse(mObjectDistanceIF.text);
                ImageDistance = -Magnification * ObjectDistance;
                mImageDistanceIF.text = ImageDistance.ToString("F1");

            }
            else if (MagnificationIF.text == "")
            {
                ObjectDistance = float.Parse(mObjectDistanceIF.text);
                ImageDistance = float.Parse(mImageDistanceIF.text);
                Magnification = -ImageDistance / ObjectDistance;
                MagnificationIF.text = Magnification.ToString("F1");
            }
        }
    }
}
