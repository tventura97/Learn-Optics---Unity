using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{

    GameObject OpticalElement;
    Slider n_Index_Slider;
    Slider RoC_Slider;
    Vector3 initial_RoC;
    // Use this for initialization
    void Start()
    {
        
        n_Index_Slider = transform.parent.GetChild(1).transform.GetChild(1).transform.GetChild(0).transform.GetComponent<Slider>();
        RoC_Slider = transform.parent.GetChild(1).transform.GetChild(1).transform.GetChild(1).transform.GetComponent<Slider>();
    }

    private void FixedUpdate()
    {
        OpticalElement = GameObject.FindGameObjectsWithTag("OpticalElement")[0];                //There will only ever be one (Unless the MOE feature has been implemented)
        initial_RoC = OpticalElement.transform.localScale;
        
    }

    public void onValueChanged()
    {
        float n_Index_slider_value = n_Index_Slider.value;
        float RoC_Slider_value = RoC_Slider.value;
    
 

        OpticalElement.GetComponent<Properties_Optical>().n_Index += n_Index_slider_value;
        OpticalElement.transform.localScale = new Vector3(initial_RoC.x, initial_RoC.y, initial_RoC.z - RoC_Slider_value);
        print(OpticalElement.transform.localScale);
    }
    // Update is called once per frame
    void Update()
    {

    }
}