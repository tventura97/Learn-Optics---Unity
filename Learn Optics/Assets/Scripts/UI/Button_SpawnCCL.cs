using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_SpawnCCL : MonoBehaviour {

    public GameObject ConcaveLens;
    private GameObject[] OpticalElements;

    public void OnClick()
    {
        OpticalElements = GameObject.FindGameObjectsWithTag("OpticalElement");
        for (int i = 0; i < OpticalElements.Length; i++)
        {
            Destroy(OpticalElements[i]);
        }

        Instantiate(ConcaveLens, transform.TransformPoint(new Vector3(0, 0, 0)), Quaternion.Euler(new Vector3 (0, 90, 0)));


    }
	
}
