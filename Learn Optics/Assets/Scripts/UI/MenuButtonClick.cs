using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonClick : MonoBehaviour
{


    Animator animator_OpticalMenuPanel;
    Animator animator_ParameterMenuPanel;
    Animator animator_button;
    void Start()
    {
        animator_button = GetComponent<Animator>();           //Get animator of button
        animator_OpticalMenuPanel = transform.parent.gameObject.transform.parent.gameObject.GetComponent<Animator>();                       //Get animator of Optical Menu Panel
        animator_ParameterMenuPanel = transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.GetChild(1).GetComponent<Animator>();       //Get animator of Parmeter Menu Panel
        //Initialize default state (Optical Menu hidden, Parameter menu shown, button pointing left)
        animator_button.SetBool("toggleMenu", false);
        animator_OpticalMenuPanel.SetBool("toggleMenu", false);
        animator_ParameterMenuPanel.SetBool("toggleMenu", false);

    }
    private void Update()
    {

    }
    public void toggleMenu()
    {
        animator_button.SetBool("toggleMenu", !animator_button.GetBool("toggleMenu"));
        animator_OpticalMenuPanel.SetBool("toggleMenu", !animator_OpticalMenuPanel.GetBool("toggleMenu"));
        animator_ParameterMenuPanel.SetBool("toggleMenu", !animator_ParameterMenuPanel.GetBool("toggleMenu"));

    }
}