using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateThinLens : MonoBehaviour {

    InputField FocalLengthIF;
    InputField ObjectDistanceIF;
    InputField ImageDistanceIF;
    float FocalLength;
    float ImageDistance;
    float ObjectDistance;
    private void Start()
    {
        FocalLengthIF = GameObject.Find("FocalLengthInputField").GetComponent<InputField>();
        ObjectDistanceIF = GameObject.Find("ObjectDistanceInputField").GetComponent<InputField>();
        ImageDistanceIF = GameObject.Find("ImageDistanceInputField").GetComponent<InputField>();
        
    }

    public void OnClick() {

        if (FocalLengthIF.text == "")
        {
            ImageDistance = float.Parse(ImageDistanceIF.text);
            ObjectDistance = float.Parse(ObjectDistanceIF.text);
            float tempvalue = 1/ImageDistance + 1/ObjectDistance;
            print(tempvalue);
            FocalLength = 1 / tempvalue;
            FocalLengthIF.text = FocalLength.ToString("F1");
        }
        else if (ObjectDistanceIF.text == "")
        {
            FocalLength = float.Parse(FocalLengthIF.text);
            ImageDistance = float.Parse(ImageDistanceIF.text);
            float tempvalue = 1 / FocalLength - 1 / ImageDistance;
            print(tempvalue);
            ObjectDistance = 1 / tempvalue;
            ObjectDistanceIF.text = ObjectDistance.ToString("F1");
        }
        else if (ImageDistanceIF.text == "")
        {
            FocalLength = float.Parse(FocalLengthIF.text);
            ObjectDistance = float.Parse(ObjectDistanceIF.text);
            float tempvalue = 1 / FocalLength - 1 / ObjectDistance;
            print(tempvalue);
            ImageDistance = 1 / tempvalue;
            ImageDistanceIF.text = ImageDistance.ToString("F1");
        }

    }
}
