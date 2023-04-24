using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private AudioSource source;
    private AudioPlayer ap;
    public GameObject ps;
    //[SerializeField] private GameObject[] coin_UI;
    //private Transform tp;
    //private GameObject ob;
    //[SerializeField] private MeshRenderer mesh;

    void Start()
    {
        source = GameObject.Find("AudioCoin").GetComponent<AudioSource>();
        ap = FindObjectOfType<AudioPlayer>();
        this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        //tp = GameObject.Find("TouchPad").transform;
    }

    void OnTriggerEnter()
    {
        Instantiate(ps);
        ps.transform.position = this.transform.position;
        //ParticleSystem pss = ps.GetComponent<ParticleSystem>();
        //pss.Play();
        //int r = Random.Range(0, coin_UI.Length);
        //ob = Instantiate(coin_UI[r]);
        //ob.transform.SetParent(tp);
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
        PlayerPrefs.SetFloat("Coins_Game", PlayerPrefs.GetFloat("Coins_Game") + 1f);
        Destroy(this.gameObject);
        //mesh.enabled = false;
    }
}
