using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLRPoints : MonoBehaviour {

    LineRenderer lineRenderer;

    public void InitializeLR()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.1F;
        lineRenderer.endWidth = 0.1F;
        lineRenderer.positionCount = 2;
    }

    public void SetLineRendPoints(int index, Vector3 point)
    {
        lineRenderer.SetPosition(index, point);
    }

    public void SetNumLRPoints(int points)
    {
        lineRenderer.positionCount = points;
    }
}
