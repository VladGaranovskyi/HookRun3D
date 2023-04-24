using System.Collections;
using UnityEngine;

public class EnemyBonusLevel : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider[] bones;
    [SerializeField] private Transform mesh;
    [HideInInspector] public bool IsGrabbed;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == 4)
        {
            EnableRagdoll(1f);
        }
    }
    private void FixedUpdate()
    {
        if (IsGrabbed)
        {
            EnableRagdoll(5f);
        }
        else
        {
            rb.velocity = Vector3.forward * -177f * Time.deltaTime;
        }
    }

    private void EnableRagdoll(float t)
    {
        anim.enabled = false;
        foreach (Collider bone in bones) bone.isTrigger = false;
        GetComponent<Collider>().isTrigger = true;
        mesh.parent = bones[0].transform;
        StartCoroutine(DeSpawn(t));
    }

    public Transform GetPelvis()
    {
        return bones[0].transform;
    }

    private IEnumerator DeSpawn(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}
