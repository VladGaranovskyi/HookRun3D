using UnityEngine;

public class Boss : MonoBehaviour
{
    private RifleShot rs;
    private bool IsEnter;
    [SerializeField] private ParticleSystem blood;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private Collider[] ragdoll;
    [SerializeField] private Animator anim;
    [SerializeField] private float k;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Floor") && other.GetComponent<Boss>() == null && IsEnter)
        {
            if(other.GetComponent<Rigidbody>().mass == 1.07f)
            {
                rs.SubHP();
                foreach(Collider c in ragdoll)
                {
                    c.isTrigger = false;
                }
                ragdoll[0].GetComponent<Rigidbody>().AddForce((ragdoll[0].transform.position - other.transform.position) * k, ForceMode.Impulse);
                anim.enabled = false;
                explosion.transform.parent = null;
                explosion.transform.position = other.transform.position;
                explosion.Play();
            }
            else
            {
                rs.SubHP();
                blood.transform.parent = null;
                blood.transform.position = other.transform.position;
                blood.Play();
            }
        }  
    }

    public void SetRS(RifleShot rifle)
    {
        rs = rifle;
        IsEnter = true;
    }
}
