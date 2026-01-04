using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    public GameObject cam;
    private float repeatHeight;
    private float initialGap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        repeatHeight = GetComponent<BoxCollider>().size.z*5;
        initialGap = transform.position.z - cam.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(startPos.z - cam.transform.position.z != initialGap)
        {
            startPos.z = cam.transform.position.z + initialGap;
        }
        
        if (transform.position.z < startPos.z - repeatHeight)
        {
            transform.position = startPos;
        }
    }
}
