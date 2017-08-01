using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseFBPanel : MonoBehaviour
{

    Animator FeedbackPanelAnimator;

    void Start()
    {
        FeedbackPanelAnimator = GameObject.Find("FeedbackPanel").GetComponent<Animator>();
    }

    public void OnClick()
    {
        FeedbackPanelAnimator.SetBool("FeedbackPanel", false);
    }
}
