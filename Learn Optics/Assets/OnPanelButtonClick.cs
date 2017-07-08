using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnPanelButtonClick : MonoBehaviour {

    public GameObject AnimatedLightEmitter;
    GameObject promptPanelText;
    GameObject LightEmitter;
    GameObject Root;
    Animator MainCamera;
    GameObject LightEmitterHolder;
    Animator PrismHolder;
    Text panelText;
    string [] texts;
    int promptNumber;
    int counter;
	// Use this for initialization
	void Start () {
        Root = GameObject.Find("Root");
        MainCamera = GameObject.Find("Main Camera").GetComponent<Animator>();
        LightEmitterHolder = GameObject.Find("LightEmitterHolder");
        PrismHolder = GameObject.Find("PrismHolder").GetComponent<Animator>();
        //Look I know this is a dumb way to initialize it but I'm lazy right now
        texts = new string[] { "This is a convex, or converging, lens.", "It is called a converging lens because rays of light that pass through the lens converge at the lens' focal point.",
            "To understand how light refracts through a convex lens,", "Imagine that the lens is really just a series of prisms stacked on top of each other.",
            "When light passes through a prism, it always bends towards the base of the prism."};
        promptPanelText = GameObject.Find("PromptPanelText");
        print(GameObject.Find("PromptPanelText").GetComponent<Text>());
        panelText = promptPanelText.GetComponent<Text>();
        counter = 1;
        panelText.text = texts[0];
        promptNumber = texts.Length;
        LightEmitterHolder = GameObject.Find("LightEmitterHolder");
        LightEmitterHolder.SetActive(false);


    }

    // Update is called once per frame
    public void onClick () {

        switch (counter)
        {
            case 1:
                MainCamera.SetBool("Pan_FocalPoint", true);
                LightEmitterHolder.SetActive(true);
                break;

            case 2:
                MainCamera.SetBool("Pan_FocalPoint", false);
                break;

            case 3:
                LightEmitterHolder.SetActive(false);
                MainCamera.SetBool("Pan_TopofLens", true);
                PrismHolder.SetBool("triggerPrism", true);
                break;

            case 4:
                Instantiate(AnimatedLightEmitter, new Vector3(Root.transform.position.x - 80, Root.transform.position.y + 6, 0), Quaternion.identity, GameObject.Find("Root").transform);
                Instantiate(AnimatedLightEmitter, new Vector3(Root.transform.position.x - 80, Root.transform.position.y + 9, 0), Quaternion.identity, GameObject.Find("Root").transform);
                break;

            case 5:
                GameObject[] AnimatedLightEmitters = GameObject.FindGameObjectsWithTag("AnimatedLightEmitter");
                for (int i = 0; i < AnimatedLightEmitters.Length; i++)
                {
                    Destroy(AnimatedLightEmitters[i]);
                }
                MainCamera.SetBool("Pan_TopofLens", false);
                PrismHolder.SetBool("triggerPrism", false);
                break;

            case 6:
                MainCamera.SetBool("ZoomOut", true);
                break;


        }

        if (counter < promptNumber)
        {
            panelText.text = texts[counter];
        }
        counter++;


	}
}
