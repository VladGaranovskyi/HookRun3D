using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private List<Collider> _colls = new List<Collider>();
    private Animator _anim;
    [SerializeField] private GameObject player;
    private GrappleHuman _grapple;
    private Transform _piv;
    private bool IsEnter;
    private FollowHuman _cam;
    private Rigidbody _rb;
    private Transform _follower;
    private GameManager _gm;

    private void Start()
    {
        _grapple = player.GetComponent<GrappleHuman>();
        _colls = _grapple.GetColliders();
        _anim = player.GetComponent<Animator>();
        _piv = GameObject.Find("Pivot").transform;
        _cam = Camera.main.GetComponent<FollowHuman>();
        _rb = player.GetComponent<Rigidbody>();
        _gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death") || other.CompareTag("Car"))
        {
            ActivateDeath();
        }
    }

    private void FixedUpdate()
    {
        if (IsEnter)
        {
            _cam.transform.position = _follower.transform.position + new Vector3(0f, 5f, -7f);
            _cam.transform.LookAt(_follower.transform);
            _rb.AddForce(0f, 1f, 0f);
        }
    }

    private void LateUpdate()
    {
        if (IsEnter && _grapple.enabled) 
        {
            _grapple.enabled = false;
        }
    }

    public void ActivateDeath()
    {
        foreach (Collider c in _colls)
        {
            if (c.name == "pelvis") _follower = c.transform;
            c.isTrigger = false;
        }
        //player.GetComponent<Collider>().enabled = false;
        _grapple.SetIsStart(false);
        _cam.enabled = false;
        _cam.transform.parent = null;
        _cam.transform.LookAt(player.transform);
        _anim.enabled = false;
        _anim.avatar = null;
        Rigidbody rb = _grapple.GetConnection().GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        _rb.velocity = Vector3.zero;
        player.transform.GetChild(1).parent = _follower;
        StartCoroutine(_gm.CoroutineRestartMenu());
        IsEnter = true;
    }
}
