using UnityEngine;

public class Boost : MonoBehaviour
{
    public Vector3 jumpDirection;
    private AudioPlayer ap;
    public AudioSource source;
    private GrappleHuman _grap;

    void Awake()
    {
        ap = FindObjectOfType<AudioPlayer>();
    }

    void OnTriggerEnter(Collider Object)
    {
        Rigidbody rb = Object.gameObject.GetComponent<Rigidbody>();
        if (Object.CompareTag("player"))
        {
            _grap = Object.GetComponent<GrappleHuman>();
        }
        if((_grap != null && !_grap.GetHooking()) || _grap == null)
        {
            rb.AddForce(jumpDirection, ForceMode.Impulse);
        }
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "On")
            {
                ap.PlayAudio(source, source.clip);
            }
            else if (PlayerPrefs.GetString("Sound") == "Vib")
            {
                Handheld.Vibrate();
            }
        }
        else
        {
            ap.PlayAudio(source, source.clip);
        }
    }
}
