using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpticalProperties : MonoBehaviour {

    public float focal_length = 5;
    public float n_Index = 1.52F;
    //Radii of curvature. 99% of the time, they'll be the same. I just use them as separate variables since the lensmaker equation dictates it.
    public float R_Curve_1 = 25;
    public float R_Curve_2 = 25;
    public float damping_factor;
    //Is this object a mirror? Default is false
    public bool isReflective = false;
    public bool isCurved = true;
    //Distance moved on keypress
    private float movementDistance;


    // Use this for initialization
    void Start()
    {
        damping_factor = Vector3.Magnitude(transform.localScale) / 10;
        movementDistance = 0.3F;
    }

    // Update is called once per frame
    void Update()
    {
        // The collider size will automatically scale with the size of the object

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (true) //focallength > 1.5)
            {
                transform.localScale += new Vector3(movementDistance, 0, 0);
                R_Curve_1 -= 0.25F;
                R_Curve_2 -= 0.25F;
            }
            else
            {
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.localScale.x > 0.1)
            {
                transform.localScale += new Vector3(movementDistance, 0, 0);
                R_Curve_1 += 0.25F;
                R_Curve_2 += 0.25F;
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.localScale += new Vector3(0, movementDistance, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.localScale.y > 0.1)
            {
                transform.localScale += new Vector3(0, movementDistance, 0);
            }
        }
    }


}
