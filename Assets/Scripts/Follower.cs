using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform follower;
    private Transform player;
    private PlayerCollision pc;
    [SerializeField] private bool IsPlayer;
    private Transform cam;
    private Vector3 prePos = new Vector3(0f, 0f, 0f);
    private Rigidbody rb;
    private float angle;
    private float currentSpeed;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    void FixedUpdate()
    {
        if(cam == null) cam = Camera.main.transform;
        if (IsPlayer && player == null) player = GameObject.FindWithTag("player").transform; follower = player;
        if (IsPlayer && pc == null) pc = player.GetComponent<PlayerCollision>();
        if (IsPlayer && rb == null) rb = player.GetComponent<Rigidbody>();
        transform.position = follower.position;
        if (IsPlayer)
        {
            Vector3 direction = new Vector3(player.transform.position.x - prePos.x, 0f, player.transform.position.z - prePos.z);
            angle = GetAngle(cam.transform.forward, new Vector3(direction.x, 0f, direction.z)
                , Vector3.up);
            currentSpeed = Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2);
            transform.eulerAngles = new Vector3(0f, Mathf.Lerp(transform.eulerAngles.y,
                transform.eulerAngles.y + angle, Time.deltaTime * 
                (currentSpeed <= 122.5f ? currentSpeed / 70f : 2f)), 0f);
            prePos = new Vector3(player.transform.position.x, 0f, player.transform.position.z);
        }
    }

    private float GetAngle(Vector3 v1, Vector3 v2, Vector3 axis)
    {
        v1 -= Vector3.Project(v1, axis);
        v2 -= Vector3.Project(v2, axis);
        float angle = Vector3.Angle(v1, v2);
        return angle * (Vector3.Dot(axis, Vector3.Cross(v1, v2)) < 0 ? -1 : 1);
    }
}
