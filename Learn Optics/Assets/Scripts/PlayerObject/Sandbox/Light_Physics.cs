using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * This class is responsible for handling when light beams collide with optical elements (light, lenses, mirrors)
 * It uses small angle approximations to calculate the angles at which light is refracted. Refer to refraction at spherical
 * surfaces for the exact math (University Physics CH. 34-3)
 **/

public class Light_Physics : MonoBehaviour
{

    private RaycastHit OpticalElementHit;
    private Transform laser;
    private LineRenderer lightBeamRenderer;
    private float focal_length;
    private float n_Index;
    private float lightBeamWidth;
    private bool isReflective = false;
    private bool isCurved = true;
    private bool enableDebugLines;
    private float scalingFactor;

    public void Start()
    {

        lightBeamRenderer = GetComponent<LineRenderer>();
        lightBeamRenderer.enabled = true;
        lightBeamRenderer.useWorldSpace = false; //Sometimes this setting is enabled. This ensures that on start, it is disabled. Though, it should be disabled in the hierarchy.
        lightBeamWidth = 0.25F;

        lightBeamRenderer.startWidth = lightBeamWidth;
        lightBeamRenderer.endWidth = lightBeamWidth;
        enableDebugLines = false;
        //In case the RaycastHit fails to get optical properties, they are initialized here
        isReflective = false;
        isCurved = true;

    }

    public void Update()
    {
        //This must be set to a high number since LineRenderers draw lines between two points. If the point is too close and the user translates the light-emitter, it will appear as though
        //the focal point of the optical element is moving around.
        scalingFactor = 100000000;
        controls();
        if (lightBeamRenderer.enabled)
        {
            shootLightBeam();
        }
        DebugLines(enableDebugLines);
        //Get optical properties from object hit
        isReflective = OpticalElementHit.transform.gameObject.GetComponent<Properties_Optical>().isReflective;
        isCurved = OpticalElementHit.transform.gameObject.GetComponent<Properties_Optical>().isCurved;
        focal_length = OpticalElementHit.transform.gameObject.GetComponent<Properties_Optical>().focal_length;
        n_Index = OpticalElementHit.transform.gameObject.GetComponent<Properties_Optical>().n_Index;

    }

    //Responsible for handling user input and moving/rotating around the light-beam emitter accordingly
    public void controls()
    {
        if (Input.GetKey(KeyCode.P))
        {
            focal_length += 0.1f;
        }
        if (Input.GetKey(KeyCode.O))
        {
            focal_length -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lightBeamRenderer.enabled = !lightBeamRenderer.enabled;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            enableDebugLines = !enableDebugLines;       //toggle debug lines
        }

    }

    public void shootLightBeam()
    {
        if (!isReflective)
        {
            if (Physics.Raycast(transform.position, transform.right, out OpticalElementHit))
            {


                //Sets positions of the vertexes of the line renderer (lightBeamRenderer)
                lightBeamRenderer.SetPosition(1, new Vector3(OpticalElementHit.distance, 0, 0));
                lightBeamRenderer.SetPosition(2, calculateDirection());

            }
            else
            {
                lightBeamRenderer.SetPosition(1, new Vector3(1000, 0, 0));
            }
        }

        else if (isReflective)
        {
            if (Physics.Raycast(transform.position, transform.right, out OpticalElementHit))
            {
                lightBeamRenderer.SetPosition(1, new Vector3(OpticalElementHit.distance, 0, 0));
                lightBeamRenderer.SetPosition(2, calculateReflectedDirection());
            }
            else
            {
                lightBeamRenderer.SetPosition(1, new Vector3(1000, 0, 0));
            }
        }
    }

    //Calculates the direction taken by the refracted beam of light
    //Refer to diagram in Readme folder for explanation on variable nomenclature


    public Vector3 calculateDirection()
    {
        Vector3 RefractedRayDirection;
        Vector3 traj_in = OpticalElementHit.point - transform.position; //Vector from light origin to point hit


        if (isCurved)
        {

            float angle_alpha = -AngleBetween(traj_in, Vector3.right);
            float angle_a = -AngleBetween(traj_in, -OpticalElementHit.normal);
            float angle_phi = AngleBetween(-OpticalElementHit.normal, Vector3.right);
            float angle_b = Mathf.Asin(1 / n_Index * Mathf.Sin(Deg2Rad(angle_a)));
            float angle_beta = angle_phi - angle_b;


            //Multiply by scalingFactor because line renderers are not true vectors. They only draw a line between two points. Cos(beta) and Sin(beta) return very small numbers, 
            //So if left unaltered, the resulting line would be very short.
            RefractedRayDirection = scalingFactor * new Vector3(Mathf.Cos(Deg2Rad(angle_beta)), -Mathf.Sin(Deg2Rad(angle_beta)));

        }

        else
        {
            float angle_a = -AngleBetween(traj_in, -OpticalElementHit.normal);
            float angle_beta = Mathf.Asin(1 / n_Index * Mathf.Sin(Deg2Rad(angle_a)));
            RefractedRayDirection = scalingFactor * new Vector3(Mathf.Cos(angle_beta), -Mathf.Sin(angle_beta));
        }
        return RefractedRayDirection;

    }
    
    public Vector3 calculateReflectedDirection()
    {
        Vector3 ReflectedRayDirection = new Vector3(0, 0, 0);
        Vector3 traj_in = OpticalElementHit.point - transform.position; //Vector from light origin to point hit
        float angle_incident = AngleBetween(traj_in, -OpticalElementHit.normal);
        float angle_reflected = 2*angle_incident;
        ReflectedRayDirection = scalingFactor * new Vector3(-Mathf.Cos(Deg2Rad(angle_reflected)), -Mathf.Sin(Deg2Rad(angle_reflected)));
        return ReflectedRayDirection;
    }

    public float Deg2Rad(float d)
    {
        return d * Mathf.Deg2Rad;
    }

    // Finds the angle between two vectors. Determines the sign via cross product.
    // If Vector3 From is above Vector3 To, the angle is positive and vice versa
    public float AngleBetween(Vector3 From, Vector3 To)
    {
        float angle = Vector3.Angle(From, To);
        if (Vector3.Cross(From, To).z < 0)
        {
            angle *= -1;
        }


        return angle;
    }

    //Creates various axes and lines that are useful for debugging. I used these more to make sure that my math made sense.
    public void DebugLines(bool isEnabled)
    {

        if (isEnabled)
        {
            //x-axis at hit point
            Debug.DrawRay(OpticalElementHit.point, 1000 * Vector3.right, Color.grey);
            Debug.DrawRay(OpticalElementHit.point, 1000 * Vector3.left, Color.grey);

            //normal line to hit point (passes through center of curve)
            Debug.DrawRay(OpticalElementHit.point, 1000 * OpticalElementHit.normal, Color.black);
            Debug.DrawRay(OpticalElementHit.point, 1000 * -OpticalElementHit.normal, Color.black);

            //x-axis at gameobject position
            Debug.DrawRay(transform.position, 1000 * Vector3.right, Color.gray);
            Debug.DrawRay(transform.position, 1000 * Vector3.left, Color.gray);
        }
    }
    
}
