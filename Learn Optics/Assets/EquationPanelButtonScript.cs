using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationPanelButtonScript : MonoBehaviour {

    Animator EquationPanelAnimator;
    Animator ButtonAnimator;

	void Start () {
        EquationPanelAnimator = GameObject.Find("EquationPanel").GetComponent<Animator>();
        ButtonAnimator = GetComponent<Animator>();
        ButtonAnimator.SetBool("toggleMenu", false);

    }

    public void OnClick () {
        EquationPanelAnimator.SetBool("toggleMenu", !EquationPanelAnimator.GetBool("toggleMenu"));
        ButtonAnimator.SetBool("toggleMenu", !ButtonAnimator.GetBool("toggleMenu"));

    }
}
