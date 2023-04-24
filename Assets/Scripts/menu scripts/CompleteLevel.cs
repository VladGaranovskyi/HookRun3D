using System.Collections;
using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
    private GameObject player;
    private GrappleHuman grapple;
    private Rigidbody rb;
    private Camera cam;
    private Holder holder;
    [SerializeField] private GameObject[] roulette;
    [SerializeField] private Transform hook;
    [SerializeField] private Material mat;
    [SerializeField] private Transform ball;
    [SerializeField] private float first;
    [SerializeField] private float second;
    [SerializeField] private float third;
    [SerializeField] private float fourth;
    [SerializeField] private float fifth;
    private LineRenderer lineRenderer;
    private Transform _connection;
    private Transform _cursor;
    private bool IsEnter;
    private bool IsCor;
    private bool IsReady;
    private bool IsMove = true;
    private bool IsCam;
    private Transform _pivot;
    private Animator _anim;
    private bool IsAnim;
    private Vector3 _forceBall;
    private Vector3 ballPos = new Vector3(0.1f, 54f, 833.7f);
    private Vector3 playerHitPos = new Vector3(0.5f, 48.596f, 838.5f);
    private float _cursorPos;
    private bool IsFollowBall;

    private void Start()
    {
        holder = FindObjectOfType<Holder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            IsEnter = true;
            player = other.gameObject;
            cam = Camera.main;
            _pivot = cam.transform.parent;
            rb = player.GetComponent<Rigidbody>();
            grapple = player.GetComponent<GrappleHuman>();
            _connection = grapple.GetConnection();
            if (grapple.GetHooking()) grapple.UnGrapple();
            grapple.SetIsStart(false);
            _anim = player.GetComponent<Animator>();
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            player.transform.position = new Vector3(0f, player.transform.position.y, transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        if (IsEnter)
        {
            if (grapple.GetIsUngr())
            {
                if (!IsCor)
                {
                    IsCor = true;
                    StartCoroutine(EnterCor());
                }
                if (holder.IsTapped() && IsReady)
                {
                    IsReady = false;
                    foreach (var r in roulette)
                    {
                        r.SetActive(false);
                    }
                    lineRenderer = player.GetComponent<LineRenderer>();
                    if (lineRenderer == null)
                    {
                        player.AddComponent<LineRenderer>();
                        lineRenderer = player.GetComponent<LineRenderer>();
                    }
                    lineRenderer.startWidth = 0.03f;
                    lineRenderer.endWidth = 0.05f;
                    lineRenderer.material = mat;
                    lineRenderer.SetPosition(0, _connection.position);
                    lineRenderer.SetPosition(1, hook.position);
                    _anim.SetBool("RunBool", false);
                    _anim.SetBool("HookUpBool", true);
                    StartCoroutine(AnimCor());
                    rb.AddForce((ballPos - player.transform.position) * 50f, ForceMode.Acceleration);
                    _cursorPos = Mathf.Abs(_cursor.position.x - 540f);
                    IsCam = true;
                    if (_cursorPos < 30f)
                    {
                        _forceBall = new Vector3(0f, 0f, first);
                    }
                    else if (_cursorPos < 86f)
                    {
                        _forceBall = new Vector3(0f, 0f, second);
                    }
                    else if (_cursorPos < 190f)
                    {
                        _forceBall = new Vector3(0f, 0f, third);
                    }
                    else if (_cursorPos < 280f)
                    {
                        _forceBall = new Vector3(0f, 0f, fourth);
                    }
                    else
                    {
                        _forceBall = new Vector3(0f, 0f, fifth);
                    }
                }
                else if (IsMove)
                {
                    rb.velocity = new Vector3(0f, rb.velocity.y, 1000f * Time.deltaTime);
                }
            }
            else
            {
                if((ball.position - player.transform.position).sqrMagnitude <= 4f && !IsFollowBall)
                {
                    player.transform.position = playerHitPos;
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    ball.gameObject.AddComponent<Rigidbody>().AddForce(_forceBall, ForceMode.Force);
                    cam.transform.GetComponent<FollowHuman>().enabled = false;
                    cam.transform.eulerAngles = new Vector3(30f, 0f, 0f);
                    cam.transform.parent = null;
                    Time.timeScale = 1f;
                    IsCam = false;
                    IsFollowBall = true;
                }
                if (IsFollowBall)
                {
                    cam.transform.position = ball.position + new Vector3(0f, 3f, -5f);
                }
            }
            if (IsAnim)
            {
                lineRenderer.SetPosition(0, _connection.position);
                lineRenderer.SetPosition(1, hook.position);
            }
            if (IsCam)
            {
                //cam.transform.position = Vector3.Lerp(cam.transform.position, _cameraPos, Time.deltaTime * 3f);
                _pivot.transform.eulerAngles = new Vector3(-20f, Mathf.Lerp(cam.transform.eulerAngles.y, 70f, Time.deltaTime * 3f), 0f);
            }
        }
    }

    private void LateUpdate()
    {
        if (IsAnim)
        {
            lineRenderer.SetPosition(0, _connection.position);
            lineRenderer.SetPosition(1, hook.position);
        }
    }

    private IEnumerator EnterCor()
    {
        yield return new WaitForSeconds(1f);
        IsReady = true;
        Time.timeScale = 0.5f;
        foreach (var r in roulette)
        {
            if(r.name == "Cursor")
            {
                _cursor = r.transform;
            }
            r.SetActive(true);
        }
        cam.GetComponent<FollowHuman>().ForbideRotate();
        _pivot.Rotate(-20f, -20f, 0f);
        IsMove = false;
        rb.velocity = new Vector3(0f, rb.velocity.y, 10f * Time.deltaTime);
    }

    private IEnumerator AnimCor()
    {
        IsAnim = true;
        yield return new WaitForSeconds(0.3f);
        IsAnim = false;
        _anim.SetBool("FinishBool", true);
    }
}
