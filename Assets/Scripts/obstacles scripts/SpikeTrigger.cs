using UnityEngine;
//using System.Collections;

public class SpikeTrigger : MonoBehaviour
{
    public AudioSource source;
    [SerializeField] private bool IsSaw;
    private GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider obj)
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
        if(obj.tag == "player" && !gm.GetIsRestarted())
        {
            StartCoroutine(gm.CoroutineRestartMenu());
            //StartCoroutine(CorTimescale());
        }
    }

    /*private IEnumerator CorTimescale()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(this.transform.root.gameObject);
        Time.timeScale = 0f;
    }*/
}
