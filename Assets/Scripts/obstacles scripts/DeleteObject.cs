using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    private GameObject player;
    public Vector3 jumpDirection;
    private AudioPlayer ap;
    public AudioSource source;
    private float Sensitivity;
    private Rigidbody rb;

    void Awake()
    {
        ap = FindObjectOfType<AudioPlayer>();
    }

    void OnTriggerEnter(Collider Object)
    {
        if(Object.gameObject == GameObject.FindWithTag("player"))
        {
            float r = 0.1f;
            PlayerPrefs.SetFloat("Coins_Game", 0);
            FindObjectOfType<GameManager>().EndGame(r);
        }
        else
        {
            Destroy(Object.gameObject);
            rb = GameObject.FindWithTag("player").GetComponent<Rigidbody>();
            rb.AddForce(jumpDirection.x * Time.deltaTime, jumpDirection.y * Time.deltaTime, jumpDirection.z * Time.deltaTime, ForceMode.Impulse);
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
}
