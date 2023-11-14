using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRandomMove : MonoBehaviour
{
    public Vector3 startPos;
    public Quaternion startRot;
    public float speed = 5f;
    private Rigidbody rb;
    float during = 15;
    float curTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        Vector3 moveDirection = transform.forward * speed;
        rb.MovePosition(rb.position + moveDirection * Time.deltaTime);
        if (curTime > during)
        {
            transform.position = startPos;
            transform.rotation = startRot;
            curTime = 0;
        }
    }
}
