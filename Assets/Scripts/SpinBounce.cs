using UnityEngine;

public class SpinBounce : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float spinSpeed = 100.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
