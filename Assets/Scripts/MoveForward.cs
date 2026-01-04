using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private SpawnManager spawnManagerScript;
    [SerializeField] private float speed = 15.0f;
    private float lowerBound = -30;
    private GameObject cam;
    private float initialGap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        initialGap = lowerBound - cam.transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (spawnManagerScript.isGameActive)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed, Space.World);

        }
        else
        {
            if(gameObject.tag == "Obstacle" || gameObject.tag == "Powerup")
            {
                Destroy(gameObject);
            }
        }
        
        if (transform.position.z < lowerBound)
        {
            if(gameObject.tag != "ground")
            {
                Destroy(gameObject);
            }
            
        }
        if(lowerBound - cam.transform.position.z != initialGap)
        {
            lowerBound = cam.transform.position.z + initialGap;
        }
        
    }
}
