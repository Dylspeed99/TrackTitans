using UnityEngine;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour
{
    float firstTrain = -13.85f;
    public GameObject[] trains;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        firstTrain = 0.0f;
        for(int i = 0; i<trains.Length; i++)
        {
            if(trains[i].transform.position.z > firstTrain)
            {
                firstTrain = trains[i].transform.position.z;
            }
        }
        if(firstTrain - transform.position.z > 30)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, firstTrain - 30);
        }
        
    }
}
