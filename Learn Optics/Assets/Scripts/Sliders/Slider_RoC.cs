using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider_RoC : MonoBehaviour {

    GameObject OpticalElement;
    float initial_local_scale;
    // Use this for initialization
    void Start()
    {
        OpticalElement = transform.GetChild(0).gameObject;
        initial_local_scale = OpticalElement.transform.localScale.z;
    }

    public void onRoCChanged()
    {
        print("Value Has Changed");
        OpticalElement.transform.localScale += new Vector3(0, 0, 0.5F);
    }
    // Update is called once per frame
    void Update()
    {

    }
}

