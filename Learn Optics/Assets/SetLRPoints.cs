using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLRPoints : MonoBehaviour
{

    LineRenderer lineRenderer;

    public void InitializeLR()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.2F;
        lineRenderer.endWidth = 0.2F;

    }

    public void SetLineRendPoints(int index, Vector3 point)
    {
        lineRenderer.SetPosition(index, point);
    }

    public void SetNumLRPoints(int points)
    {
        lineRenderer.positionCount = points;
    }

    public void SetVisible(bool isVisible)
    {
        if (isVisible)
        {
            lineRenderer.startWidth = 0.2F;
            lineRenderer.endWidth = 0.2F;
        }
        else if (!isVisible)
        {
            lineRenderer.startWidth = 0;
            lineRenderer.endWidth = 0;
        }
    }

    public void SetEnabled(bool isEnabled)
    {
        lineRenderer.enabled = isEnabled;
    }

    public void ResetPoints(Vector3 point)
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, point);
        }
    }
}
