using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    public float force = 0.0f;
    private float multiplier = 0.0f;
    const float leftBound = -9.6f;
    const float rightBound = 9.6f;
    [SerializeField] float speed = 10.0f;
    public bool canMove = true;
    public GameObject cam;
    private float smallestGap = 5.0f;
    private Vector3 initPos;
    private SpawnManager spawnManagerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StopAllCoroutines();
        enemyRb = GetComponent<Rigidbody>();
        initPos = transform.position;
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnManagerScript.isGameActive)
        {
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            for(int i=0; i<obstacles.Length; i++)
            {
                float xdiff = obstacles[i].transform.position.x - transform.position.x;
                float zdiff = obstacles[i].transform.position.z - transform.position.z;
                if(xdiff == 0 && zdiff < 10)
                {
                    if (canMove)
                    {
                        MoveEnemy();
                        canMove = false;
                        StartCoroutine(MoveCountdown());
                    }
                    
                }
            }
            if(transform.position.z - cam.transform.position.z < smallestGap)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z + smallestGap);
            }
        }
        else
        {
            transform.position = new Vector3(initPos.x, transform.position.y, transform.position.z);
            StopAllCoroutines();
        }
        
    }
    void MoveEnemy()
    {
        if(transform.position.x == rightBound)
        {
            Move(false);
        }
        else if(transform.position.x == leftBound)
        {
            Move(true);
        }
        else
        {
            if(Random.Range(0,2) == 0)
            {
                Move(false);
            }
            else
            {
                Move(true);
            }
        }
    }
    void Move(bool directionRight)
    {
        float newPos = 0.0f;
        if (directionRight == true)
        {
            newPos = transform.position.x + 4.8f;
            while(transform.position.x < newPos)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
        }
        else if(directionRight == false)
        {
            newPos = transform.position.x - 4.8f;
            while(transform.position.x > newPos)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
        }
    }

    IEnumerator MoveCountdown()
    {
        yield return new WaitForSeconds(1);
        canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //applies forward force if powerup
        if (other.gameObject.CompareTag("Powerup"))
        {
            if (force < 0)
            {
                multiplier = force * -1 + 0.5f;
            }
            else
            {
                multiplier = 0.5f;
            }
            enemyRb.AddForce(Vector3.forward * multiplier, ForceMode.Impulse);
            force += multiplier;
            
        }
        
        //applies backwards force if obstacle
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            if (force > 0)
            {
                multiplier = force * 1 + 0.75f;
            }
            else
            {
                multiplier = 0.75f;
            }
            enemyRb.AddForce(Vector3.back * multiplier, ForceMode.Impulse);
            force -= multiplier;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            spawnManagerScript.GameOver();
        }
        
    }
}
