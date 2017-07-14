using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateMagnification : MonoBehaviour {

    InputField mObjectDistanceIF;
    InputField mImageDistanceIF;
    InputField MagnificationIF;
    float ObjectDistance;
    float ImageDistance;
    float Magnification;

	void Start () {
        mObjectDistanceIF = GameObject.Find("mObjectDistanceIF").GetComponent<InputField>();
        mImageDistanceIF = GameObject.Find("mImageDistanceIF").GetComponent<InputField>();
        MagnificationIF = GameObject.Find("MagnificationIF").GetComponent<InputField>();
    }
	
	public void OnClick ()
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
