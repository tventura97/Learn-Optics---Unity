using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateThinLens : MonoBehaviour
{

    InputField FocalLengthIF;
    InputField ObjectDistanceIF;
    InputField ImageDistanceIF;
    GameObject OpticalElement;
    GameObject ObjectArrow;
    GameObject ImageArrow;
    Button ResetInputFields;
    float FocalLength;
    float ImageDistance;
    float ObjectDistance;
    private void Start()
    {
        FocalLengthIF = GameObject.Find("FocalLengthInputField").GetComponent<InputField>();
        ObjectDistanceIF = GameObject.Find("ObjectDistanceInputField").GetComponent<InputField>();
        ImageDistanceIF = GameObject.Find("ImageDistanceInputField").GetComponent<InputField>();
        ResetInputFields = GameObject.Find("ResetButton").GetComponent<Button>();
    }
    private void Update()
    {
        OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
        ObjectArrow = GameObject.Find("ObjectArrow");
        ImageArrow = GameObject.Find("ImageArrow");
    }
    public void OnClick()
    {
        //If they're all filled, just clear them and calculate image distance using the given object distance and focal length
        if (!(FocalLengthIF.text == "" && ObjectDistanceIF.text == "" && ImageDistanceIF.text == ""))
        {
            string OD = ObjectDistanceIF.text;
            string F = FocalLengthIF.text;
            FocalLength = float.Parse(F);
            ObjectDistance = float.Parse(OD);
            float tempvalue = 1 / FocalLength - 1 / ObjectDistance;
            print(tempvalue);
            ImageDistance = 1 / tempvalue;

            ClearTLInputFields();

            ObjectDistanceIF.text = OD;
            FocalLengthIF.text = F;
            ImageDistanceIF.text = ImageDistance.ToString("F1");

        }
        //If they're all empty, just pull the values from the scene

        if (FocalLengthIF.text == "" && ObjectDistanceIF.text == "" && ImageDistanceIF.text == "")
        {
            if (OpticalElement.GetComponent<Properties_Optical>().isConvexReflective || (OpticalElement.GetComponent<Properties_Optical>().isConcave && !OpticalElement.GetComponent<Properties_Optical>().isReflective))
            {
                FocalLengthIF.text = (-Mathf.Abs(OpticalElement.transform.position.x - GameObject.Find("F1").transform.position.x)).ToString("F1");
            }
            else
            {
                FocalLengthIF.text = (Mathf.Abs(OpticalElement.transform.position.x - GameObject.Find("F1").transform.position.x)).ToString("F1");
            }

            FocalLength = float.Parse(FocalLengthIF.text);
            ObjectDistanceIF.text = Mathf.Abs(OpticalElement.transform.position.x - ObjectArrow.transform.position.x).ToString("F1");

            ObjectDistance = float.Parse(ObjectDistanceIF.text);
            float tempvalue = 1 / FocalLength - 1 / ObjectDistance;
            print(tempvalue);
            ImageDistance = 1 / tempvalue;
            ImageDistanceIF.text = ImageDistance.ToString("F1");
        }
        else
        {
            if (FocalLengthIF.text == "")
            {
                string ID = ImageDistanceIF.text;
                string OD = ObjectDistanceIF.text;
                //Store values cause we're gonna clear the input fields to make sure the values update properly
                ImageDistance = float.Parse(ID);
                ObjectDistance = float.Parse(OD);
                float tempvalue = 1 / ImageDistance + 1 / ObjectDistance;
                print(tempvalue);
                FocalLength = 1 / tempvalue;

                ClearTLInputFields();

                ObjectDistanceIF.text = OD;
                ImageDistanceIF.text = ID;
                FocalLengthIF.text = FocalLength.ToString("F1");
            }
            else if (ObjectDistanceIF.text == "")
            {
                string F = FocalLengthIF.text;
                string ID = ImageDistanceIF.text;
                FocalLength = float.Parse(F);
                ImageDistance = float.Parse(ID);
                float tempvalue = 1 / FocalLength - 1 / ImageDistance;
                print(tempvalue);
                ObjectDistance = 1 / tempvalue;

                ClearTLInputFields();

                ImageDistanceIF.text = ID;
                FocalLengthIF.text = F;
                ObjectDistanceIF.text = ObjectDistance.ToString("F1");
            }
            else if (ImageDistanceIF.text == "")
            {
                string OD = ObjectDistanceIF.text;
                string F = FocalLengthIF.text;
                FocalLength = float.Parse(F);
                ObjectDistance = float.Parse(OD);
                float tempvalue = 1 / FocalLength - 1 / ObjectDistance;
                print(tempvalue);
                ImageDistance = 1 / tempvalue;

                ClearTLInputFields();

                ObjectDistanceIF.text = OD;
                FocalLengthIF.text = F;
                ImageDistanceIF.text = ImageDistance.ToString("F1");
            }
        }

    }

    public void ClearTLInputFields()
    {
        //Clear InputFields
        FocalLengthIF.text = "";
        ObjectDistanceIF.text = "";
        ImageDistanceIF.text = "";
    }
}
