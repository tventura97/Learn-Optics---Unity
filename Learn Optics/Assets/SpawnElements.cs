using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnElements : MonoBehaviour {


    public GameObject ConcaveLens, ConvexLens, ConcaveMirror, ConvexMirror, OpticalElement;
    public Vector3 InitialScale;
    private GameObject[] OpticalElements;
    private GameObject Root;

    private void Start()
    {
        Root = GameObject.Find("Root");
        OpticalElement = GameObject.FindGameObjectWithTag("OpticalElement");
        InitialScale = OpticalElement.transform.localScale;
    }


    //Whenever something is spawned, save its initial scale and height so the sliders can access it.
    public void SpawnConcaveLens()
    {
        OpticalElements = GameObject.FindGameObjectsWithTag("OpticalElement");
        for (int i = 0; i < OpticalElements.Length; i++)
        {
            Destroy(OpticalElements[i]);
        }

        OpticalElement = Instantiate(ConcaveLens, Root.transform.position, Quaternion.Euler(new Vector3(0, 90, 0)));
        InitialScale = OpticalElement.transform.localScale;
        print(InitialScale);

    }
    public void SpawnConvexLens()
    {
        OpticalElements = GameObject.FindGameObjectsWithTag("OpticalElement");
        for (int i = 0; i < OpticalElements.Length; i++)
        {
            Destroy(OpticalElements[i]);
        }

        OpticalElement = Instantiate(ConvexLens, Root.transform.position, Quaternion.Euler(new Vector3(0, 90, 90)));
        InitialScale = OpticalElement.transform.localScale;
        print(InitialScale);

    }
    public void SpawnConcaveMirror()
    {
        OpticalElements = GameObject.FindGameObjectsWithTag("OpticalElement");
        for (int i = 0; i < OpticalElements.Length; i++)
        {
            Destroy(OpticalElements[i]);
        }

        OpticalElement = Instantiate(ConcaveMirror, Root.transform.position, Quaternion.Euler(new Vector3(0, 90, 90)));
        InitialScale = OpticalElement.transform.localScale;
        print(InitialScale);


    }
    public void SpawnConvexMirror()
    {
        OpticalElements = GameObject.FindGameObjectsWithTag("OpticalElement");
        for (int i = 0; i < OpticalElements.Length; i++)
        {
            Destroy(OpticalElements[i]);
        }

        OpticalElement = Instantiate(ConvexMirror, Root.transform.position, Quaternion.Euler(new Vector3(0, -90, 90)));
        InitialScale = OpticalElement.transform.localScale;
        print(InitialScale);


    }
}
