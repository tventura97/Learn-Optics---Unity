using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVirtualRays : MonoBehaviour {

    GameObject[] VirtualRays;
	void Start () {
        VirtualRays = GameObject.FindGameObjectsWithTag("VirtualRay");		
	}
	
	public void OnToggle () {
		
        for (int i = 0; i < VirtualRays.Length; i++)
        {
            VirtualRays[i].SetActive(!VirtualRays[i].activeSelf);
        }
	}
}
