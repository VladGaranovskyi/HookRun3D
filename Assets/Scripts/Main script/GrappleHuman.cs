using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHuman : MonoBehaviour
{
    private float Speed = 1000f;
    public GameObject player;
    private Holder holder;
    public Material line_color;
    [SerializeField] private Transform Connection;
    private bool l = true;
    private bool hooking = false;
    [HideInInspector]
    public Rigidbody rb;
    private GameObject Connect;
    private GameObject hook;
    private AudioPlayer ap;
    private AudioSource Grapple_sound;
    private AudioSource Grapple_sound_jump;
    private bool IsSoundPlayed = false;
    public bool LetHook = true;
    private bool IsGrappled;
    private Vector3 hookStartPos;
    [HideInInspector]
    public bool CanGrapple;
    [HideInInspector]
    public bool IsTrigger;
    private bool IsUnGrappled;
    private LineRenderer lineRenderer;
    private MeshRenderer hook_mesh;
    private CharacterJoint joint;
    private Transform cam;
    private bool IsReadyRotate;
    [SerializeField] private GameObject HookToSpawn;
    [SerializeField] private List<Collider> hitBoxes = new List<Collider>();
    [SerializeField] private DeathTrigger triggerDeath;
    [SerializeField] private AudioSource soundRun;
    [SerializeField] private TrailRenderer[] trails;
    [SerializeField] private AudioSource soundWind;
    private float pointToGo;
    private Animator anim;
    private Vector2 direction;
    private bool IsGrounded;
    private float preVelocity_y;
    private float preVelocity_z;
    private bool IsRoll;
    private bool IsStart;
    private Transform zoneParticles;
    private ParticleSystem zonePartSys;
    private float _koefRun;
    private float _koefForce;
    private float _kRHook = 1f;
    private float _lastGroundPos;
    private GameManager gm;

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        cam = Camera.main.transform;
        player.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = player.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(-100, -100, -100));
        lineRenderer.SetPosition(1, new Vector3(-99, -99, -99));
        rb = gameObject.GetComponent<Rigidbody>();
        holder = FindObjectOfType<Holder>();
        ap = FindObjectOfType<AudioPlayer>();
        zoneParticles = GameObject.Find("zoneParticles").transform;
        zonePartSys = zoneParticles.gameObject.GetComponent<ParticleSystem>();
        if (PlayerPrefs.HasKey("RunK"))
            _koefRun = PlayerPrefs.GetInt("RunK") * 25;
        else
            _koefRun = 0;
        if (PlayerPrefs.HasKey("ForceK"))
            _koefForce = 2f + PlayerPrefs.GetInt("ForceK") / 3;
        else
            _koefForce = 2f;
        StartCoroutine(FCor());
        StartCoroutine(SCor());
        gm = FindObjectOfType<GameManager>();
    }


    void FixedUpdate()
    {
        if(cam == null)
        {
            cam = GameObject.Find("Main Camera(Clone)").transform;
        }
        if(holder == null)
        {
            holder = FindObjectOfType<Holder>();
        }
        if (IsGrounded) foreach (TrailRenderer t in trails) t.enabled = false;
        else if(!trails[2].enabled) foreach (TrailRenderer t in trails) t.enabled = true;
        direction = holder.GetDirection();
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
        if(IsGrounded && Mathf.Abs(player.transform.position.z - _lastGroundPos) > 50f && _lastGroundPos != 0f)       
            StartCoroutine(HookHint());
        
        if (holder.IsTapped())
        {
            if (hooking)
            {
                rb.AddForce(direction.x * Time.deltaTime / 27f, 0f, 0f, ForceMode.VelocityChange);                            
            }
            else if (IsGrounded && IsStart)
            {
                if (direction.y > -400f)
                {
                    rb.velocity = new Vector3(direction.x * Time.deltaTime, rb.velocity.y, (Speed + _koefRun) * Time.deltaTime);
                }
                else if(!IsRoll && LetHook)
                {
                    anim.SetBool("RunBool", false);
                    anim.SetBool("HookUpBool", true);
                    HookToSpawn = Instantiate(HookToSpawn);
                    Vector3 plpos = player.transform.position;
                    Connect = HookToSpawn.transform.GetChild(5).gameObject;
                    HookToSpawn.transform.position = new Vector3(plpos.x, plpos.y + 30f, plpos.z);
                    if (player.GetComponent<LineRenderer>() == null)
                    {
                        player.AddComponent<LineRenderer>();
                    }
                    lineRenderer = player.GetComponent<LineRenderer>();
                    lineRenderer.startWidth = 0.05f;
                    lineRenderer.endWidth = 0.07f;                    
                    lineRenderer.material = line_color;
                    lineRenderer.SetPosition(0, new Vector3(Connect.transform.position.x, Connect.transform.position.y, Connect.transform.position.z));
                    lineRenderer.SetPosition(1, new Vector3(Connection.position.x, Connection.position.y, Connection.position.z));
                    _lastGroundPos = 0f;
                    rb.AddForce(0f, 30f, 0f, ForceMode.Impulse);
                    StartCoroutine(CoroutineHookUp());
                }                             
            }
        }
        else if (hooking == false && IsStart)
        {
            if (IsGrounded)
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, (Speed + _koefRun) * Time.deltaTime);
            }
            else
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, (Speed + _koefRun) * Time.deltaTime / 1.5f * _kRHook);
            }
        }
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
        if (IsReadyRotate)
        {
            player.transform.rotation = Quaternion.Euler(0, 0, 0);           
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            IsReadyRotate = false;
        }
        if (IsUnGrappled)
        {
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.07f;
            lineRenderer.material = line_color;
            lineRenderer.SetPosition(0, new Vector3(Connect.transform.position.x, Connect.transform.position.y, Connect.transform.position.z));
            lineRenderer.SetPosition(1, new Vector3(Connection.position.x, Connection.position.y, Connection.position.z));
        }
        if (holder.IsTapped() && !IsReadyRotate && !IsGrounded && !IsUnGrappled && (player.transform.position.y > 10f || hooking) && !anim.GetBool("FlipBool") && IsStart && !anim.GetBool("KickBool") && LetHook)// Êðþê
        {
            if (hooking == false)
            {
                HookToSpawn = Instantiate(HookToSpawn);
                Vector3 plpos = player.transform.position;
                HookToSpawn.transform.position = new Vector3(plpos.x, plpos.y + 15f, plpos.z + 9f);
                pointToGo = HookToSpawn.transform.position.y - 6f;
                SetGrappleData(HookToSpawn.transform.root.gameObject.transform.GetChild(5).gameObject
                    , HookToSpawn.transform.root.gameObject.transform.GetChild(4).gameObject, HookToSpawn.transform.root.gameObject.transform.GetChild(4).Find("default1").gameObject
                    , HookToSpawn.transform.root.gameObject.transform.GetChild(6).gameObject.GetComponent<AudioSource>()
                    , HookToSpawn.transform.root.gameObject.transform.GetChild(7).gameObject.GetComponent<AudioSource>()
                    , new Vector3(HookToSpawn.transform.position.x, pointToGo, HookToSpawn.transform.position.z));
                anim.SetBool("HookBool", true);
                player.transform.eulerAngles += new Vector3(45f, 0f/*Vector2.Angle(new Vector2(HookToSpawn.transform.position.x, HookToSpawn.transform.position.y - pointToGo), new Vector2(HookToSpawn.transform.position.x, player.transform.position.y - HookToSpawn.transform.position.y))*/, 0f);
                rb.constraints = RigidbodyConstraints.FreezeRotationY;
                anim.SetBool("FlipBool", false);
            }
            if (Mathf.Abs(HookToSpawn.transform.position.y - pointToGo) > 0.1f)
            {
                HookToSpawn.transform.position = new Vector3(HookToSpawn.transform.position.x, Mathf.Lerp(HookToSpawn.transform.position.y, pointToGo, 0.1f), HookToSpawn.transform.position.z);
            }
            if (l == false)
            {
                gameObject.AddComponent<LineRenderer>();
                rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
                l = true;
            }
            hooking = true;
            if (hook_mesh == null)
            {
                hook_mesh = hook.GetComponent<MeshRenderer>();
            }
            hook_mesh.enabled = true;
            if (lineRenderer == null)
            {
                lineRenderer = player.GetComponent<LineRenderer>();
            }
            lineRenderer.startWidth = 0.03f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = line_color;
            if (!IsGrappled)
            {
                hook.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                IsGrappled = true;
            }
            else if (hook.transform.position != hookStartPos)
            {
                hook.transform.position = Vector3.Lerp(hook.transform.position, hookStartPos, 0.1f);
                lineRenderer.SetPosition(0, new Vector3(hook.transform.position.x, hook.transform.position.y, hook.transform.position.z));
                lineRenderer.SetPosition(1, new Vector3(Connection.position.x, Connection.position.y, Connection.position.z));
            }
            else
            {
                lineRenderer.SetPosition(0, new Vector3(Connect.transform.position.x, Connect.transform.position.y, Connect.transform.position.z));
                lineRenderer.SetPosition(1, new Vector3(Connection.position.x, Connection.position.y, Connection.position.z));
            }
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
            if (Connection.position.y < HookToSpawn.transform.position.y)
            {
                rb.AddForce(0f, (rb.velocity.y - preVelocity_y) * _koefForce, (rb.velocity.z - preVelocity_z) * _koefForce);
            }
            preVelocity_y = rb.velocity.y;
            preVelocity_z = rb.velocity.z;
        }
        else if (hooking && (!holder.IsTapped() || !IsStart))
        {
            UnGrapple();
        }
    }

    private void LateUpdate()
    {
        if (holder.IsTapped() && !IsReadyRotate && !IsGrounded && !IsUnGrappled && (player.transform.position.y > 10f || hooking) && !anim.GetBool("FlipBool") && IsStart && lineRenderer != null && !anim.GetBool("HookUpBool") && !anim.GetBool("KickBool") && LetHook)
        {
            if (hook.transform.position != hookStartPos)
            {
                //hook.transform.position = Vector3.Lerp(hook.transform.position, hookStartPos, 0.1f);
                lineRenderer.SetPosition(0, new Vector3(hook.transform.position.x, hook.transform.position.y, hook.transform.position.z));
                lineRenderer.SetPosition(1, new Vector3(Connection.position.x, Connection.position.y, Connection.position.z));
            }
            else
            {
                lineRenderer.SetPosition(0, new Vector3(Connect.transform.position.x, Connect.transform.position.y, Connect.transform.position.z));
                lineRenderer.SetPosition(1, new Vector3(Connection.position.x, Connection.position.y, Connection.position.z));
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            IsGrounded = true;
            anim.SetBool("RunBool", true);
            //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            StartCoroutine(CoroutineRoll());
            _lastGroundPos = player.transform.position.z;
        }
        else if (other.gameObject.CompareTag("Death"))
        {
            triggerDeath.ActivateDeath();
        }
    }

    private IEnumerator SCor()
    {
        yield return new WaitForSeconds(4f);
        IsStart = true;
    }

    private IEnumerator HookHint()
    {
        GameObject[] hints = gm.GetHints();
        foreach(GameObject h in hints)
        {
            h.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        foreach (GameObject h in hints)
        {
            h.SetActive(false);
        }
        _lastGroundPos = player.transform.position.z;
    }

    private IEnumerator GroundCor()
    {
        yield return new WaitForSeconds(0.5f);
        IsGrounded = false;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            StartCoroutine(GroundCor());
            _lastGroundPos = 0f;
            anim.SetBool("RunBool", false);
            if (PlayerPrefs.HasKey("Sound"))
            {
                if (PlayerPrefs.GetString("Sound") == "On")
                {
                    soundRun.Stop();
                }
                else if (PlayerPrefs.GetString("Sound") == "Vib")
                {
                    Handheld.Vibrate();
                }
            }
            else
            {
                soundRun.Stop();
            }
            //rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private IEnumerator CoroutineGrapple(float per)
    {
        anim.SetBool("FlipBool", true);
        if(_koefForce > 2f)
        {
            _kRHook = 1.5f + PlayerPrefs.GetInt("ForceK") / 10;
        }
        else
        {
            _kRHook = 1.5f;
        }
        yield return new WaitForSeconds(per);
        _kRHook = 1f;
        anim.SetBool("FlipBool", false);
    }

    private IEnumerator CoroutineRoll()
    {
        IsRoll = true;
        yield return new WaitForSeconds(1f);
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "On")
            {
                soundRun.Play();
            }
            else if (PlayerPrefs.GetString("Sound") == "Vib")
            {
                Handheld.Vibrate();
            }
        }
        else
        {
            soundRun.Play();
        }
        IsRoll = false;
    }

    private IEnumerator CoroutineHookUp()
    {
        IsUnGrappled = true;
        IsRoll = true;
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("HookUpBool", false);
        cam.gameObject.GetComponent<FollowHuman>().ResetPivot(true);
        HookToSpawn.transform.position = new Vector3(-1000f, -1000f, -1000f);
        lineRenderer.SetPosition(0, new Vector3(-100, -100, -100));
        lineRenderer.SetPosition(1, new Vector3(-99, -99, -99));
        yield return new WaitForSeconds(0.2f);
        IsUnGrappled = false;
    }
    private IEnumerator FCor()
    {
        yield return new WaitForSeconds(1.2f);
        rb.AddForce(0f, 1200f, 500f);
    }

    public void SetGrappleData(GameObject Connect1, GameObject hookchild1, GameObject hook1, AudioSource Grapple_sound1, AudioSource Grapple_sound_jump1, Vector3 hookStart)
    {
        Connect = Connect1;
        hook = hook1;
        Grapple_sound = Grapple_sound1;
        Grapple_sound_jump = Grapple_sound_jump1;
        hookStartPos = hookStart;
    }

    public void SetIsStart(bool b)
    {
        IsStart = b;
    }

    public bool GetIsUngr()
    {
        return IsGrounded;
    }

    public bool GetIsGr()
    {
        return IsUnGrappled;
    }

    public void UnGrapple(bool IsNotZ = true)
    {
        CanGrapple = false;
        IsGrappled = false;
        Destroy(lineRenderer);
        l = false;
        Rigidbody diversion = hook.GetComponent<Rigidbody>();
        if(hook_mesh != null)
        {
            hook_mesh.enabled = false;
            hook_mesh = null;
        }
        hooking = false;
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (IsSoundPlayed && PlayerPrefs.GetString("Sound") == "On")
            {
                ap.PlayAudio(Grapple_sound_jump, Grapple_sound_jump.clip);
                ap.PlayAudio(soundWind, soundWind.clip);
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
            ap.PlayAudio(soundWind, soundWind.clip);
            IsSoundPlayed = false;
        }
        FollowHuman fh = cam.gameObject.GetComponent<FollowHuman>();
        Debug.Log(rb.velocity.y);
        if(rb.velocity.y >= 18f) fh.ResetPivot(false);
        else fh.ResetPivot(true);
        joint.connectedBody = diversion;
        joint = null;
        HookToSpawn.transform.position = new Vector3(-1000f, -1000f, -1000f);
        if (IsNotZ)
        {
            StartCoroutine(CoroutineGrapple(0.8f));
            if (Connection.position.y < HookToSpawn.transform.position.y && IsStart)
            {
                rb.AddForce(0f, (rb.velocity.y - preVelocity_y) / Time.deltaTime * _koefForce, (rb.velocity.z - preVelocity_z) / Time.deltaTime * _koefForce);
            }
        }
        int ia = Random.Range(0, 2);
        anim.SetInteger("Flipint", ia);
        anim.SetBool("HookBool", false);
        IsReadyRotate = true;
    }

    public bool GetHooking()
    {
        return hooking;
    }

    public Transform GetConnection()
    {
        return Connection;
    }

    public List<Collider> GetColliders()
    {
        return hitBoxes;
    }

    public void MultiplySpeed(float mult)
    {
        Speed *= mult;
    }
}
