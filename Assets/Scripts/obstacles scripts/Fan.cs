using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    private bool IsIn;
    [SerializeField] private float speed = 2200f;
    private Rigidbody rb;
    private float appendSpeed;
    [SerializeField] private float appendAppender = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            IsIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "player")
        {
            IsIn = false;
            appendSpeed = 0f;
        }
    }

    void FixedUpdate()
    {
        if(rb == null)
        {
            rb = GameObject.FindWithTag("player").GetComponent<Rigidbody>();
        }
        if (IsIn)
        {
            rb.AddForce(0f, 0f, -((speed * 7f * Time.deltaTime) + appendSpeed));
            appendSpeed += appendAppender;
        }
    }
}
