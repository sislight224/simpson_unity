using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRandomMove : MonoBehaviour
{
    public float moveSpeed = 2f; // The speed at which to move and rotate
    public float rotateSpeed = 2f; // The speed at which to move and rotate
    private float maxRadius = 5;
    private float curSpeed = 0;

    private Quaternion targetRotation;
    private Vector3 targetPos;

    public float duration = 5f; // The duration of the timer in seconds
    private float elapsedTime = 0f; // The elapsed time

    private Rigidbody rb;

    private void Start()
    {
        targetPos = transform.position;
        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        duration = Random.RandomRange(7, 15);
    }

    void Update()
    {
        // Smoothly move the object to the target position
        //transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

        Vector3 moveDirection = -transform.forward * curSpeed;
        rb.MovePosition(rb.position + moveDirection * Time.deltaTime);

        // Smoothly rotate the object to the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);


        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        // Check if the timer has reached the desired duration
        if (elapsedTime >= duration)
        {
            // Timer has finished, do something
            SetTarget();

            if(Random.RandomRange(0, 10) < 3)
            {
                curSpeed = moveSpeed;
                StartCoroutine(stopNPC());
            }
        }

    }

    IEnumerator stopNPC()
    {
        Debug.Log("Coroutine started");

        yield return new WaitForSeconds(1f);

        curSpeed = 0;
        //isPlayingPrompt = true;
    }

    void SetTarget()
    {
        float radius = Mathf.FloorToInt(Random.RandomRange(1, maxRadius));
        float radian = Mathf.Deg2Rad * Mathf.Floor(Random.RandomRange(0, 360));

        float x = radius * Mathf.Cos(radian);
        float y = radius * Mathf.Sin(radian);

        targetPos += new Vector3(x, 0, y);
        targetRotation.y = radian;

        elapsedTime = 0;
    }
}
