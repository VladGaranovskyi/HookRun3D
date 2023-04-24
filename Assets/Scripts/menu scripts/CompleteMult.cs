using System.Collections;
using UnityEngine;

public class CompleteMult : MonoBehaviour
{
    public float mult;
    public GameObject CompleteUI;
    public Coins_Menu coins;
    public Coins_Menu coins3x;
    [SerializeField] private CompletedLevel cl;
    [SerializeField] private Vector3 cameraPos;
    [SerializeField] private Transform confetti;
    [SerializeField] private GameObject parts;
    private GameObject cam;
    private bool IsEnd;

    void OnCollisionEnter(Collision other)
    {
        parts.SetActive(true);
        Destroy(other.gameObject);
        GameObject.FindWithTag("player").transform.localScale = new Vector3(4f, 4f, 4f);
        GameObject.FindWithTag("player").transform.position = new Vector3(0f, 29.81001f, 854.6709f);
        GameObject.FindWithTag("player").transform.eulerAngles = new Vector3(0f, 180f, 0f);
        FindObjectOfType<CompleteLevel>().enabled = false;
        //GameObject.FindWithTag("player").GetComponent<GrappleHuman>().enabled = false;
        Destroy(GameObject.FindWithTag("player").GetComponent<Rigidbody>());
        //FindObjectOfType<EndManager>().enabled = false;
        //Camera.main.GetComponent<FollowHuman>().enabled = false;
        Camera.main.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        Camera.main.GetComponent<Camera>().fieldOfView = 72f;
        confetti.gameObject.SetActive(true);
        confetti.transform.position = new Vector3(0f, 77f, 854.6709f);
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "On")
            {
                GameObject.Find("GameMusic").GetComponent<AudioSource>().volume = 0.075f;
            }
        }
        else
        {
            GameObject.Find("GameMusic").GetComponent<AudioSource>().volume = 0.075f;
        }
        IsEnd = true;
        StartCoroutine(CompleteCor());
        StartCoroutine(SoundCor());
    }

    void FixedUpdate()
    {
        if(cam == null)
        {
            cam = Camera.main.gameObject;
        }
        if (IsEnd && cam.transform.position != cameraPos)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, cameraPos, 2 * Time.deltaTime);
        }
    }

    IEnumerator CompleteCor()
    {
        yield return new WaitForSeconds(7f);
        CompleteUI.SetActive(true);
        cl.multiply = mult;
        coins.multiply = mult;
        coins3x.multiply = mult;
    }
    IEnumerator SoundCor()
    {
        yield return new WaitForSeconds(1f);
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "On")
            {
                GameObject.Find("FinishAudio").GetComponent<AudioSource>().Play();
            }            
        }
        else
        {
            GameObject.Find("FinishAudio").GetComponent<AudioSource>().Play();
        }
    }
}
