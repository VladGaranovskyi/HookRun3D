using UnityEngine;

public class Speed : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private AudioSource sound;
    private AudioPlayer ap;

    private void Start()
    {
        ap = FindObjectOfType<AudioPlayer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
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
        }
    }
}
