using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public GameObject powerup;
    public GameObject obstacle;
    public GameObject finish;
    public GameObject cam;
    public bool isGameActive = false;
    private float initialGap;
    [SerializeField] float zSpawn = 40;
    public GameObject[] trains;
    public GameObject titleScreen;
    public GameObject endScreen;
    public TextMeshProUGUI finishOrder;
    public TextMeshProUGUI distanceLeft;
    public Button pause;
    private float initDistance;
    private string[] endings = new string[5];
    [SerializeField] float[] lanes = {-9.6f, -4.8f, 0f, 4.8f, 9.6f};
    public AudioSource spawnAudio;
    public AudioSource titleMusic;
    public AudioClip bgMusic;
    public AudioClip startWhistle;
    public AudioClip endingSound;
    private bool isPaused = false;
    public GameObject pauseScreen;
    private TextMeshProUGUI pauseText;

    public GameObject rulesScreen;
    private bool printedOrder;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        titleMusic.Play();
        StopAllCoroutines();
        isGameActive = false;
        Time.timeScale = 1f;
        initDistance = 1000;
        initialGap = zSpawn - cam.transform.position.z;
        endings[0] = "st";
        endings[1] = "nd";
        endings[2] = "rd";
        endings[3] = "th";
        endings[4] = "th";
        //spawnAudio = GetComponent<AudioSource>();
    }
    public void StartGame()
    {
        titleMusic.Stop();
        isGameActive = true;
        for(int i = 0; i<lanes.Length; i++)
        {
            StartCoroutine(SpawnLoop(i));
        }
        StartCoroutine(FinishLine());
        titleScreen.gameObject.SetActive(false);
        distanceLeft.gameObject.SetActive(true);
        pause.gameObject.SetActive(true);
        spawnAudio.PlayOneShot(startWhistle, 1);
        spawnAudio.Play();
        pauseText = pause.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(zSpawn - cam.transform.position.z != initialGap)
        {
            zSpawn = cam.transform.position.z + initialGap;
        }
        if (isGameActive)
        {
            initDistance -= 1 * Time.deltaTime * 1000/60;
            if (initDistance < 0)
            {
                initDistance = 0;
            }
            distanceLeft.text = (int)initDistance + "m";
        }

    }

    IEnumerator SpawnLoop(int laneIndex)
    {
        while (isGameActive)
        {
            float wait = Random.Range(1.0f, 5.0f);
            yield return new WaitForSeconds(wait);
            Spawn(laneIndex);
            
        }
        
    }

    private void Spawn(int laneIndex)
    {
        Vector3 pos = new Vector3(lanes[laneIndex], 0.0f, zSpawn);
        if(Random.Range(0,2) == 0)
        {
            pos.y = powerup.transform.position.y;
            Instantiate(powerup, pos, powerup.transform.rotation);
        }
        else
        {
            Instantiate(obstacle, pos, obstacle.transform.rotation);
        }
    }


    IEnumerator FinishLine()
    {
        yield return new WaitForSeconds(60);
        Vector3 pos = new Vector3(finish.transform.position.x, finish.transform.position.y, zSpawn);
        Instantiate(finish, pos, finish.transform.rotation);
    }
    public void GameOver()
    {
        pause.gameObject.SetActive(false);
        spawnAudio.Stop();
        spawnAudio.PlayOneShot(endingSound, 1);
        isGameActive = false;
        StopAllCoroutines();
        Time.timeScale = 0f;
        if (!printedOrder)
        {
            for(int i = 0; i < trains.Length - 1; i++)
            {
                for(int j = 0; j < trains.Length - 1; j++)
                {
                    float z1 = trains[j].transform.position.z;
                    float z2 = trains[j+1].transform.position.z;
                    if (z1 < z2)
                    {
                        GameObject temp = trains[j];
                        trains[j] = trains[j+1];
                        trains[j+1] = temp;
                    }
                }
            }
            for(int i = 0; i<trains.Length; i++)
            {
                finishOrder.text += $"{i+1}{endings[i]} - {trains[i].name} \n";
                endScreen.gameObject.SetActive(true);
            }
            printedOrder = true;
        }
    }

    public void ResetGame()
    {
        StopAllCoroutines();
        isGameActive = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
            pauseScreen.gameObject.SetActive(false);
            spawnAudio.Play();
            pauseText.text = "| |";
        }
        else
        {
            Time.timeScale = 0f;
            isPaused = true;
            pauseScreen.gameObject.SetActive(true);
            spawnAudio.Pause();
            pauseText.text = "►";
        }
    }

    public void showRules()
    {
        titleScreen.gameObject.SetActive(false);
        rulesScreen.gameObject.SetActive(true);
    }

    public void closeRules()
    {
        titleScreen.gameObject.SetActive(true);
        rulesScreen.gameObject.SetActive(false);
    }


}
