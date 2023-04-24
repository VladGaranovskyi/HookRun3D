using UnityEngine;

public class Grapple : MonoBehaviour
{
    public GameObject player;
    private Holder holder;
    public Material line_color;
    public PlayerCollision pc;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public bool CanGrapple;
    [HideInInspector]
    public bool IsTrigger;
    [SerializeField] private AudioSource rollSound;
    private Transform zoneParticles;
    private ParticleSystem zonePartSys;
    private Transform cam;
    private Vector3 prePos;
    private Transform Pivot;
    private float deadzone = 10f;
    private Vector3 newDir;
    private Vector3 preDirection;
    private Vector3 nDirection;
    private const float limitRoll = 0.7071067811865475244f;
    private bool IsStarted;
    private AudioPlayer ap;
    private Vector3 _camNRight;
    private Vector3 _camNForward;

    void Start()
    {
        cam = Camera.main.transform;
        ap = FindObjectOfType<AudioPlayer>();
        player.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = player.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(-100, -100, -100));
        lineRenderer.SetPosition(1, new Vector3(-99, -99, -99));
        rb = gameObject.GetComponent<Rigidbody>();
        holder = FindObjectOfType<Holder>();
        deadzone = Mathf.Pow(Screen.height / deadzone, 2);
        zoneParticles = GameObject.Find("zoneParticles").transform;
        zonePartSys = zoneParticles.gameObject.GetComponent<ParticleSystem>();
    }


    void FixedUpdate()
    {
        if(Pivot == null)
            Pivot = GameObject.Find("Pivot").transform;
        if (prePos == null)
        {
            prePos = Pivot.position;
        }
        if (holder.IsTapped())
        {
            Vector2 direction = holder.GetDirection();
            if (direction.x > 777f)
            {
                direction.x = 777f;
            }
            else if (direction.x < -777f)
            {
                direction.x = -777f;
            }
            if (direction.y > 777f)
            {
                direction.y = 777f;
            }
            else if (direction.y < -777f)
            {
                direction.y = -777f;
            }
            if (direction.sqrMagnitude >= deadzone)
            {
                if (pc.IsGrounded())
                {
                    if (!IsStarted)
                    {
                        _camNRight = cam.transform.right;
                        _camNForward = cam.transform.forward;
                    }
                    newDir = _camNRight * (direction.x * 12f * Time.deltaTime) + _camNForward * (direction.y * 12f * Time.deltaTime);
                    nDirection = newDir.normalized;
                    if (!IsStarted)
                    {
                        if (PlayerPrefs.HasKey("Sound"))
                        {
                            if (PlayerPrefs.GetString("Sound") == "On")
                            {
                                rollSound.Play();
                            }
                            else if (PlayerPrefs.GetString("Sound") == "Vib")
                            {
                                Handheld.Vibrate();
                            }
                        }
                        else
                        {
                            rollSound.Play();
                        }
                        if (Vector3.Dot(nDirection, preDirection) < limitRoll) rb.AddForce(-rb.velocity / 2f * rb.mass, ForceMode.Impulse);
                        IsStarted = true;
                    }
                    rollSound.volume = (rb.velocity.sqrMagnitude <= 777f ? rb.velocity.sqrMagnitude / 100f : 0.777f);
                }
                else
                {
                    if (!IsStarted)
                    {
                        rollSound.Stop();
                        _camNRight = cam.transform.right;
                        _camNForward = cam.transform.forward;
                        IsStarted = true;
                    }
                    newDir = _camNRight * (direction.x * 1.5f * Time.deltaTime) + _camNForward * (direction.y * 1.5f * Time.deltaTime);
                    nDirection = newDir.normalized;
                }
                rb.AddForce(newDir);
                preDirection = nDirection;
            }
        }
        else IsStarted = false;
        if (player.transform.position.x < -7.7f)
        {
            rb.velocity = new Vector3(5f, rb.velocity.y, rb.velocity.z);
            zoneParticles.position = new Vector3(-8f, player.transform.position.y, player.transform.position.z);
            zonePartSys.Play();
        }
        else if (player.transform.position.x > 7.7f)
        {
            rb.velocity = new Vector3(-5f, rb.velocity.y, rb.velocity.z);
            zoneParticles.position = new Vector3(8f, player.transform.position.y, player.transform.position.z);
            zonePartSys.Play();
        }
        prePos = Pivot.position;
    }

    public Vector2 GetNewDirection()
    {
        return newDir;
    }
}
