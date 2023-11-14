using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    public float during = 10;
    private float elapsedTime = 0f; // The elapsed time
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        // Check if the timer has reached the desired duration
        if (elapsedTime >= during)
        {
            // Timer has finished, do something
            Destroy(gameObject);
        }
    }
}
