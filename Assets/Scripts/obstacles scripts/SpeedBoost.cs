using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] private float metersPath;
    [SerializeField] private float multiplier;
    [SerializeField] private GameObject[] children;
    private GrappleHuman _grapple;
    private Transform player;
    private float zPos;
    private bool IsEnter;

    private void Start()
    {
        zPos = transform.position.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            IsEnter = true; 
            foreach(GameObject g in children) g.SetActive(false);
            player = other.transform;
            _grapple = player.GetComponent<GrappleHuman>();
            _grapple.MultiplySpeed(multiplier);
        }
        //else if (other.CompareTag("Death")) Destroy(other.gameObject);
    }

    private void FixedUpdate()
    {
        //if (IsEnter) transform.position = player.position + offset;
        if (IsEnter)
        {
            if (Mathf.Abs(player.transform.position.z - zPos) > metersPath)
            {
                _grapple.MultiplySpeed(1 / multiplier); Destroy(gameObject);
            }
        }
        else transform.Rotate(0f, 1f, 0f);
    }
}
