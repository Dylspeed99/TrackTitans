using UnityEngine;
using UnityEngine.UI;
public class ButtonListener : MonoBehaviour
{
    private Button startButton;
    private SpawnManager spawnManagerScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StartGame()
    {
        if(gameObject.name == "Start Button")
        {
            spawnManagerScript.StartGame();
        }
        else if(gameObject.name == "Reset")
        {
            spawnManagerScript.ResetGame();
        }
    }
}
