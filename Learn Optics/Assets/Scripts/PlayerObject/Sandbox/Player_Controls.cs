using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controls : MonoBehaviour
{

    Vector3 origin;
    bool isSelected;
    RaycastHit hit;
    // Use this for initialization
    void Start()
    {

        origin = Camera.main.gameObject.transform.position;
        isSelected = false;

    }

    void FixedUpdate()
    {
        if (Input.touchCount >= 1)
        {
            checkSelected();

        }
        else
        {
            isSelected = false;
            transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (isSelected)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            transform.position = new Vector3(point.x, point.y, 0);
            transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = true;


        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.eulerAngles += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles += new Vector3(0, 0, -1);
        }
    }
    void checkSelected()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider && Input.touchCount >= 1)
        {
            isSelected = true;

        }
    }
}

