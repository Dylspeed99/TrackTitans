using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float speed = 10.0f;
    private Rigidbody playerRb;
    private float leftBound = -9.6f;
    private float rightBound = 9.6f;
    private float target;
    public bool gameOver = false;
    public float force = 0.0f;
    private float multiplier = 0.0f;
    public GameObject cam;
    private float smallestGap = 5.0f;
    private SpawnManager spawnManagerScript;
    private AudioSource playerAudio;
    public AudioClip hitBarrier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        playerAudio = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnManagerScript.isGameActive)
        {
            //checks for A and left arrow
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && transform.position.x > leftBound)
            {
                target = transform.position.x - 4.8f;
                MovePlayer(target);
            }
            //checks for D and right arrow
            else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && transform.position.x < rightBound)
            {
                target = transform.position.x + 4.8f;
                MovePlayer(target);
            }
            if(transform.position.z - cam.transform.position.z < smallestGap)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z + smallestGap);
            }
        }
        else
        {
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        }
    }
    
    void MovePlayer(float newPos)
    {
        //moves player left
        if (newPos < transform.position.x)
        {
            while (transform.position.x > newPos)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            transform.position = new Vector3(target, transform.position.y, transform.position.z);
        }
        //moves player right
        else if (newPos > transform.position.x)
        {
            while (transform.position.x < target)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            transform.position = new Vector3(target, transform.position.y, transform.position.z);
        }
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
            playerRb.AddForce(Vector3.forward * multiplier, ForceMode.Impulse);
            force += multiplier;
            
        }
        
        //applies backwards force if obstacle
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            playerAudio.PlayOneShot(hitBarrier, 1);
            if (force > 0)
            {
                multiplier = force * 1 + 0.75f;
            }
            else
            {
                multiplier = 0.75f;
            }
            playerRb.AddForce(Vector3.back * multiplier, ForceMode.Impulse);
            force -= multiplier;
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Finish"))
        {
            spawnManagerScript.GameOver();
        }
        
    }
        
}
