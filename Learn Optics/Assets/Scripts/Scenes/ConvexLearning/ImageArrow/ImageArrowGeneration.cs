using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer
{
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
        GameObject OpticalElement;
        bool Quizzing;
        bool isSelected;
        Vector3 origin;

        void Start()
        {
            ObjectArrow = GameObject.Find("ObjectArrow");
            spriteRenderer = GetComponent<SpriteRenderer>();
            FocalLength = 12;
            Magnification = 1;
            InitialScale = transform.localScale;
            SpriteBounds = spriteRenderer.bounds.size.y;
            origin = Camera.main.gameObject.transform.position;
            OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
        }
        //Note that all distances are relative to the lens, which is located at Root.transform.position
        void Update()
        {
            Quizzing = GameObject.Find("QuizToggle").GetComponent<Toggle>().isOn;
            ObjectDistance = GameObject.Find("Root").transform.position.x - GameObject.Find("ObjectArrow").transform.position.x;
            ImageDistance = Mathf.Abs(1 / (1 / ObjectDistance - 1 / FocalLength));
            Magnification = Mathf.Abs(ImageDistance / ObjectDistance);     // I multiply by the initial scale of the object because it doesn't start at 1. It's 1.25F, I think.
            transform.localScale = new Vector3(InitialScale.x, Magnification, InitialScale.z);

            //If not quizzing
            if (!Quizzing)
            {
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
            else if (Quizzing)
            {
                if (Input.touchCount >= 1)
                {
                    checkSelected();

                }
                if (true)
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        transform.position -= new Vector3(0.5F, 0, 0);
                    }
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        transform.position += new Vector3(0.5F, 0, 0);
                    }
                    if (transform.position.x - OpticalElement.transform.position.x < 0)
                    {
                        transform.position = new Vector3(transform.position.x, OpticalElement.transform.position.y + 2 * Magnification, 0);

                        spriteRenderer.flipY = false;
                    }

                    else if (transform.position.x - OpticalElement.transform.position.x >= 0)
                    {
                        transform.position = new Vector3(transform.position.x, OpticalElement.transform.position.y - 2 * Magnification, 0);
                        spriteRenderer.flipY = true;
                    }
                    //Touch Controls
                    /*
                    if (transform.position.x - OpticalElement.transform.position.x < 0)
                    {
                        Vector3 point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        transform.position = new Vector3(point.x, origin.y + 2 * Magnification, 0);
                        spriteRenderer.flipY = false;
                    }
                    else if (transform.position.x - OpticalElement.transform.position.x >= 0)
                    {
                        Vector3 point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        transform.position = new Vector3(point.x, origin.y - 2 * Magnification, 0);
                        spriteRenderer.flipY = true;
                    }
                    */

                }


            }
        }

        private void checkSelected()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider && Input.touchCount >= 1)
            {
                isSelected = true;

            }
            else
            {
                isSelected = false;
            }
        }

        public void ResetALR()
        {
            ObjectArrow.GetComponent<ObjectArrowControls>().ResetALRs();
        }
    }
}

