using UnityEngine;

public class RevertESphere : MonoBehaviour
{
    private bool IsTrigger;
    public float k;
    public AudioSource source;
    private Transform player;
    private Rigidbody rb;

    void OnTriggerEnter(Collider obj)
    {
        if(obj.gameObject == GameObject.FindWithTag("player"))
        {
            IsTrigger = true;
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject == GameObject.FindWithTag("player"))
        {
            IsTrigger = false;
        }
    }

    void FixedUpdate()
    {
        if(rb == null)
        {
            rb = GameObject.FindWithTag("player").GetComponent<Rigidbody>();
        }
        if(player == null)
        {
            player = GameObject.FindWithTag("player").transform;
        }
        if (IsTrigger)
        {
            rb.AddForce((-(this.transform.position.x) + player.position.x) * k * Time.deltaTime,
                (-(this.transform.position.y) + player.position.y) * k * Time.deltaTime,
                (-(this.transform.position.z) + player.position.z) * k * Time.deltaTime);
        }

        if(Vector3.Distance(this.transform.position, player.position) <= 1.8f)
        {
            if (PlayerPrefs.HasKey("Sound"))
            {
                if (PlayerPrefs.GetString("Sound") == "On")
                {
                    FindObjectOfType<AudioPlayer>().PlayAudio(source, source.clip);
                }
                else if (PlayerPrefs.GetString("Sound") == "Vib")
                {
                    Handheld.Vibrate();
                }
            }
            else
            {
                FindObjectOfType<AudioPlayer>().PlayAudio(source, source.clip);
            }
            PlayerPrefs.SetFloat("Coins_Game", 0);
            FindObjectOfType<GameManager>().EndGame(0.1f);
        }
    }
}
