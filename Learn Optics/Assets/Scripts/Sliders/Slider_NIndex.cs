using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_NIndex : MonoBehaviour
{

    GameObject OpticalElement;
    Slider n_Index_Slider;
    Slider RoC_Slider;
    Vector3 initial_RoC;
    // Use this for initialization
    void Start()
    {
        OpticalElement = transform.GetChild(0).gameObject;
        n_Index_Slider = transform.parent.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.GetComponent<Slider>();
        RoC_Slider = transform.parent.GetChild(1).transform.GetChild(1).transform.GetChild(1).transform.GetComponent<Slider>();
        initial_RoC = OpticalElement.transform.localScale;
    }

    private void FixedUpdate()
    {
        //print(OpticalElement.transform.localScale);
    }

    public void onValueChanged()
    {
        print("SOMETHING IS HAPPENING");
        float n_Index_slider_value = n_Index_Slider.value;
        float RoC_Slider_value = RoC_Slider.value;

        OpticalElement.GetComponent<Properties_Optical>().n_Index += n_Index_slider_value;
        OpticalElement.transform.localScale = new Vector3(initial_RoC.x, initial_RoC.y, initial_RoC.z - RoC_Slider_value*5);
        
    }
    // Update is called once per frame
    void Update()
    {

    }
}
