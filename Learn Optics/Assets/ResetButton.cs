using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    InputField FocalLengthIF;
    InputField ObjectDistanceIF;
    InputField ImageDistanceIF;
    InputField mObjectDistanceIF;
    InputField mImageDistanceIF;
    InputField MagnificationIF;

    void Start()
    {
        FocalLengthIF = GameObject.Find("FocalLengthInputField").GetComponent<InputField>();
        ObjectDistanceIF = GameObject.Find("ObjectDistanceInputField").GetComponent<InputField>();
        ImageDistanceIF = GameObject.Find("ImageDistanceInputField").GetComponent<InputField>();
        mObjectDistanceIF = GameObject.Find("mObjectDistanceIF").GetComponent<InputField>();
        mImageDistanceIF = GameObject.Find("mImageDistanceIF").GetComponent<InputField>();
        MagnificationIF = GameObject.Find("MagnificationIF").GetComponent<InputField>();
    }

    // Update is called once per frame
    public void OnClick()
    {
        FocalLengthIF.text = "";
        ObjectDistanceIF.text = "";
        ImageDistanceIF.text = "";
        mObjectDistanceIF.text = "";
        mImageDistanceIF.text = "";
        MagnificationIF.text = "";

    }
}
