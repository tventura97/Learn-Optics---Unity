using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRealRays : MonoBehaviour {

    GameObject[] RealRays;

	void Start () {
        RealRays = GameObject.FindGameObjectsWithTag("RealRay");
	}
	
	public void OnToggle () {
		for (int i = 0; i < RealRays.Length; i++)
        {
            RealRays[i].SetActive(!RealRays[i].activeSelf);
        }
	}
}
