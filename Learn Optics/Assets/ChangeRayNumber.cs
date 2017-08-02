using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRayNumber : MonoBehaviour
{

    public GameObject LightEmitter;
    private GameObject InitialPlayer, LastRayAdded;
    private float[] Positions;
    private float sign;

    private void Start()
    {
        InitialPlayer = GameObject.Find("Player");
        Positions = new float[] { 2.5F, -2.5F, 5F, -5F };
    }

    public void AddRays()
    {
        if (GameObject.FindGameObjectsWithTag("LightEmitter").Length <= 5)
        {
            LastRayAdded = Instantiate(LightEmitter, new Vector3(InitialPlayer.transform.position.x, InitialPlayer.transform.position.y +  Positions[GameObject.FindGameObjectsWithTag("LightEmitter").Length - 1]), Quaternion.identity, InitialPlayer.transform);
            LastRayAdded.GetComponent<Player_Controls>().enabled = false;

        }
    }

    public void RemoveRays()
    {

        if (GameObject.FindGameObjectsWithTag("LightEmitter").Length > 1)
        {
            GameObject[] AddedRays = GameObject.FindGameObjectsWithTag("LightEmitter");
            Destroy(AddedRays[AddedRays.Length - 1]);
        }
    }


}
