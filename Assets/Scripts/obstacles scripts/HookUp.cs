using System.Collections;
using UnityEngine;

public class HookUp : MonoBehaviour
{
    [SerializeField] private Transform anchor;
    [SerializeField] private Transform forcePoint;
    [SerializeField] private float forceSpeed;
    [SerializeField] private float per;
    [SerializeField] private Material mat;
    private Transform _player;
    private Transform _connection;
    private GrappleHuman _grap;
    private Rigidbody _rb;
    private Animator _anim;
    private LineRenderer _lr;
    private bool IsEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            _player = other.transform;
            _grap = _player.GetComponent<GrappleHuman>();
            _connection = _grap.GetConnection();
            _rb = _player.GetComponent<Rigidbody>();
            _anim = _player.GetComponent<Animator>();
            _grap.SetIsStart(false);
            if (_grap.GetHooking()) _grap.UnGrapple(false);
            _anim.SetBool("HookUpBool", true);
            _rb.AddForce((forcePoint.position - _player.position) * forceSpeed, ForceMode.Impulse);
            StartCoroutine(LineCor());
            IsEnter = true;
        }
    }

    private void FixedUpdate()
    {
        if (IsEnter)
        {
            if(_lr == null)
            {
                _player.gameObject.AddComponent<LineRenderer>();
                _lr = _player.gameObject.GetComponent<LineRenderer>();
                _lr.startWidth = 0.03f;
                _lr.endWidth = 0.05f;
                _lr.material = mat;
            }
            _lr.SetPosition(0, _connection.position);
            _lr.SetPosition(1, anchor.position);
        }
    }

    private IEnumerator LineCor()
    {
        yield return new WaitForSeconds(per);
        IsEnter = false;
        Destroy(_lr);
        _anim.SetBool("HookUpBool", false);
        yield return new WaitForSeconds(per * 2);
        _grap.SetIsStart(true);
    }
}
