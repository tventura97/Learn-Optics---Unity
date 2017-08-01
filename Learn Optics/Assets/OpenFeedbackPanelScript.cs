using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFeedbackPanelScript : MonoBehaviour
{
    Animator FeedbackPanelAnimator;

    private void Start()
    {
        FeedbackPanelAnimator = GameObject.Find("FeedbackPanel").GetComponent<Animator>();
    }

    public void OnClick()
    {
        FeedbackPanelAnimator.SetBool("FeedbackPanel", true);
    }

}
