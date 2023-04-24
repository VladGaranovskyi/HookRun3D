using UnityEngine;

public class ShieldBooster : MonoBehaviour
{
    [SerializeField] private float metersPath;
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject collMain;
    private Transform player;
    private float zPos;
    private bool IsEnter;

    private void Start()
    {
        zPos = transform.position.z;
    } 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player")) {
            IsEnter = true; transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        }
        else if (other.CompareTag("Death")) Destroy(other.gameObject);
    }

    private void FixedUpdate()
    {
        if (player == null) player = GameObject.FindWithTag("player").transform;
        if (IsEnter) transform.position = player.position + offset;
        //else transform.Rotate(0f, 1f, 0f);
        if (Mathf.Abs(transform.position.z - zPos) > metersPath) Destroy(gameObject);
    }
}
