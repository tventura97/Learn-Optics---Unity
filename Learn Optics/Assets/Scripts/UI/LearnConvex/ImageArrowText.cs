using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageArrowText : MonoBehaviour
{

    GameObject ImageArrow;
    GameObject OpticalElement;
    TextMesh ImageArrowTextMesh;

    void Start()
    {
        ImageArrow = GameObject.Find("ImageArrow");
        OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
        ImageArrowTextMesh = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ImageArrow.transform.position.x - OpticalElement.transform.position.x > 0)
        {
            transform.position = new Vector3(ImageArrow.transform.position.x, OpticalElement.transform.position.y + 2);
        }
        else if (ImageArrow.transform.position.x - OpticalElement.transform.position.y <= 0)
        {
            transform.position = new Vector3(ImageArrow.transform.position.x, OpticalElement.transform.position.y - 0.75F);
        }
        ImageArrowTextMesh.text = "di = " + (transform.position.x - OpticalElement.transform.position.x).ToString("F2");

    }
}
