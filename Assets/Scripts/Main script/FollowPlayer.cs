using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    private Rigidbody rb;
    private Camera camera;
    [SerializeField] private bool IsSmooth;
    public Vector3 offset;
    private ParticleSystem warp;
    private float currentVeclocity;

    void Awake()
    {
        if(this.gameObject == GameObject.Find("Main Camera"))
        {
            this.transform.rotation = new Quaternion(0f, 0f, 0f, 1);
            camera = this.gameObject.GetComponent<Camera>();
            warp = transform.GetChild(0).GetComponent<ParticleSystem>();
        }
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.Find("Pivot");
            this.transform.position = player.transform.position + offset;
        }
        if (rb == null)
        {
            rb = GameObject.FindWithTag("player").GetComponent<Rigidbody>();
        }
        if (IsSmooth)
        {
            this.transform.SetParent(player.transform);
        }
        else
        {
            transform.position = player.transform.position + offset;
        }
        if (camera != null)
        {
            currentVeclocity = Mathf.Pow(rb.velocity.z, 2) + Mathf.Pow(rb.velocity.x, 2);
            camera.fieldOfView = 60f + (currentVeclocity >= 777f ? 7.77f : currentVeclocity / 100f);
            if (currentVeclocity >= 777f) warp.startSpeed = 10f;
            else warp.startSpeed = 5f;
        }
    }
}
