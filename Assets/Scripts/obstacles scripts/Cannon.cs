using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform direct;
    [SerializeField] private float koefficient;
    [SerializeField] private GameObject ball;
    [SerializeField] private float period;
    [SerializeField] private float scaleBall;
    private bool IsCor = true;
    [SerializeField] private AudioSource sound;
    [SerializeField] private ParticleSystem[] pses;
    private AudioPlayer ap;

    private void Start()
    {
        ap = FindObjectOfType<AudioPlayer>();
    }

    private void FixedUpdate()
    {
        if (IsCor) 
        {
            StartCoroutine(MainCor());
            pses[0].Play();
            pses[1].Play();
            pses[2].Play();
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

    private IEnumerator MainCor()
    {
        IsCor = false;
        GameObject _ball = Instantiate(ball);
        _ball.transform.position = direct.position;
        _ball.GetComponent<Rigidbody>().AddForce(direct.up * koefficient, ForceMode.Acceleration);
        _ball.transform.localScale = Vector3.one * scaleBall;
        yield return new WaitForSeconds(period);
        IsCor = true;

    }
}
