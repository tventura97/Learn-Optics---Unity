using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageArrowHolderPosition : MonoBehaviour {

	
	void Update () {
        transform.position = transform.GetChild(0).transform.position;
		
	}
}
