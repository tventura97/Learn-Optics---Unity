using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{

    GameObject OpticalElement;
    Slider NIndexSlider;
    Slider RoCSlider;
    Slider HeightSlider;
    Vector3 InitialScale;
    // Use this for initialization
    void Start()
    {
        NIndexSlider = GameObject.Find("NIndexSlider").GetComponent<Slider>();
        RoCSlider = GameObject.Find("RoCSlider").GetComponent<Slider>();
        HeightSlider = GameObject.Find("HeightSlider").GetComponent<Slider>();


    }

    private void FixedUpdate()
    {
        OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");                //There will only ever be one (Unless the MOE feature has been implemented)
        InitialScale = GameObject.Find("Content").GetComponent<SpawnElements>().InitialScale;

    }

    public void onValueRoCChanged()
    {
        if (OpticalElement.name == "ConvexLens" || OpticalElement.name == "ConvexLens(Clone)")
        {
            OpticalElement.transform.localScale = new Vector3(InitialScale.x, InitialScale.y, InitialScale.z - 10 * RoCSlider.value);
        }

        if (OpticalElement.name == "ConcaveLens(Clone)")
        {
            OpticalElement.transform.localScale = new Vector3(InitialScale.x, InitialScale.y, InitialScale.z - 2 * RoCSlider.value); 

        }

        if (OpticalElement.name == "ConcaveMirror(Clone)")
        {
            OpticalElement.transform.localScale = new Vector3(InitialScale.x, InitialScale.y, InitialScale.z + 8 * RoCSlider.value);

        }

        if (OpticalElement.name == "ConvexMirror(Clone)")
        {
            OpticalElement.transform.localScale = new Vector3(InitialScale.x, InitialScale.y, InitialScale.z + 8 * RoCSlider.value);

        }
    }

    public void onValueHeightChanged()
    {
        if (OpticalElement.name == "ConvexLens" || OpticalElement.name == "ConvexLens(Clone)")
        {
            OpticalElement.transform.localScale = new Vector3(InitialScale.x + 8 * HeightSlider.value, InitialScale.y, InitialScale.z);
        }

        if (OpticalElement.name == "ConcaveLens(Clone)")
        {
            OpticalElement.transform.localScale = new Vector3(InitialScale.x, InitialScale.y + 4 * HeightSlider.value, InitialScale.z);
        }

        if (OpticalElement.name == "ConcaveMirror(Clone)")
        {
            OpticalElement.transform.localScale = new Vector3(InitialScale.x + 4 * HeightSlider.value, InitialScale.y , InitialScale.z);

        }

        if (OpticalElement.name == "ConvexMirror(Clone)")
        {
            OpticalElement.transform.localScale = new Vector3(InitialScale.x + 4 * HeightSlider.value, InitialScale.y , InitialScale.z);

        }
    }

    public void onValueNIndexChanged()
    {
        if (OpticalElement.name == "ConvexLens" || OpticalElement.name == "ConvexLens(Clone)")
        {
        }

        if (OpticalElement.name == "ConcaveLens(Clone)")
        {

        }

        if (OpticalElement.name == "ConcaveMirror(Clone)")
        {

        }

        if (OpticalElement.name == "ConvexMirror(Clone)")
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}