using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlayScene : MonoBehaviour {

	GameObject ScenePanel;

	void Start () {
		ScenePanel = GameObject.Find ("PromptPanel");
	}
	
	public void OnToggle () {
		ScenePanel.SetActive (!ScenePanel.activeSelf);
        
	}
}
