using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] destroyObjs;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private AudioSource sound;
    private AudioPlayer ap;

    private void Start()
    {
        ap = FindObjectOfType<AudioPlayer>();
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("player"))
        {
            FindObjectOfType<GameManager>().SetCheckPoint(transform.position);
            foreach(GameObject g in destroyObjs)
            {
                Destroy(g);
            }
            ps.Play();
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
