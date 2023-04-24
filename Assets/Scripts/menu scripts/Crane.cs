using System.Collections;
using UnityEngine;

public class Crane : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 force;
    [SerializeField] private Vector2 Pos;
    [SerializeField] private float per;
    private bool IsCor;

    private void Start()
    {
        rb.AddForce(100f, 0f, 0f, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (transform.position.y >= Pos.y && !IsCor) StartCoroutine(ForceCor(transform.position.x > Pos.x ? -1f : 1f));
        else IsCor = false;
    }

    private IEnumerator ForceCor(float m)
    {
        IsCor = true;
        yield return new WaitForSeconds(per);
        rb.velocity = Vector3.zero;
        rb.AddForce(force.x * m, force.y, force.z, ForceMode.Impulse);
    }
}
