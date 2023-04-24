using System.Collections;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    private GameObject obj1;
    [SerializeField] private GameObject obj2;
    [SerializeField] private GameObject obj3;
    [SerializeField] private float period;
    [SerializeField] private ParticleSystem ps;
    private MeshRenderer mesh1;
    private MeshRenderer mesh2;
    private MeshRenderer mesh3;
    private Collider coll;

    private void Start()
    {
        obj1 = gameObject;
        coll = obj1.GetComponent<Collider>();
        mesh1 = obj1.GetComponent<MeshRenderer>();
        mesh2 = obj2.GetComponent<MeshRenderer>();
        mesh3 = obj3.GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            StartCoroutine(Cor1());
            StartCoroutine(Cor2());
            StartCoroutine(Cor3());
            StartCoroutine(Cor4());
        }
    }

    private IEnumerator Cor1()
    {
        yield return new WaitForSeconds(period);
        mesh1.enabled = false;
        mesh2.enabled = true;
    }
    private IEnumerator Cor2()
    {
        yield return new WaitForSeconds(period * 2f);
        mesh2.enabled = false;
        mesh3.enabled = true;
    }
    private IEnumerator Cor3()
    {
        yield return new WaitForSeconds(period * 3f);
        mesh3.enabled = false;
        coll.enabled = false;
        ps.Play();
    }

    private IEnumerator Cor4()
    {
        yield return new WaitForSeconds(period * 6f);
        mesh1.enabled = true;
        coll.enabled = true;
    }
}
