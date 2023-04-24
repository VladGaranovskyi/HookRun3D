using UnityEngine;

public class DestructibleCube : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private Material[] mats;
    private float hpm;
    private MeshRenderer mesh;
    [SerializeField] private float[] hps;
    [SerializeField] private GameObject ps;
    private Rigidbody rb;
    private bool IsEnter;
    [SerializeField] private AudioSource sound;
    private AudioPlayer ap;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        hpm = hp;
        ap = FindObjectOfType<AudioPlayer>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("player"))
        {
            IsEnter = true;
            if (PlayerPrefs.HasKey("Sound"))
            {
                if (PlayerPrefs.GetString("Sound") == "On")
                {
                    ap.PlayAudio(sound, sound.clip);
                }
                else if (PlayerPrefs.GetString("Sound") == "Vib")
                {
                    Handheld.Vibrate();
                }
            }
            else
            {
                ap.PlayAudio(sound, sound.clip);
            }
            if (hpm > Mathf.Abs(rb.velocity.z))
            {
                hpm -= Mathf.Abs(rb.velocity.z);
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -rb.velocity.z);
            }
            else
            {
                GameObject g = Instantiate(ps);
                g.transform.position = this.transform.position;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("player")) IsEnter = false;       
    }

    private void FixedUpdate()
    {
        if (rb == null) rb = GameObject.FindWithTag("player").GetComponent<Rigidbody>();
        if (IsEnter)
        {
            Debug.Log(hpm);
            if (hpm < hps[0])
            {
                if (hpm > hps[1]) mesh.material = mats[0];
                else if(hpm < hps[1] && hpm > hps[2]) mesh.material = mats[1];
                else if (hpm < hps[2] && hpm > hps[3]) mesh.material = mats[2];
                else if(hpm < hps[3])
                {
                    GameObject g = Instantiate(ps);
                    g.transform.position = this.transform.position;
                    Destroy(gameObject);
                }
            }
        }
    }
}
