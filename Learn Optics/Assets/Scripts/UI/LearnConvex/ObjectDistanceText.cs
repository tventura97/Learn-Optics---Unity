using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDistanceText : MonoBehaviour
{
    TextMesh ObjectDistanceTextMesh;
    GameObject OpticalElement;

    void Start()
    {

    }
    void Update()
    {
        OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
        ObjectDistanceTextMesh = GetComponent<TextMesh>();
        ObjectDistanceTextMesh.text = "do = " + (OpticalElement.transform.position.x - transform.parent.position.x).ToString("F1");
    }
}
