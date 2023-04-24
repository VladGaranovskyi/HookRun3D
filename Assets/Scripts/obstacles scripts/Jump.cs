using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private Vector3 jumpDirection;
    [SerializeField] private bool IsStop;
    [SerializeField] private AudioSource source;
    private AudioPlayer ap;
    private bool IsEnter;
    private float posToGo;
    private float startPos;
    private bool IsWent;

    void Awake()
    {
        ap = FindObjectOfType<AudioPlayer>();
    }

    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("player"))
        {
            Rigidbody rb = obj.gameObject.GetComponent<Rigidbody>();
            if (IsStop) rb.velocity = Vector3.zero;
            rb.AddForce(jumpDirection, ForceMode.Impulse);
            IsEnter = true;
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

    private void FixedUpdate()
    {
        if (IsEnter)
        {
            if (posToGo == 0f)
            {
                posToGo = this.transform.position.y + 1f;
                startPos = this.transform.position.y;
            }
            if (Mathf.Abs(this.transform.position.y - posToGo) > 0.1f && !IsWent)
                this.transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, posToGo, Time.deltaTime * 5f), transform.position.z);
            else
            {
                this.transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, startPos, Time.deltaTime * 5f), transform.position.z);
                IsWent = true;
            }
            if(IsWent && Mathf.Abs(this.transform.position.y - startPos) < 0.001f)
            {
                IsWent = false;
                IsEnter = false;
            }
        }
    }
}
