using UnityEngine;

public class EnergySphere : MonoBehaviour
{
    private Collider col;
    private bool IsTrigger;
    [SerializeField] private Vector3 BoostDirection;
    [SerializeField] private float height;
    [SerializeField] private Vector3 force;
    private Rigidbody rb;
    [SerializeField] private AudioSource sound;

    void OnTriggerEnter(Collider obj)
    {
        if(obj.GetComponent<Rigidbody>() != null)
        {
            col = obj;
            sound.Play();
            IsTrigger = true;
            rb = col.gameObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if(col == obj)
        {
            IsTrigger = false;
            sound.Stop();
            rb = null;
        }
    }

    void FixedUpdate()
    {
        if (IsTrigger)
        {
            if(col.transform.position.y < height)
            {
                rb.AddForce(BoostDirection * Time.deltaTime);
            }
            else
            {
                rb.AddForce(force);
                IsTrigger = false;
            }
        }
    }
}
