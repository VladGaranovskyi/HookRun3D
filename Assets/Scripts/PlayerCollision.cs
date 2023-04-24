using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gm;
    private AudioSource sound;
    private AudioPlayer ap;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        sound = GameObject.Find("Collide_Sound").GetComponent<AudioSource>();
        ap = FindObjectOfType<AudioPlayer>();
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (gm.GetGrounds() == 0 || !collisionInfo.collider.CompareTag("Untagged"))
        {
            sound.transform.position = transform.position;
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
        if (collisionInfo.collider.CompareTag("Floor"))
        {
            gm.ChangeGrounds(1);
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.collider.CompareTag("Floor"))
        {
            gm.ChangeGrounds(-1);
        }
    }

    public bool IsGrounded()
    {
        return gm.GetGrounds() == 0 ? false : true;
    }
}
