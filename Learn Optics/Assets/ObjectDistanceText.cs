using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDistanceText : MonoBehaviour
{
    TextMesh ObjectDistanceTextMesh;
    GameObject OpticalElement;

    void Start()
    {
        OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
        ObjectDistanceTextMesh = GetComponent<TextMesh>();
    }
    void Update()
    {
        ObjectDistanceTextMesh.text = (OpticalElement.transform.position.x - transform.parent.position.x).ToString("F2");
    }
}
