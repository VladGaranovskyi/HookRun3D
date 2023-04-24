using System.Collections;
using UnityEngine;
public class Zipline : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private float speedZip;
    [SerializeField] private GameObject Connect;
    [SerializeField] private AudioSource Grapple_sound;
    [SerializeField] private AudioSource Grapple_sound_jump;
    [SerializeField] private Material line_color;
    [SerializeField] private Rigidbody diversion;
    [SerializeField] private Vector3 forceZip;
    [SerializeField] private Transform connectPos;
    [SerializeField] private bool IsPlane;
    private Transform Connection;
    private LineRenderer lr;
    private bool l;
    private bool IsSoundPlayed = false;
    private bool hooking = false;
    private LineRenderer lineRenderer;
    private CharacterJoint joint;
    private bool IsReadyRotate;
    private bool IsStart;
    private Holder holder;
    private GameObject player;
    private Rigidbody rb;
    private GrappleHuman grap;
    private Animator anim;
    private Vector2 direction;
    private AudioPlayer ap;
    private Transform cam;
    private bool IsEnter;

    private void Start()
    {
        transform.position = start.position;
        transform.LookAt(end);
        holder = FindObjectOfType<Holder>();
        ap = FindObjectOfType<AudioPlayer>();
        cam = Camera.main.transform;
        lr = start.gameObject.GetComponent<LineRenderer>();
        if (!IsPlane)
        {
            lr.SetPosition(0, start.position);
            lr.SetPosition(1, end.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            player = GameObject.FindWithTag("player");
            anim = player.GetComponent<Animator>();
            grap = player.GetComponent<GrappleHuman>();
            grap.SetIsStart(false);
            if (grap.GetHooking())
            {
                grap.UnGrapple(false);
            }
            this.GetComponent<Collider>().enabled = false;
            transform.LookAt(end);
            Connection = grap.GetConnection();
            rb = player.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            IsEnter = true;
        }
    }
    private void FixedUpdate()
    {
        if(cam == null)
        {
            cam = Camera.main.transform;
        }
        if (IsStart)
        {
            transform.eulerAngles = new Vector3(-30f, 0f, 0f);
            transform.Translate(0f, 0f, 1f);
        }
        if (IsReadyRotate)
        {
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            IsReadyRotate = false;
        }
        if (IsEnter)
        {
            direction = holder.GetDirection();
            if ((player.transform.position - connectPos.position).sqrMagnitude >= 1f && !hooking)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, connectPos.position, 0.1f);
            }
            else
            {
                //if (holder.IsTapped() && (direction.x == 0f || IsDir) && !IsReadyRotate && !IsGrounded && !IsUnGrappled && (player.transform.position.y > 10f || hooking) && !anim.GetBool("FlipBool") && !grap.GetHooking() && (transform.position - end.position).sqrMagnitude >= 4f)// Êðþê
                //{
                transform.Translate(0f, 0f, speedZip);
                //IsDir = true;
                if (hooking == false)
                {
                    anim.SetBool("HookBool", true);
                    //player.transform.eulerAngles += new Vector3(45f, 0f/*Vector2.Angle(new Vector2(HookToSpawn.transform.position.x, HookToSpawn.transform.position.y - pointToGo), new Vector2(HookToSpawn.transform.position.x, player.transform.position.y - HookToSpawn.transform.position.y))*/, 0f);
                   // rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
                    anim.SetBool("FlipBool", false);
                }
                //rb.isKinematic = false;
                //rb.useGravity = true;
                if (l == false)
                {
                    player.AddComponent<LineRenderer>();
                    rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
                    l = true;
                }
                rb.AddForce(direction.x * Time.deltaTime / 37f, 0f, 0f, ForceMode.VelocityChange);
                hooking = true;
                if (lineRenderer == null)
                {
                    lineRenderer = player.GetComponent<LineRenderer>();
                }
                lineRenderer.startWidth = 0.03f;
                lineRenderer.endWidth = 0.05f;
                lineRenderer.material = line_color;
                lineRenderer.SetPosition(0, new Vector3(Connect.transform.position.x, Connect.transform.position.y, Connect.transform.position.z));
                lineRenderer.SetPosition(1, new Vector3(Connection.position.x, Connection.position.y, Connection.position.z));
                if (joint == null)
                {
                    joint = Connect.GetComponent<CharacterJoint>();
                }
                joint.connectedBody = rb;
                if (PlayerPrefs.HasKey("Sound"))
                {
                    if (!IsSoundPlayed && PlayerPrefs.GetString("Sound") == "On")
                    {
                        ap.PlayAudio(Grapple_sound, Grapple_sound.clip);
                        IsSoundPlayed = true;
                    }
                    else if ((!IsSoundPlayed && PlayerPrefs.GetString("Sound") == "Vib"))
                    {
                        Handheld.Vibrate();
                        IsSoundPlayed = true;
                    }
                }
                else if (!IsSoundPlayed)
                {
                    ap.PlayAudio(Grapple_sound, Grapple_sound.clip);
                    IsSoundPlayed = true;
                }
                //}
            }
            if ((transform.position - end.position).sqrMagnitude <= 4f)
            {
                //Destroy(lineRenderer);
                l = false;
                hooking = false;
                if (PlayerPrefs.HasKey("Sound"))
                {
                    if (IsSoundPlayed && PlayerPrefs.GetString("Sound") == "On")
                    {
                        ap.PlayAudio(Grapple_sound_jump, Grapple_sound_jump.clip);
                        IsSoundPlayed = false;
                    }
                    else if ((IsSoundPlayed && PlayerPrefs.GetString("Sound") == "Vib"))
                    {
                        Handheld.Vibrate();
                        IsSoundPlayed = false;
                    }
                }
                else if (IsSoundPlayed)
                {
                    ap.PlayAudio(Grapple_sound_jump, Grapple_sound_jump.clip);
                    IsSoundPlayed = false;
                }
                rb.AddForce(forceZip);
                cam.gameObject.GetComponent<FollowHuman>().ResetPivot(true);
                joint.connectedBody = diversion;
                joint = null;
                anim.SetBool("HookBool", false);
                StartCoroutine(CoroutineGrapple(0.8f));
                IsReadyRotate = true;
                //IsDir = false;
                int ia = Random.Range(0, 2);
                anim.SetInteger("Flipint", ia);
                if (IsPlane) IsStart = true;
                else { transform.position = start.position; }
                IsEnter = false;
                grap.SetIsStart(true);
            }
        }
    }

    private void LateUpdate()
    {
        if (IsEnter)
        {
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, new Vector3(Connect.transform.position.x, Connect.transform.position.y, Connect.transform.position.z));
                lineRenderer.SetPosition(1, new Vector3(Connection.position.x, Connection.position.y, Connection.position.z));
            }
        }
    }

    private IEnumerator CoroutineGrapple(float per)
    {
        anim.SetBool("FlipBool", true);
        yield return new WaitForSeconds(per);
        anim.SetBool("FlipBool", false);
        if (IsPlane) Destroy(gameObject);
    }

    public void SetSE(Transform s, Transform e)
    {
        start = s;
        end = e;
    }
}
