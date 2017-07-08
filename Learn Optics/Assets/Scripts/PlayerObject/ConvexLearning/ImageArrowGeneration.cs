using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImageArrowGeneration : MonoBehaviour
{

    //Thin lens approximation of the object-image relationship: 1/do + 1/di = 1/f
    //Where do is object distance, di is image distance, and f is the focal length of the lens. An arbitrary length will be picked for the purposes of this learning module.

    public float FocalLength;
    public float ObjectDistance;
    public float ImageDistance;
    public float Magnification;
    float SpriteBounds;
    private Vector3 InitialScale;
    private SpriteRenderer spriteRenderer;
    GameObject ObjectArrow;

    void Start()
    {
        ObjectArrow = GameObject.Find("ObjectArrow");
        spriteRenderer = GetComponent<SpriteRenderer>();
        FocalLength = 12;
        Magnification = 1;
        InitialScale = transform.localScale;
        SpriteBounds = spriteRenderer.bounds.size.y;
    }
    //Note that all distances are relative to the lens, which is located at Root.transform.position
    void Update()
    {
        ObjectDistance = GameObject.Find("Root").transform.position.x - GameObject.Find("ObjectArrow").transform.position.x;
        ImageDistance = Mathf.Abs(1 / (1 / ObjectDistance - 1 / FocalLength));
        Magnification = Mathf.Abs(ImageDistance / ObjectDistance);     // I multiply by the initial scale of the object because it doesn't start at 1. It's 1.25F, I think.
        transform.localScale = new Vector3(InitialScale.x, Magnification, InitialScale.z);

        if (ObjectDistance < FocalLength)
        {
            spriteRenderer.flipY = false;
            transform.position = new Vector3(GameObject.Find("Root").transform.position.x - ImageDistance, GameObject.Find("Root").transform.position.y + Magnification * 2, 0);
        }
        else
        {
            spriteRenderer.flipY = true;
            transform.position = new Vector3(GameObject.Find("Root").transform.position.x + ImageDistance, GameObject.Find("Root").transform.position.y - Magnification * 2, 0);
        }
    }
}

