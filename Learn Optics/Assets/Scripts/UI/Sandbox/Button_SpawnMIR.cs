using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_SpawnMIR : MonoBehaviour {


    public GameObject CurvedMirror;
    private GameObject[] OpticalElements;
    public void OnClick()
    {

        OpticalElements = GameObject.FindGameObjectsWithTag("OpticalElement");
        for (int i = 0; i < OpticalElements.Length; i++)
        {
            Destroy(OpticalElements[i]);
        }
        Instantiate(CurvedMirror, transform.TransformPoint(new Vector3(0, 0, 0)), Quaternion.Euler(new Vector3(0, 90, 90)));

    }
}
